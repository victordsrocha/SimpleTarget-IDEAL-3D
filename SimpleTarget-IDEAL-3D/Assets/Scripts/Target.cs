using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Target : MonoBehaviour
{
    public void DestroyTarget()
    {
        Destroy(gameObject);
    }
}