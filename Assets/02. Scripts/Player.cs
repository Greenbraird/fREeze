using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{

    public float speed = 5f;

    private Vector3 touchStartPos;
    public float slideThreshold = 0.1f; // 슬라이드로 감지하는 최소 거리

    [SerializeField]
    private Animator playerAnimator;

    bool touchable = true;

    Rigidbody rb;
    bool Isjump = false;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // 매 프레임마다 호출되는 함수
    void Update()
    {
        playerMove();
        if (Isjump)
        {
            StartCoroutine(Jumping());
        }
    }

    void playerMove()
    {

        // Y 축으로 앞으로 이동
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    #region PlayerMovement

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
                StartCoroutine(moveRight()); 

            }
            else if (slideDistanceX < 0 && touchable == true)
            {
                // 왼쪽으로 슬라이드
                Debug.Log("Left Slide");
                StartCoroutine(moveLeft());
            }
        }
        if (Mathf.Abs(slideDistanceY) > slideThreshold)
        {
            if (slideDistanceY > 0 && touchable == true)
            {
                Isjump = true;
                playerAnimator.SetTrigger("OnJump");
                Debug.Log("Jump");
                
            }
            else if (slideDistanceY < 0 && touchable == true)
            {
                Debug.Log("Sliding");
                StartCoroutine(Sliding());
            }
        }
    }

    IEnumerator moveLeft()
    {
        AudioManager.Instance.LeftRightSFXPlay();
        touchable = false;
        playerAnimator.SetBool("OnDragLeft", true);

        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition - transform.right * 1.5f;
        float elapsedTime = 0f;

        while (elapsedTime < 0.2f)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / 0.2f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition; // Ensure final position is set exactly
        playerAnimator.SetBool("OnDragLeft", false);
        touchable = true;
    }

    IEnumerator moveRight()
    {
        AudioManager.Instance.LeftRightSFXPlay();
        touchable = false;
        playerAnimator.SetBool("OnDragRight", true);
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + transform.right * 1.5f;
        float elapsedTime = 0f;

        while (elapsedTime < 0.2f)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / 0.2f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition; // Ensure final position is set exactly
        playerAnimator.SetBool("OnDragRight", false);
        touchable = true;
    }

    IEnumerator Jumping()
    {
        AudioManager.Instance.JumpSFXPlay();

        touchable = false;
        
        rb.AddForce(Vector3.up * 1, ForceMode.Impulse);
        
        yield return new WaitForSeconds(0.5f);

        Isjump = false;
        touchable = true;
        
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

    #endregion

    #region Collision Script
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
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
    #endregion

    #region Trigger Script
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "RotationLine")
        {
            Debug.Log(other.gameObject.tag);
            // 현재 플레이어의 회전 값을 가져옴
            Vector3 currentRotation = transform.eulerAngles;
            // 타겟 오브젝트의 y 회전 값만 가져옴
            float targetYRotation = other.gameObject.transform.localEulerAngles.y;
            // 현재 플레이어의 회전 값을 유지하면서 y 회전 값만 타겟 오브젝트의 y 회전 값으로 설정
            transform.eulerAngles = new Vector3(currentRotation.x, targetYRotation, currentRotation.z);
        }
    }
    #endregion


}
