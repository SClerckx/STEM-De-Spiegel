using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceTimer : MonoBehaviour
{
    Text timerText;

    public void EditTimer(float timeRemaining)
    {
        timerText = GetComponent<Text>();
        timerText.text = (timeRemaining + 1).ToString();
    }
}
