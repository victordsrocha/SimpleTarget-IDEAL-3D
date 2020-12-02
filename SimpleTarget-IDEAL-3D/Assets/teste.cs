using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teste : MonoBehaviour
{
    private bool podeContinuar;
    private int x;
    
    void Start()
    {
        x = 1;
        podeContinuar = false;
        StartCoroutine(testeCo());
    }

    IEnumerator testeCo()
    {
        x = 2;
        StartCoroutine(tempinho());
        yield return new WaitUntil(()=> podeContinuar);
        Debug.Log(x);
    }

    IEnumerator tempinho()
    {
        yield return new WaitForSeconds(0.5f);
        x = 3;
        podeContinuar = true;
    }
}
