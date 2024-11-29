using System.Collections;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.UI.Image;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class CharaterMovement : MonoBehaviour
{
    
    [SerializeField]
    private float jumpForce;

    public int speed;

    private Animator animator;
    private Rigidbody rb;

    private bool isRunning = false;
    private bool isGrounded = true;

    protected float m_JumpStart;
    protected bool m_Jumping;

    protected bool m_Sliding;
    protected float m_SlideStart;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (GameSystem.Instance.IsGamestart)
        {
            if (isRunning && GameSystem.Instance.touchable)
            {
                Vector3 movement = Vector3.forward * (speed * Time.deltaTime);
                transform.Translate(movement);
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
        Vector3 origin = transform.position + Vector3.up * 0.5f; 
        Debug.DrawRay(origin, Vector3.down * 1f, Color.red);
        if (Physics.Raycast(origin, Vector3.down, out hit, 1f))
        {
            Vector3 slopeDirection = Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized;
            rb.velocity = slopeDirection * speed;
        }
        else
        {
            rb.velocity = transform.forward * speed;
            rb.velocity += Physics.gravity * Time.deltaTime;
        }
    }

    public IEnumerator MoveLeft()
    {
        GameSystem.Instance.touchable = false;
        animator.SetBool("OnDragLeft", true);

        gameObject.transform.DOMove(transform.position - transform.right * 1.8f, 0.3f).OnComplete(() =>
        {
            animator.SetBool("OnDragLeft", false);
            GameSystem.Instance.touchable = true;
        });

        yield return null;
    }

    public IEnumerator MoveRight()
    {
        GameSystem.Instance.touchable = false;
        animator.SetBool("OnDragRight", true);

        gameObject.transform.DOMove(transform.position + transform.right * 1.8f, 0.3f).OnComplete(() =>
        {
            animator.SetBool("OnDragRight", false);
            GameSystem.Instance.touchable = true;
        });

        yield return null;
    }

    void Jump()
    {
        if (!isRunning || m_Jumping) return;

        m_Jumping = true;
        m_JumpStart = transform.position.y; // 점프 시작 위치 저장
        animator.SetTrigger("OnJump");

        AudioManager.Instance.SFXPlay(gameObject, 2);
    }

    public IEnumerator Jumping()
    {
        if (!isGrounded) yield break; // ���� �ߺ� ����

        GameSystem.Instance.touchable = false;
        isGrounded = false;

        // ���� �ִϸ��̼� Ʈ����
        animator.SetTrigger("OnJump");
        AudioManager.Instance.SFXPlay(gameObject, 2);

        // ���� ����
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // ���� ���
        yield return new WaitUntil(() => IsGrounded());

        // ���� �� touchable Ȱ��ȭ
        GameSystem.Instance.touchable = true;
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

        GameSystem.Instance.touchable = true;
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