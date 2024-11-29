using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject mainCharater;

    private void Start()
    {
        Invoke("CamerSetParent", 4.5f);
    }

    void CamerSetParent()
    {
        mainCharater.transform.eulerAngles = new Vector3(0, -180, 0);

        gameObject.transform.SetParent(mainCharater.transform);
    }
}
