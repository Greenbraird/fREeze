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
                // ���������� �����̵�
                Debug.Log("Right Slide");
                OpenFridge();

            }
        }
    }

    void OpenFridge()
    {
        touchable = false;

        //ī�޶� ������ �ٰ���
        _camera.transform.DOMoveZ(-12, 2f).SetEase(Ease.OutCirc).OnComplete(()=> 
        {
            rbFridgeDoor.freezeRotation = true;
        });

        // Rigidbody�� ���� ���� (���� ��ǥ ����)
        rbFridgeDoor.AddForce(-Vector3.forward.normalized * 5, ForceMode.Impulse);
    }

}
