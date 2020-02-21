using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class myBaloonSpawner : MonoBehaviour
{
    [Header("Количество шаров")]
    public int baloonAmount = 3;
    [HideInInspector]
    public int answerBalloon;
    [Header("Время на решение")]
    public float durationTillExpire = 10f;
    [Header("Число примеров")]
    public int exerciseAmount = 10;

    [Header("Начальное ускорение шаров")]
    public Vector3 initForce = new Vector3(0, 9.9f, 0);
    [HideInInspector]
    public GameObject mistakes;
    [HideInInspector]
    public GameObject successes;
    [HideInInspector]
    public GameObject outcomeSign;

    [HideInInspector]
    public exampleGenerator exersize;

    public GameObject balloonPrefab;
    [HideInInspector]
    public GameObject title;
    [HideInInspector]
    public GameObject timer;
    [HideInInspector]
    public GameObject weeble;

    [HideInInspector]
    public bool spawnAtStartup = true;

    public bool playSounds = true;
    public SoundPlayOneshot inflateSound;
    public SoundPlayOneshot stretchSound;
    public enum spawnForms { Circle, Line, FullRandom };

    [Header("Форма появления шаров")]
    public spawnForms spawnForm = spawnForms.Circle;
    [Header("Размер шара")]
    public float scale = 2f;
    [Header("Расстояние между шарами")]
    public float distBetweenBalloons = 1.5f;
    [Header("Расстояние до шаров")]
    public float distanceToBalloons = 10f;
    [Header("Начальная точка генерации шаров")]
    public Vector3 initPoint = new Vector3(0, 0, 0);
    [Header("Доля случайности по каждому измерению")]
    public Vector3 randDev = new Vector3(0.25f, 0, 0.25f);



    //-------------------------------------------------
    void Start()
    {
        if (balloonPrefab == null)
        {
            return;
        }


        if (spawnAtStartup)
        {
            SpawnBalloons();
        }
    }


    public void popBalloons()
    {
        GameObject[] balloonsToPop = GameObject.FindGameObjectsWithTag("balloonDestroy");
        foreach (GameObject bal in balloonsToPop)
        {
            Destroy(bal);
        }

    }
    public void onCorrectPop()
    {
        successes.GetComponent<successesScript>().Successes += 1;
        popBalloons();
        SpawnBalloons();
    }

    public void onMistakePop()
    {
        mistakes.GetComponent<mistakesScript>().Mistakes += 1;
        popBalloons();
        SpawnBalloons();
    }
    public void onExpirePop()
    {
        mistakes.GetComponent<mistakesScript>().Mistakes += 1;
        SpawnBalloons();
    }

    void onGameEnd()
    {
        //replace reset to the beginning of new game neither end of previous
        title.GetComponent<exerciseVisualise>().showResults
            (mistakes.GetComponent<mistakesScript>().Mistakes, successes.GetComponent<successesScript>().Successes, timer.GetComponent<timerVisulize>().curTime);
        timer.GetComponent<timerVisulize>().resetTimer();
        timer.SetActive(false);

        mistakes.GetComponent<mistakesScript>().Mistakes = 0;
        mistakes.SetActive(false);

        successes.GetComponent<successesScript>().Successes = 0;
        successes.SetActive(false);

        weeble.SetActive(true);
    }

    string addSpace(int numberLength)
    {
        switch (numberLength)
        {
            case 1: return "   ";
            case 2: return "  ";
            case 3: return "";
            default: return "  ";
        }
    }

    void loadAddToTextureObject(ref AddTextToTexture attt)
    {
        attt.textPlacementY = 60;
        attt.textureID = "_MainTex";
        attt.useSharedMaterial = false;
        attt.characterSize = 0.45f;
        attt.customFont = Resources.Load<Font>("Font/CustomFontArial");
        attt.fontCountX = 10;
        attt.fontCountY = 10;
        attt.lineSpacing = 1;
        attt.decalTextureSize = 128;
    }

    void initUI()
    {
        timer.SetActive(true);
        timer.GetComponent<timerVisulize>().startTimer();
        mistakes.SetActive(true);
        successes.SetActive(true);
    }

    public void SpawnBalloons()
    {
        if (mistakes.GetComponent<mistakesScript>().Mistakes + successes.GetComponent<successesScript>().Successes >= exerciseAmount)
        {
            onGameEnd();
            return;
        }

        answerBalloon = Random.Range(0, baloonAmount);
        exersize.generateExample();
        title.GetComponent<exerciseVisualise>().writeExersise();
        Stack<int> wrongAnswers = exersize.generateWrongAnswers(baloonAmount - 1);
        float baseAngle = Mathf.PI / 2 + (baloonAmount - 1) * distBetweenBalloons / (2*distanceToBalloons);
        Vector3 pos;

        for (int i = 0; i < baloonAmount; i++)
        {
            if (balloonPrefab == null)
            {
                return;
            }

            switch (spawnForm)
            {
                case spawnForms.Circle:
                    {
                        pos = new Vector3(initPoint.x + distanceToBalloons * Mathf.Cos(baseAngle), initPoint.y, initPoint.z + distanceToBalloons * Mathf.Sin(baseAngle));
                        pos += new Vector3(Random.Range(-randDev.x, randDev.x), Random.Range(-randDev.y, randDev.y), Random.Range(-randDev.z, randDev.z));
                        baseAngle -= distBetweenBalloons / distanceToBalloons;
                        break;
                    }
                case spawnForms.Line:
                    {
                        pos = new Vector3(initPoint.x - distBetweenBalloons * (baloonAmount - 1) / 2 + distBetweenBalloons * i, initPoint.y, initPoint.z + distanceToBalloons);
                        pos += new Vector3(Random.Range(-randDev.x, randDev.x), Random.Range(-randDev.y, randDev.y), Random.Range(-randDev.z, randDev.z));
                        break;
                    }
                case spawnForms.FullRandom:
                    {
                        pos = new Vector3(initPoint.x + Random.Range(-0.8f*distanceToBalloons, 0.8f * distanceToBalloons), initPoint.y + 0.2f * distanceToBalloons + Random.Range(-0.7f * 0.2f * distanceToBalloons, 0.7f * 0.2f * distanceToBalloons), initPoint.z + distanceToBalloons + Random.Range(-0.8f * distanceToBalloons, 0.8f * distanceToBalloons));
                        break;
                    }
                default:
                    {
                        throw new System.Exception("unappropriate spawnForm");
                    }
            }
            
            GameObject balloon = Instantiate(balloonPrefab, pos, Quaternion.identity) as GameObject;
            balloon.transform.localScale = new Vector3(scale, scale, scale);
            balloon.transform.localRotation = Quaternion.Euler(0, 256, 0);
            AddTextToTexture attt = balloon.AddComponent<AddTextToTexture>();


            loadAddToTextureObject(ref attt);

            balloon.GetComponent<mySignedBaloon>().isAnswer = (i == answerBalloon);
            if ((i == answerBalloon))
                attt.text = exersize.answer.ToString() + " " + addSpace(exersize.answer.ToString().Length) + exersize.answer.ToString();
            else
            {
                string wrong = wrongAnswers.Pop().ToString();
                attt.text = wrong + " " + addSpace(wrong.Length) + wrong;
            }

            balloon.GetComponent<mySignedBaloon>().onCorrectPop.AddListener(outcomeSign.GetComponent<outcomeText>().onRightWrite);
            balloon.GetComponent<mySignedBaloon>().onMistakePop.AddListener(outcomeSign.GetComponent<outcomeText>().onFailWrite);
            balloon.GetComponent<mySignedBaloon>().onExpirePop.AddListener(outcomeSign.GetComponent<outcomeText>().onExpireWrite);

            balloon.GetComponent<mySignedBaloon>().onCorrectPop.AddListener(onCorrectPop);
            balloon.GetComponent<mySignedBaloon>().onMistakePop.AddListener(onMistakePop);
            balloon.GetComponent<mySignedBaloon>().onExpirePop.AddListener(onExpirePop);
            balloon.GetComponent<mySignedBaloon>().lifetime = durationTillExpire;

            if (playSounds && balloon.GetComponent<mySignedBaloon>().isAnswer)
            {
                if (inflateSound != null)
                {
                    inflateSound.Play();
                }
                if (stretchSound != null)
                {
                    stretchSound.Play();
                }
            }
            balloon.GetComponent<ConstantForce>().force = initForce;
        }
        return;

    }

    public void SpawnBalloonFromEvent()
    {
        initUI();
        SpawnBalloons();
    }
}

