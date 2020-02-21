using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class successesScript : MonoBehaviour
{
    public Color32 successesColor = new Color32(0, 255, 0, 255);

    int successes;
    bool updated = true;
    public int Successes
    {
        get
        {
            return successes;
        }
        set
        {
            updated = false;
            successes = value;
        }
    }
    void Start()
    {
        GetComponent<TMPro.TextMeshPro>().text = "Successes: " + 0.ToString();
        GetComponent<TMPro.TextMeshPro>().faceColor = successesColor;
        GetComponent<TMPro.TextMeshPro>().outlineWidth = 0.2f;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!updated)
        {
            GetComponent<TMPro.TextMeshPro>().text = "Successes: " + successes.ToString();
            updated = true;
        }

    }
}
