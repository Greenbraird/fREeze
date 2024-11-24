using System.Collections;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.UI.Image;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class CharaterMovement : MonoBehaviour
{
    
    [SerializeField]
    private float jumpForce;

    public int speed;

    private Animator animator;
    private Rigidbody rb;

    private bool touchable;
    private bool isRunning = false;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        touchable = GameSystem.Instance.touchable;
    }

    private void FixedUpdate()
    {
        // ��Ÿ ���� ó��
        if (GameSystem.Instance.IsGamestart)
        {
            if (isRunning && touchable)
            {
                MoveForwardWithSlope(); // ��� ��� �̵�
            }
            else
            {
                isRunning = true;
                animator.SetBool("Run", true);
            }
        }
    }

    private void MoveForwardWithSlope()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 0.5f; // ĳ���� ��ġ �ణ ������ Raycast ����
        Debug.DrawRay(origin, Vector3.down * 1f, Color.red);
        // ������ �����ϱ� ���� �Ʒ� �������� Raycast
        if (Physics.Raycast(origin, Vector3.down, out hit, 1f))
        {

            // ǥ���� ���⸦ �������� �̵� ���� ����
            Vector3 slopeDirection = Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized;
            rb.velocity = slopeDirection * speed;
        }
        else
        {
            // Raycast ���� �� �⺻������ ���� �̵�
            rb.velocity = transform.forward * speed;
            // ���߿� �� �ִ� ���
            rb.velocity += Physics.gravity * Time.deltaTime;
        }
    }

    public IEnumerator MoveLeft()
    {
        touchable = false;
        animator.SetBool("OnDragLeft", true);

        // �������� �̵�
        gameObject.transform.DOMove(transform.position - transform.right * 1.8f, 0.3f).OnComplete(() =>
        {
            animator.SetBool("OnDragLeft", false);
            touchable = true;
        });

        yield return null;
    }

    public IEnumerator MoveRight()
    {
        touchable = false;
        animator.SetBool("OnDragRight", true);

        // ���������� �̵�
        gameObject.transform.DOMove(transform.position + transform.right * 1.8f, 0.3f).OnComplete(() =>
        {
            animator.SetBool("OnDragRight", false);
            touchable = true;
        });

        yield return null;
    }

    public IEnumerator Jumping()
    {
        if (!isGrounded) yield break; // ���� �ߺ� ����

        touchable = false;
        isGrounded = false;

        // ���� �ִϸ��̼� Ʈ����
        animator.SetTrigger("OnJump");
        AudioManager.Instance.SFXPlay(gameObject, 2);

        // ���� ����
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // ���� ���
        yield return new WaitUntil(() => IsGrounded());

        // ���� �� touchable Ȱ��ȭ
        touchable = true;
        isGrounded = true;
    }

    public IEnumerator Sliding()
    {
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        capsuleCollider.enabled = false;
        boxCollider.enabled = true;

        animator.SetTrigger("OnSliding");

        yield return new WaitForSeconds(1);

        capsuleCollider.enabled = true;
        boxCollider.enabled = false;

        touchable = true;
    }

    private bool IsGrounded()
    {
        // ĳ���� �Ʒ��� Raycast �߻��Ͽ� ���� ����
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.down * 0.1f; // Raycast�� ĳ���� �Ʒ��� ����

        if (Physics.Raycast(origin, Vector3.down, out hit, 0.2f))
        {
            return true;
        }
        return false;
    }
}
