using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaleScript : MonoBehaviour
{
    public void SetTimeScale(float newValue)
    {
        Time.timeScale = newValue;
    }

    public void SetFrameRate(float newValue)
    {
        Time.captureFramerate = (int)newValue;
        //Application.targetFrameRate = (int)newValue;
    }
}