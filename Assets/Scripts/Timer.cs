using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    public bool timerActive;
    private float currentTime;
    public TMP_Text currentTimeText;

    void Update()
    {
        if (timerActive)
        {
            currentTime = currentTime - Time.deltaTime;
            {
                if (currentTime <= 0)
                    timerActive = false;
            }
        }
        TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = timeSpan.Seconds.ToString();
    }

    public void StrtTimer(int newTime)
    {
        timerActive = true;
        currentTime = newTime;
    }

    public void StopTimer()
    {
        timerActive = false;
    }
}
