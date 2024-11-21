using UnityEngine;
using System;

public enum StatusInput { Right, Left, Up, Down }

public class TutorialInput : TutorialBase
{

    [SerializeField]
    private Animator characteranimator;
    [SerializeField]
    private CharaterMovement charaterMovement;

    [SerializeField]
    private GameObject slidePanel;

    [SerializeField]
    private GameObject[] slideText;

    [SerializeField]
    private StatusInput CurrentStatus = StatusInput.Right;

    private Vector2 touchStartPos;
    private float slideThreshold = 0.1f;
    private bool isDragging = false;

    int speedtmp = 0;

    bool IssameStatus = false;

    // Start is called before the first frame update
    void Start()
    {

        speedtmp = charaterMovement.speed;
    }

    public override void Enter()
    {
        slidePanel.SetActive(true);
        slideText[(int)CurrentStatus].SetActive(true);

        characteranimator.SetBool("Run", false);
        charaterMovement.speed = 0;
    }

    public override void Execute(TutorialController controller)
    {
        HandleTouchInput();
        HandleActions(controller);
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
            print(slideDistance);

            if (Mathf.Abs(slideDistance.x) > slideThreshold)
            {
                if (slideDistance.x > 0)
                    if (CurrentStatus == StatusInput.Right) { IssameStatus = true; }
                else
                    if (CurrentStatus == StatusInput.Left) { IssameStatus = true; }

                EndDrag(); // 슬라이드가 끝났으므로 드래그 종료
            }
            else if (Mathf.Abs(slideDistance.y) > slideThreshold)
            {
                if (slideDistance.y > 0)
                    if (CurrentStatus == StatusInput.Up) { IssameStatus = true; }
                else
                    if (CurrentStatus == StatusInput.Down) { IssameStatus = true; }

                EndDrag();
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

    private void EndDrag()
    {
        isDragging = false;
    }

    private void HandleActions(TutorialController controller)
    {
        
        if (IssameStatus)
        {
            characteranimator.SetBool("Run", true);
            charaterMovement.speed = speedtmp;
            switch (CurrentStatus)
            {
                case StatusInput.Right:
                    StartCoroutine(charaterMovement.moveRight());
                    controller.SetNextTutorial();
                    break;
                case StatusInput.Left:
                    StartCoroutine(charaterMovement.moveLeft());
                    controller.SetNextTutorial();
                    break;
                case StatusInput.Up:

                    StartCoroutine(charaterMovement.Jumping());
                    controller.SetNextTutorial();
                    break;
                case StatusInput.Down:

                    StartCoroutine(charaterMovement.Sliding());
                    controller.SetNextTutorial();
                    break;
            }
        }
    }


    public override void Exit()
    {
        slidePanel.SetActive(false);
        slideText[(int)CurrentStatus].SetActive(false);
        
    }

    public override void Skip(TutorialController controller)
    {
        throw new System.NotImplementedException();
    }
}
