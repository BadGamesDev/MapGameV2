using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Calendar : MonoBehaviour
{
    public TimeManager timeManager;
    public TMP_Text calendarText;

    public void Update()
    {
        calendarText.text = timeManager.day.ToString("D2") + " / " + timeManager.month.ToString("D2") + " / " + timeManager.year.ToString();
    }

    public void chooseSpeed0()
    {
        timeManager.timeMultiplier = 0;
    }
    public void chooseSpeed1()
    {
        timeManager.timeMultiplier = 1;
    }
    public void chooseSpeed2()
    {
        timeManager.timeMultiplier = 2;
    }
    public void chooseSpeed3()
    {
        timeManager.timeMultiplier = 4;
    }
    public void chooseSpeed4()
    {
        timeManager.timeMultiplier = 8;
    }
}

