using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TargetGenerator : MonoBehaviour
{
    public LayerMask obstacleMask;
    public Target targetPrefab;
    private Target target;

    private void Start()
    {
        target = FindObjectOfType<Target>();
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            target = InstantiateNewTarget();
        }
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
        }

        return newTarget;
    }
}