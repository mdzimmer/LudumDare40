using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Currency : MonoBehaviour
{
    public int goalValue = 0;
    int curValue = 0;
    Text text;

    float TICK_TIME = 0.1f;

    void Start()
    {
        StartCoroutine(UpdateValue());
        text = GetComponent<Text>();
        UpdateDisplay();
    }

    public void IncrementValue(int amt)
    {
        goalValue += amt;
    }

    void UpdateDisplay()
    {
        text.text = curValue + " / 100 gold";
    }

    IEnumerator UpdateValue()
    {
        float elapsedTime = 0.0f;
        while (true)
        {
            if (curValue != goalValue)
            {
                if (elapsedTime >= TICK_TIME)
                {
                    curValue += curValue < goalValue ? 1 : -1;
                    UpdateDisplay();
                    elapsedTime = 0.0f;
                }
                elapsedTime += Time.deltaTime;
            } else
            {
                elapsedTime = 0.0f;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
