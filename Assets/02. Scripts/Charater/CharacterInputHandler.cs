using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    public event Action OnRightSlide;
    public event Action OnLeftSlide;
    public event Action OnUpSlide;
    public event Action OnDownSlide;

    private Vector3 touchStartPos;
    private float slideThreshold = 0.1f;
    private bool isDragging = false;  // �����̵� �� �ٷ� �巡�׸� �����ϱ� ���� �÷���

    void OnMouseDown()
    {
        touchStartPos = Input.mousePosition;
        isDragging = true;  // �巡�� ����
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;  // �巡�װ� ����� ���¸� ó������ ����

        float slideDistanceX = (Input.mousePosition.x - touchStartPos.x) / Screen.width;
        float slideDistanceY = (Input.mousePosition.y - touchStartPos.y) / Screen.height;

        // �����̵� �Ÿ��� threshold�� ������ �����̵�� ����
        if (Mathf.Abs(slideDistanceX) > slideThreshold)
        {
            if (slideDistanceX > 0)
            {
                // ���������� �����̵�
                Debug.Log("Right Slide");
                OnRightSlide?.Invoke();

                // �����̵尡 �������Ƿ� �巡�׸� ����� (���콺 �� ���¸� ����)
                EndDrag();
            }
            else if (slideDistanceX < 0)
            {
                // �������� �����̵�
                Debug.Log("Left Slide");
                OnLeftSlide?.Invoke();

                // �����̵尡 �������Ƿ� �巡�׸� ����� (���콺 �� ���¸� ����)
                EndDrag();
            }
        }

        if (Mathf.Abs(slideDistanceY) > slideThreshold)
        {
            if (slideDistanceY > 0)
            {
                Debug.Log("Jump");
                OnUpSlide?.Invoke();

                // ���� �����̵尡 �������Ƿ� �巡�׸� ����� (���콺 �� ���¸� ����)
                EndDrag();
            }
            else if (slideDistanceY < 0)
            {
                Debug.Log("Sliding");
                OnDownSlide?.Invoke();

                // �Ʒ��� �����̵尡 �������Ƿ� �巡�׸� ����� (���콺 �� ���¸� ����)
                EndDrag();
            }
        }
    }

    // �巡�� ���� (OnMouseUp�� ������ ó��)
    private void EndDrag()
    {
        isDragging = false;  // �巡�� ���¸� ����
    }
}
