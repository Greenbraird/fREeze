using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{

    public float speed = 5f;

    private Vector3 touchStartPos;
    public float slideThreshold = 0.1f; // �����̵�� �����ϴ� �ּ� �Ÿ�

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

    // �� �����Ӹ��� ȣ��Ǵ� �Լ�
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

        // Y ������ ������ �̵�
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

        // �����̵� �Ÿ��� threshold�� ������ �����̵�� ����
        if (Mathf.Abs(slideDistanceX) > slideThreshold)
        {
            if (slideDistanceX > 0 && touchable == true)
            {
                // ���������� �����̵�
                Debug.Log("Right Slide");
                StartCoroutine(moveRight()); 

            }
            else if (slideDistanceX < 0 && touchable == true)
            {
                // �������� �����̵�
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
    #endregion

    #region Trigger Script
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "RotationLine")
        {
            Debug.Log(other.gameObject.tag);
            // ���� �÷��̾��� ȸ�� ���� ������
            Vector3 currentRotation = transform.eulerAngles;
            // Ÿ�� ������Ʈ�� y ȸ�� ���� ������
            float targetYRotation = other.gameObject.transform.localEulerAngles.y;
            // ���� �÷��̾��� ȸ�� ���� �����ϸ鼭 y ȸ�� ���� Ÿ�� ������Ʈ�� y ȸ�� ������ ����
            transform.eulerAngles = new Vector3(currentRotation.x, targetYRotation, currentRotation.z);
        }
    }
    #endregion


}
