using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject mainCharater;

    //public float upfloat;
    //public float forward;

    float fixedXRotation = 11;
    void LateUpdate()
    {
        //Vector3 camereUpdis = mainCharater.transform.up * upfloat;
        //Vector3 camereBackdis = mainCharater.transform.forward * forward;
        //transform.position = new Vector3(mainCharater.transform.position.x, mainCharater.transform.position.y, mainCharater.transform.position.z) + camereUpdis + camereBackdis;

        // Ÿ�� ������Ʈ�� �ٶ󺸴� �������� ī�޶��� ȸ�� ���� ����
        Vector3 direction = mainCharater.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);

        // X�� ȸ�� �� ����
        rotation = Quaternion.Euler(fixedXRotation, rotation.eulerAngles.y, rotation.eulerAngles.z);

        // ī�޶��� ȸ�� �� ����
        transform.rotation = rotation;
    }
}
