using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class CharaterMovement : MonoBehaviour
{
    [SerializeField]
    private int speed;
    [SerializeField]
    private int jumpSpeed;

    Animator animator;
    Rigidbody rb;

    bool touchable;
    bool isrun = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        touchable = GameSystem.Instance.touchable;
    }

    private void FixedUpdate()
    {
        if (GameSystem.Instance.IsGamestart)
        {
            if (isrun && touchable) { moveforward(); }
            else
            { // �ִϸ��̼� ����
                isrun = true;
                animator.SetBool("Run", true);
            }
        }
    }

    public void moveforward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public IEnumerator moveLeft()
    {

        touchable = false;
        animator.SetBool("OnDragLeft", true);

        gameObject.transform.DOMove(transform.position - transform.right * 1.8f, 0.3f).OnComplete(() =>
        {
            animator.SetBool("OnDragLeft", false);
            touchable = true;
        });

        yield return null;  


        
    }

    public IEnumerator moveRight()
    {
        touchable = false;
        animator.SetBool("OnDragRight", true);

        gameObject.transform.DOMove(transform.position + transform.right * 1.8f, 0.3f).OnComplete(() => 
        {
            animator.SetBool("OnDragRight", false);
            touchable = true;
        });

        yield return null;
    }

    public IEnumerator Jumping()
    {
        touchable = false;

        // ���� �ִϸ��̼� Ʈ���� (�ʿ� �� �߰�)
        animator.SetTrigger("OnJump");
        AudioManager.Instance.SFXPlay(gameObject, 2);

        // �������� �����ϴ� ���� ���ϸ鼭 ������ ��� �̵�
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);

        // ���� �߿��� ������ ��� �̵��ϵ��� ���� �ð� ���� moveforward�� ��� ȣ��
        float jumpDuration = 0.5f; // ���� ���� �ð�
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            moveforward(); // ���� �߿��� ������ ������ ����
            elapsedTime += Time.deltaTime;
            yield return null; // �� ������ ���
        }

        // ���� �� �ٽ� touchable�� Ȱ��ȭ
        touchable = true;
    }


    public IEnumerator Sliding()
    {
        CapsuleCollider playCollier = gameObject.GetComponent<CapsuleCollider>();

        playCollier.center = new Vector3(0, 0.3f, 0);
        playCollier.height = 1f;

        animator.SetTrigger("OnSliding");

        yield return new WaitForSeconds(1);

        playCollier.center = new Vector3(0, 0.8f, 0);
        playCollier.height = 1.5f;

        touchable = true;
    }

}

