using System;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    public event Action OnRightSlide;
    public event Action OnLeftSlide;
    public event Action OnUpSlide;
    public event Action OnDownSlide;


    private Vector2 touchStartPos;
    private float slideThreshold = 0.1f; // 슬라이드 감지 민감도 (화면 비율 기준)
    private bool isDragging = false;

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        // 터치 시작
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            isDragging = true;
        }

        // 터치 중 (드래그)
        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 currentPos = Input.mousePosition;
            Vector2 slideDistance = CalculateSlideDistance(currentPos);

            if (Mathf.Abs(slideDistance.x) > slideThreshold)
            {
                if (slideDistance.x > 0)
                    TriggerSlide("Right", OnRightSlide);
                else
                    TriggerSlide("Left",  OnLeftSlide);
            }
            else if (Mathf.Abs(slideDistance.y) > slideThreshold)
            {
                if (slideDistance.y > 0)
                    TriggerSlide("Up", OnUpSlide);
                else
                    TriggerSlide("Down",OnDownSlide);
            }
        }

        // 터치 종료
        if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }

    private Vector2 CalculateSlideDistance(Vector2 currentPos)
    {
        float normalizedX = (currentPos.x - touchStartPos.x) / Screen.width;
        float normalizedY = (currentPos.y - touchStartPos.y) / Screen.height;
        return new Vector2(normalizedX, normalizedY);
    }

    private void TriggerSlide(string direction, Action slideEvent)
    {
        Debug.Log($"{direction} Slide");
        slideEvent?.Invoke();
        EndDrag();
    }

    private void EndDrag()
    {
        isDragging = false;
    }

}
