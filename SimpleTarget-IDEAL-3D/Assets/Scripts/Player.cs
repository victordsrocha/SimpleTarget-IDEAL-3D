using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public bool humanInput;
    public GameObject plane;

    private Rigidbody _playerRigidbody;
    private Observation _playerObservation;
    public bool collision;
    [SerializeField] private float force;
    [SerializeField] private float timeBetweenActions;
    [SerializeField] private float rotationSpeed;

    private float time;
    public bool isFoodReached;

    private EnvInterface _envInterface;

    // Start is called before the first frame update
    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerObservation = GetComponent<Observation>();
        _envInterface = GetComponent<EnvInterface>();
        time = Time.time;
        collision = false;
    }

    private void Update()
    {
        if (humanInput && Time.time > time + 0.1f)
        {
            if (Input.GetKey("d"))
            {
                time = Time.time;
                RotateRight();
            }
            else if (Input.GetKey("a"))
            {
                time = Time.time;
                RotateLeft();
            }
        }
    }

    private void FixedUpdate()
    {
        if (humanInput && Time.time > time + 0.1f)
        {
            if (Input.GetKey("w"))
            {
                time = Time.time;
                MoveForward();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            isFoodReached = true;
            other.GetComponent<Target>().DestroyTarget();
        }
    }

    public void RotateLeft()
    {
        StartCoroutine(RotationChangeCoroutine(-1));
    }

    public void RotateRight()
    {
        StartCoroutine(RotationChangeCoroutine(1));
    }

    private IEnumerator RotationChangeCoroutine(int rotationDirection)
    {
        var rotationVector = transform.rotation.eulerAngles.y;
        transform.DORotate(new Vector3(0.0f, rotationVector + rotationDirection * rotationSpeed, 0.0f),
            timeBetweenActions);
        yield return new WaitForSeconds(timeBetweenActions);
        _playerObservation.UpdateObservation();
        _envInterface.getObservation = true;
    }

    public void MoveForward()
    {
        StartCoroutine(MoveForwardCoroutine());
    }

    private IEnumerator MoveForwardCoroutine()
    {
        _playerRigidbody.velocity = transform.forward * force;
        yield return new WaitForSeconds(timeBetweenActions);
        // ReSharper disable once Unity.InefficientPropertyAccess
        _playerRigidbody.velocity = Vector3.zero;
        _playerObservation.UpdateObservation(true);
        _envInterface.getObservation = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject != plane)
        {
            collision = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        collision = false;
    }
}