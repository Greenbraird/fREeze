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
    private bool isDragging = false;  // 슬라이드 후 바로 드래그를 종료하기 위한 플래그

    void OnMouseDown()
    {
        touchStartPos = Input.mousePosition;
        isDragging = true;  // 드래그 시작
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;  // 드래그가 종료된 상태면 처리하지 않음

        float slideDistanceX = (Input.mousePosition.x - touchStartPos.x) / Screen.width;
        float slideDistanceY = (Input.mousePosition.y - touchStartPos.y) / Screen.height;

        // 슬라이드 거리가 threshold를 넘으면 슬라이드로 간주
        if (Mathf.Abs(slideDistanceX) > slideThreshold)
        {
            if (slideDistanceX > 0)
            {
                // 오른쪽으로 슬라이드
                Debug.Log("Right Slide");
                OnRightSlide?.Invoke();

                // 슬라이드가 끝났으므로 드래그를 멈춘다 (마우스 업 상태를 강제)
                EndDrag();
            }
            else if (slideDistanceX < 0)
            {
                // 왼쪽으로 슬라이드
                Debug.Log("Left Slide");
                OnLeftSlide?.Invoke();

                // 슬라이드가 끝났으므로 드래그를 멈춘다 (마우스 업 상태를 강제)
                EndDrag();
            }
        }

        if (Mathf.Abs(slideDistanceY) > slideThreshold)
        {
            if (slideDistanceY > 0)
            {
                Debug.Log("Jump");
                OnUpSlide?.Invoke();

                // 위쪽 슬라이드가 끝났으므로 드래그를 멈춘다 (마우스 업 상태를 강제)
                EndDrag();
            }
            else if (slideDistanceY < 0)
            {
                Debug.Log("Sliding");
                OnDownSlide?.Invoke();

                // 아래쪽 슬라이드가 끝났으므로 드래그를 멈춘다 (마우스 업 상태를 강제)
                EndDrag();
            }
        }
    }

    // 드래그 종료 (OnMouseUp과 동일한 처리)
    private void EndDrag()
    {
        isDragging = false;  // 드래그 상태를 종료
    }
}
