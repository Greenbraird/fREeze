using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject mainCharater;
    void Update()
    {
        Vector3 camereUpdis = mainCharater.transform.up*3;
        Vector3 camereBackdis = mainCharater.transform.forward * -4f;
        transform.position = new Vector3(mainCharater.transform.position.x, mainCharater.transform.position.y, mainCharater.transform.position.z) + camereUpdis + camereBackdis;

    }
}
