using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{

    public float speed = 5f;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    private Vector3 touchStartPos;
    public float slideThreshold = 0.1f; // 슬라이드로 감지하는 최소 거리

    [SerializeField]
    private Animator playerAnimator;

    bool touchable = true;

    // 매 프레임마다 호출되는 함수
    void Update()
    {
        playerMove();
    }

    void playerMove()
    {
        
        // 현재 위치를 저장
        Vector3 currentPosition = transform.position;

        // Y 축으로 앞으로 이동
        currentPosition.z += speed * Time.deltaTime;

        // 새로운 위치를 설정
        transform.position = currentPosition;

        /**
        if (currentWaypointIndex < waypoints.Length)
        {
            // 다음 지점 방향으로 이동
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

            transform.DOLookAt(waypoints[currentWaypointIndex].position, 0.5f).SetEase(Ease.InOutExpo);

            // 다음 지점에 도착하면 다음 지점으로 이동
            if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        **/
    }



    void OnMouseDown()
    {
        touchStartPos = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        float slideDistanceX = (Input.mousePosition.x - touchStartPos.x) / Screen.width;
        float slideDistanceY = (Input.mousePosition.y - touchStartPos.y) / Screen.height;

        // 슬라이드 거리가 threshold를 넘으면 슬라이드로 간주
        if (Mathf.Abs(slideDistanceX) > slideThreshold)
        {
            if (slideDistanceX > 0 && touchable == true)
            {
                // 오른쪽으로 슬라이드
                Debug.Log("Right Slide");
                moveRight();

            }
            else if (slideDistanceX < 0 && touchable == true)
            {
                // 왼쪽으로 슬라이드
                Debug.Log("Left Slide");
                moveLeft();
            }
        }
        if (Mathf.Abs(slideDistanceY) > slideThreshold)
        {
            if (slideDistanceY > 0 && touchable == true)
            {
                Debug.Log("Jump");
                Jumping();
            }
            else if (slideDistanceY < 0 && touchable == true)
            {
                Debug.Log("Sliding");
                StartCoroutine(Sliding());
            }
        }
    }

    void moveLeft()
    {
        AudioManager.Instance.LeftRightSFXPlay();
        touchable = false;
        playerAnimator.SetBool("OnDragLeft", true);
        transform.DOMoveX(transform.position.x - 3f, 0.5f).OnComplete(()=> {
            playerAnimator.SetBool("OnDragLeft", false);
            touchable = true;
        });
    }

    void moveRight()
    {
        AudioManager.Instance.LeftRightSFXPlay();
        touchable = false;
        playerAnimator.SetBool("OnDragRight", true);
        transform.DOMoveX(transform.position.x+ 3f, 0.5f).OnComplete(() => {
            playerAnimator.SetBool("OnDragRight", false);
            touchable = true;
        });
    }

    void Jumping()
    {
        AudioManager.Instance.JumpSFXPlay();

        touchable = false;
        playerAnimator.SetTrigger("OnJump");
        transform.DOMoveY(transform.position.y + 3f, 1f).OnComplete(() =>
        {
            touchable = true;
        });
    }

    IEnumerator Sliding()
    {
        touchable = false;

        AudioManager.Instance.SlidingSFXPlay();

        CapsuleCollider playCollier = gameObject.GetComponent<CapsuleCollider>();

        playCollier.center = new Vector3(0, 0.3f, 0);
        playCollier.height = 1f;

        playerAnimator.SetTrigger("OnSliding");

        yield return new WaitForSeconds(1);

        playCollier.center = new Vector3(0, 0.8f, 0);
        playCollier.height = 1.5f;

        touchable = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 물체의 태그 가져오기
        string collidedObjectTag = collision.gameObject.tag;

        //// 태그에 따라 다른 동작 수행
        if (collidedObjectTag == "cube")
        {
            Debug.Log("장애물이랑 접촉했습니다!");
            //충돌 시  필요한 event
        }
        else if(collidedObjectTag == "Star")
        {
            collision.gameObject.SetActive(false);
            Debug.Log("물체와 충돌했습니다");
            UIManager.Instance.GetCoinSliderEvent();
            AudioManager.Instance.GetCoinSFXPlay();
        }
    }

}
