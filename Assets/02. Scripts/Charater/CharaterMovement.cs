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
        // 기타 동작 처리
        if (GameSystem.Instance.IsGamestart)
        {
            if (isRunning && touchable)
            {
                MoveForwardWithSlope(); // 경사 기반 이동
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
        Vector3 origin = transform.position + Vector3.up * 0.5f; // 캐릭터 위치 약간 위에서 Raycast 시작
        Debug.DrawRay(origin, Vector3.down * 1f, Color.red);
        // 지면을 감지하기 위해 아래 방향으로 Raycast
        if (Physics.Raycast(origin, Vector3.down, out hit, 1f))
        {

            // 표면의 기울기를 기준으로 이동 방향 조정
            Vector3 slopeDirection = Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized;
            rb.velocity = slopeDirection * speed;
        }
        else
        {
            // Raycast 실패 시 기본적으로 평지 이동
            rb.velocity = transform.forward * speed;
            // 공중에 떠 있는 경우
            rb.velocity += Physics.gravity * Time.deltaTime;
        }
    }

    public IEnumerator MoveLeft()
    {
        touchable = false;
        animator.SetBool("OnDragLeft", true);

        // 왼쪽으로 이동
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

        // 오른쪽으로 이동
        gameObject.transform.DOMove(transform.position + transform.right * 1.8f, 0.3f).OnComplete(() =>
        {
            animator.SetBool("OnDragRight", false);
            touchable = true;
        });

        yield return null;
    }

    public IEnumerator Jumping()
    {
        if (!isGrounded) yield break; // 점프 중복 방지

        touchable = false;
        isGrounded = false;

        // 점프 애니메이션 트리거
        animator.SetTrigger("OnJump");
        AudioManager.Instance.SFXPlay(gameObject, 2);

        // 위로 점프
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // 착지 대기
        yield return new WaitUntil(() => IsGrounded());

        // 착지 후 touchable 활성화
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
        // 캐릭터 아래로 Raycast 발사하여 지면 감지
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.down * 0.1f; // Raycast를 캐릭터 아래서 시작

        if (Physics.Raycast(origin, Vector3.down, out hit, 0.2f))
        {
            return true;
        }
        return false;
    }
}
