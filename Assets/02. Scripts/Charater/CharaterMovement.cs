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
            { // 애니메이션 시작
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

        // 점프 애니메이션 트리거 (필요 시 추가)
        animator.SetTrigger("OnJump");
        AudioManager.Instance.SFXPlay(gameObject, 2);

        // 수직으로 점프하는 힘을 더하면서 앞으로 계속 이동
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);

        // 점프 중에도 앞으로 계속 이동하도록 일정 시간 동안 moveforward를 계속 호출
        float jumpDuration = 0.5f; // 점프 지속 시간
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            moveforward(); // 점프 중에도 앞으로 움직임 유지
            elapsedTime += Time.deltaTime;
            yield return null; // 한 프레임 대기
        }

        // 착지 후 다시 touchable을 활성화
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

