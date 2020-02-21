using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class timerVisulize : MonoBehaviour
{
    bool isStarted = false;
    float startTime;
    public Color32 timerColor = new Color32(255, 255, 0, 255);
    public int curTime { get;private set; }

    void Start()
    {
        GetComponent<TMPro.TextMeshPro>().text = 0.ToString();
        GetComponent<TMPro.TextMeshPro>().faceColor = timerColor;
        GetComponent<TMPro.TextMeshPro>().outlineWidth = 0.2f;
        gameObject.SetActive(false);
    }
    public void resetTimer()
    {
        isStarted = false;
        startTime = 0;
        GetComponent<TMPro.TextMeshPro>().text = "Time: " + startTime.ToString();
    }

    public void startTimer()
    {
        startTime = Time.time;
        isStarted = true;
    }
    void Update()
    {
        if (isStarted)
        {
            curTime = Mathf.RoundToInt(Time.time - startTime);
            GetComponent<TMPro.TextMeshPro>().text = "Time: " + curTime.ToString();
        }
    }
}
