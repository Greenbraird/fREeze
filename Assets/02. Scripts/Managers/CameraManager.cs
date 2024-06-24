using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject mainCharater;
    void Update()
    {
        transform.position = new Vector3(mainCharater.transform.position.x, mainCharater.transform.position.y + 2.8f, mainCharater.transform.position.z - 4);
    }
}
