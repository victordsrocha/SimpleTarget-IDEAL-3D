using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject playerCamera;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            mainCamera.SetActive(true);
            playerCamera.SetActive(false);
        }
        else if (Input.GetKeyDown("2"))
        {
            mainCamera.SetActive(false);
            playerCamera.SetActive(true);
        }
    }
}
