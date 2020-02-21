using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class exampleGenerator : MonoBehaviour
{
    public float rangeFrom = 0.5f;
    public float rangeTo = 1.5f;
    [HideInInspector]
    public int answer{ get;set; }
    [HideInInspector]
    public string stringExample { get;set; }

    public Stack<int> generateWrongAnswers(int amount)
    {
        Stack<int> wrongAnswers = new Stack<int>(amount);
        for (int i = 0; i < amount;i++)
        {
            int newAns = Mathf.RoundToInt(Random.Range(answer * rangeFrom, answer * rangeTo));
            //kostyl newAns < 100 because texture generator gets ugly with 3-digit numbers
            if (wrongAnswers.Contains(newAns) || newAns == answer || newAns >= 100)
                i--;
            else
                wrongAnswers.Push(newAns);
        }
        return wrongAnswers;
        
    }

    public void generateExample(float difficultLevel =1)
    {
        List<int> numbers = new List<int>(2);

        if (difficultLevel==1)
        {
            for (int i = 0; i < 2; i++)
                    numbers.Add(Random.Range(2, 10));
            answer = numbers[0] * numbers[1]; 
            stringExample = numbers[0].ToString() + " * " + numbers[1].ToString() + " =";
        }
    }



}
