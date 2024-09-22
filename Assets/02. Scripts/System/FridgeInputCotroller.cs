using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FridgeInputCotroller : MonoBehaviour
{
    Camera _camera;

    Rigidbody rbFridgeDoor;

    private Vector3 touchStartPos;
    private bool touchable = true;

    void Start()
    {
        _camera = Camera.main;
        rbFridgeDoor = GetComponent<Rigidbody>();    
    }

    void OnMouseDown()
    {
        touchStartPos = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        float slideDistanceX = (Input.mousePosition.x - touchStartPos.x) / Screen.width;
        if (Mathf.Abs(slideDistanceX) > 0.1f)
        {
            if (slideDistanceX > 0 && touchable)
            {
                // 오른쪽으로 슬라이드
                Debug.Log("Right Slide");
                OpenFridge();

            }
        }
    }

    void OpenFridge()
    {
        touchable = false;

        //카메라가 앞으로 다가감
        _camera.transform.DOMoveZ(-12, 2f).SetEase(Ease.OutCirc).OnComplete(()=> 
        {
            rbFridgeDoor.freezeRotation = true;
        });

        // Rigidbody에 힘을 가함 (월드 좌표 기준)
        rbFridgeDoor.AddForce(-Vector3.forward.normalized * 5, ForceMode.Impulse);
    }

}
