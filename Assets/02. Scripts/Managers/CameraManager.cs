using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject mainCharater;

    float fixedXRotation = 11;
    void LateUpdate()
    {
        Vector3 camereUpdis = mainCharater.transform.up * 3;
        Vector3 camereBackdis = mainCharater.transform.forward * -4f;
        transform.position = new Vector3(mainCharater.transform.position.x, mainCharater.transform.position.y, mainCharater.transform.position.z) + camereUpdis + camereBackdis;

        // 타겟 오브젝트를 바라보는 방향으로 카메라의 회전 값을 설정
        Vector3 direction = mainCharater.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);

        // X축 회전 값 고정
        rotation = Quaternion.Euler(fixedXRotation, rotation.eulerAngles.y, rotation.eulerAngles.z);

        // 카메라의 회전 값 적용
        transform.rotation = rotation;
    }
}
