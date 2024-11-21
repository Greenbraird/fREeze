using System;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    public event Action OnRightSlide;
    public event Action OnLeftSlide;
    public event Action OnUpSlide;
    public event Action OnDownSlide;


    private Vector2 touchStartPos;
    private float slideThreshold = 0.1f; // �����̵� ���� �ΰ��� (ȭ�� ���� ����)
    private bool isDragging = false;

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        // ��ġ ����
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            isDragging = true;
        }

        // ��ġ �� (�巡��)
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

        // ��ġ ����
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
