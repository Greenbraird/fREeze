using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject mainCharater;
    void Update()
    {
        Vector3 camereUpdis = mainCharater.transform.up;
        Vector3 camereBackdis = mainCharater.transform.forward * -0.4f;
        transform.position = camereBackdis;
    }
}
