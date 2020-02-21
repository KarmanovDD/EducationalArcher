using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class exerciseVisualise : MonoBehaviour
{
    public exampleGenerator exercise;
    public Color32 exerciseColor = new Color32(255, 255, 200, 255);

    void Start()
    {
        GetComponent<TMPro.TextMeshPro>().text = "Shoot the target to begin the game";
        GetComponent<TMPro.TextMeshPro>().faceColor = exerciseColor;
        GetComponent<TMPro.TextMeshPro>().outlineWidth = 0.2f; 
    }

    public void showResults(int mistakes, int successes, int spentTime)
    {
        GetComponent<TMPro.TextMeshPro>().text = "Your result:"+"\nSuccesses: " + successes + "\nMistakes: " + mistakes + "\nTime: " + spentTime + "\n\nShoot the target to begin new game";
    }

    public void writeExersise()
    {
        GetComponent<TMPro.TextMeshPro>().text = exercise.stringExample;
    }
}
