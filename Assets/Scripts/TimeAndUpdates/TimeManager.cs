using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public delegate void OnDayTick();
    public static event OnDayTick dayTickSend;

    public delegate void OnMonthTick();
    public static event OnMonthTick monthTickSend;

    public delegate void OnYearTick();
    public static event OnYearTick yearTickSend;

    public int day;
    public int month;
    public int year;

    private static readonly int[] daysPerMonth = new[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

    public float timeMultiplier;
    private float timeSinceTick;

    void Start()
    {
        day = 1;
        month = 1;
        year = 1480;
    }

    void Update()
    {
        float scaledDeltaTime = Time.deltaTime * timeMultiplier;
        timeSinceTick += scaledDeltaTime;

        while (timeSinceTick >= 1f)
        {
            timeSinceTick -= 1f;
            DayTick();
        }
    }

    public void DayTick()
    {
        day += 1;
        int maxDays = daysPerMonth[month - 1];

        if (month == 2 && year % 4 == 0)
        {
            maxDays = 29;
        }

        if (day > maxDays)
        {
            MonthTick();
        }

        dayTickSend?.Invoke();
    }

    public void MonthTick()
    {
        day = 1;
        month += 1;

        if (month > 12)
        {
            YearTick();
        }

        monthTickSend?.Invoke();
    }

    public void YearTick()
    {
        month = 1;
        year += 1;

        yearTickSend?.Invoke();
    }
}