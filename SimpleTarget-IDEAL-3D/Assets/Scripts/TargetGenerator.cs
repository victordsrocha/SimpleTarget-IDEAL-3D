using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TargetGenerator : MonoBehaviour
{
    public LayerMask obstacleMask;
    public Target targetPrefab;
    
    // A lógica para destruir o alvo após timeToCatch segundos não funciona quando Time.captureFramerate é alterado
    // public float timeToCatch = 60f;
    private Target _target;

    private void Start()
    {
        _target = FindObjectOfType<Target>();
    }

    private void LateUpdate()
    {
        if (_target == null)
        {
            _target = InstantiateNewTarget();
        }

        /*
        if (Time.time - _target.birthTime > timeToCatch)
        {
            _target.DestroyTarget();
        }
        */

    }

    private Target InstantiateNewTarget()
    {
        Target newTarget = null;

        float x = Random.Range(-18f, 18f);
        float z = Random.Range(-8f, 8f);
        Vector3 birthSpot = new Vector3(x, 0.6f, z);

        Collider[] overlapSphere = Physics.OverlapSphere(birthSpot, 2f, obstacleMask);
        if (overlapSphere.Length == 0)
        {
            newTarget = Instantiate(targetPrefab, birthSpot, Quaternion.identity);
            newTarget.birthTime = Time.time;
        }

        return newTarget;
    }
}