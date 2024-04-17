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
    public float slideThreshold = 0.1f; // �����̵�� �����ϴ� �ּ� �Ÿ�

    [SerializeField]
    private Animator playerAnimator;

    bool touchable = true;

    // �� �����Ӹ��� ȣ��Ǵ� �Լ�
    void Update()
    {
        playerMove();
    }

    void playerMove()
    {
        
        // ���� ��ġ�� ����
        Vector3 currentPosition = transform.position;

        // Y ������ ������ �̵�
        currentPosition.z += speed * Time.deltaTime;

        // ���ο� ��ġ�� ����
        transform.position = currentPosition;

        /**
        if (currentWaypointIndex < waypoints.Length)
        {
            // ���� ���� �������� �̵�
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

            transform.DOLookAt(waypoints[currentWaypointIndex].position, 0.5f).SetEase(Ease.InOutExpo);

            // ���� ������ �����ϸ� ���� �������� �̵�
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

        // �����̵� �Ÿ��� threshold�� ������ �����̵�� ����
        if (Mathf.Abs(slideDistanceX) > slideThreshold)
        {
            if (slideDistanceX > 0 && touchable == true)
            {
                // ���������� �����̵�
                Debug.Log("Right Slide");
                moveRight();

            }
            else if (slideDistanceX < 0 && touchable == true)
            {
                // �������� �����̵�
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
        // �浹�� ��ü�� �±� ��������
        string collidedObjectTag = collision.gameObject.tag;

        //// �±׿� ���� �ٸ� ���� ����
        if (collidedObjectTag == "cube")
        {
            Debug.Log("��ֹ��̶� �����߽��ϴ�!");
            //�浹 ��  �ʿ��� event
        }
        else if(collidedObjectTag == "Star")
        {
            collision.gameObject.SetActive(false);
            Debug.Log("��ü�� �浹�߽��ϴ�");
            UIManager.Instance.GetCoinSliderEvent();
            AudioManager.Instance.GetCoinSFXPlay();
        }
    }

}
