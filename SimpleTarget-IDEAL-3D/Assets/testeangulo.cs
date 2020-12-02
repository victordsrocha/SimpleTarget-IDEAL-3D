using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testeangulo : MonoBehaviour
{
    public Transform other;
    public float angle;

    private void Update()
    {
        angle = CalcAngle();
    }

    float CalcAngle()
    {
        var myDir = transform.forward;
        var otherDir = other.forward;

        myDir.y = 0;
        otherDir.y = 0;

        return Vector3.Angle(myDir, otherDir);
    }
}