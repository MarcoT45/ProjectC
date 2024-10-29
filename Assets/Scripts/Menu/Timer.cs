using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    void Start()
    {
        GameManager.Instance.SetRunTimerIsActive(true);
    }

    void Update()
    {
        float timer = GameManager.Instance.GetRunTimer();
        TimeSpan time = TimeSpan.FromSeconds(timer);
        text.text = time.Minutes.ToString() + " : " + time.Seconds.ToString();
    }
}
