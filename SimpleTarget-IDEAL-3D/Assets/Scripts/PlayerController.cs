using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    [SerializeField] private float force = 1f;
    [SerializeField] private float timeBetweenActions;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private List<char> spatialSense;
    [SerializeField] private int spatialSensePositions;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        RotationChange();
    }

    private void FixedUpdate()
    {
        MoveForwardChange();
    }


    private void RotationChange()
    {
        var rotationVector = transform.rotation.eulerAngles.y;

        if (Input.GetKey("d"))
        {
            transform.DORotate(new Vector3(0.0f, rotationVector + rotationSpeed, 0.0f), timeBetweenActions);
        }
        else if (Input.GetKey("a"))
        {
            transform.DORotate(new Vector3(0.0f, rotationVector - rotationSpeed, 0.0f), timeBetweenActions);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            other.GetComponent<Target>().DestroyTarget();
        }
    }

    private void MoveForwardChange()
    {
        if (Input.GetKey("w"))
        {
            MoveForward();
        }
    }

    public void MoveForward()
    {
        StartCoroutine(MoveForwardCoroutine());
    }

    private IEnumerator MoveForwardCoroutine()
    {
        playerRigidbody.velocity = transform.forward * force;
        yield return new WaitForSeconds(timeBetweenActions);
        // ReSharper disable once Unity.InefficientPropertyAccess
        playerRigidbody.velocity = Vector3.zero;
    }
}