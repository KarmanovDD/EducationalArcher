using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System;


public class outcomeText : MonoBehaviour
{
    public float fadeTime = 5f;
    public float initScale = 0.001f;
    public float finishScale = 5f;
    public Color32 correctColor = new Color32(0, 255,0,0);
    public Color32 mistakeColor = new Color32(255, 0, 0, 0);
    public Color32 expiredColor = new Color32(255, 50, 50, 0);


    byte curAlpha = 0;
    float curScale;
    float initTime;

    bool started = false;
    bool isTextUpdated = false;
    enum outcome {Wrong, Expire, Right};
    outcome outcomeSign;

    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<TMPro.TextMeshPro>().faceColor = new Color32(0, 0, 0, 0);
        MeshRenderer rend = GetComponent<MeshRenderer>();
        rend.material.shaderKeywords = null;
    }

    void Update()
    {
        if (started)
        {
            GetComponent<MeshRenderer>().enabled = true;
            switch (outcomeSign)
            {
                case outcome.Right:
                    {
                        flushText("Right!", correctColor);
                        break;

                    }
                case outcome.Wrong: 
                    {
                        flushText("Wrong!", mistakeColor);
                        break; 
                    }
                case outcome.Expire: 
                    {
                        flushText("Time's up!", expiredColor);
                        break; 
                    }
            }
        }
    }

    void flushText(string text, Color32 textColor)
    {
        if (!isTextUpdated)
        {
            GetComponent<TMPro.TextMeshPro>().text = text;
            isTextUpdated = true;
        }

        if (curAlpha > 0)
        {
            if (Mathf.RoundToInt(255 - 255 * (Time.time - initTime) / fadeTime) > 0)
                curAlpha = Convert.ToByte(Mathf.RoundToInt(255 - 255 * Mathf.Pow((Time.time - initTime) / fadeTime, 1f / 2f)));
            else
                curAlpha = 0;
            curScale = initScale + finishScale / fadeTime * (Time.time - initTime);
            GetComponent<TMPro.TextMeshPro>().faceColor = new Color32(textColor.r, textColor.g, textColor.b, curAlpha);
            transform.localScale = new Vector3(curScale, curScale, curScale);
        }
        else
        {
            started = false;
            isTextUpdated = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void resetAndStart()
    {
        started = true;
        initTime = Time.time;
        curScale = initScale;
        curAlpha = 255;
    }

    public void onRightWrite()
    {
        outcomeSign = outcome.Right;
        resetAndStart();
    }
    public void onFailWrite()
    {
        outcomeSign = outcome.Wrong;
        resetAndStart();
    }
    public void onExpireWrite()
    {
        outcomeSign = outcome.Expire;
        resetAndStart();
    }
}
