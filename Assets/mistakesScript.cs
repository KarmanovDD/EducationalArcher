using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mistakesScript : MonoBehaviour
{
    public Color32 mistakesColor = new Color32(255, 0, 0, 255);

    int mistakes;
    bool updated = true;
    public int Mistakes 
    {
        get
        {
            return mistakes;
        } 
        set
        {
            updated = false;
            mistakes = value;
        }
    }
    void Start()
    {
        GetComponent<TMPro.TextMeshPro>().text = "Mistakes: " + 0.ToString();
        GetComponent<TMPro.TextMeshPro>().faceColor = mistakesColor;
        GetComponent<TMPro.TextMeshPro>().outlineWidth = 0.2f;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!updated)
        {
            GetComponent<TMPro.TextMeshPro>().text = "Mistakes: " + mistakes.ToString();
            updated = true;
        }

    }
}
