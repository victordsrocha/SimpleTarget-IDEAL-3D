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

    public List<char> spatialSense;

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

        spatialSense = new List<char>() {'e', 'e', 'e', 'e', 'e', 'e', 'e', 'e'};
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

    private void OnCollisionExit(Collision other)
    {
        ResetSpatialSense();
    }

    private void OnCollisionStay(Collision other)
    {
        ResetSpatialSense();
        UpdateSpatialSense(other);
    }

    private void OnCollisionEnter(Collision other)
    {
        ResetSpatialSense();
        UpdateSpatialSense(other);
    }

    private void UpdateSpatialSense(Collision other)
    {
        var objChar = '?';
        if (other.gameObject.CompareTag("Obstacle"))
        {
            objChar = 'b';
        }
        else if (other.gameObject.CompareTag("Target"))
        {
            objChar = 't';
        }

        foreach (var contact in other.contacts)
        {
            var angle = CalcAngle(contact.point); // -180 +180

            if (angle >= -180f && angle <= -135f)
            {
                spatialSense[0] = objChar;
            }
            else if (angle >= -135f && angle <= -90f)
            {
                spatialSense[1] = objChar;
            }
            else if (angle >= -90f && angle <= -45f)
            {
                spatialSense[2] = objChar;
            }
            else if (angle >= -45f && angle <= 0f)
            {
                spatialSense[3] = objChar;
            }
            else if (angle >= 0f && angle <= 45f)
            {
                spatialSense[4] = objChar;
            }
            else if (angle >= 45f && angle <= 90f)
            {
                spatialSense[5] = objChar;
            }
            else if (angle >= 90f && angle <= 135f)
            {
                spatialSense[6] = objChar;
            }
            else if (angle >= 135f && angle <= 180f)
            {
                spatialSense[7] = objChar;
            }
        }
    }

    private void ResetSpatialSense()
    {
        for (int i = 0; i < spatialSense.Count; i++)
        {
            spatialSense[i] = 'e';
        }
    }

    float CalcAngle(Vector3 other)
    {
        var myDir = transform.forward;
        var otherDir = other - transform.position;

        myDir.y = 0;
        otherDir.y = 0;

        return Vector3.SignedAngle(myDir, otherDir, Vector3.up);
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
}