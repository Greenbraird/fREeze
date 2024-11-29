using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(Animator))]
public class CharacterInputHandler : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;

    public int speed = 5;
    public float jumpHeight = 1;

    public float jumpDuration = 1.0f;
    public float slideDuration = 1.0f;
    public float laneDuration = 0.01f;

    bool m_IsSwiping = false;
    Vector2 m_StartingTouch;

    bool m_IsRunning = false;

    protected float m_jumpStartTime;
    protected bool m_Jumping;
    protected float m_JumpStart;

    private float slideStartTime;
    protected bool m_Sliding;
    protected float m_SlideStart;

    protected float m_chageLaneTime;
    protected bool m_chageLane;
    protected float m_chageLaneStart;
    protected int m_LaneDirection;
    protected int m_LaneCount;

    private Vector3 rootMotionVelocity; // Root Motion에서 가져온 이동 속도
    public float gravity = -5f;      // 중력 값
    private float verticalVelocity;    // 중력에 의한 Y축 속도



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        m_IsRunning = true;
        m_Jumping = false;
        m_Sliding = false;

        // Rigidbody 설정: 중력 활성화, 물리 상호작용 방지
        rb.useGravity = false; // Root Motion과 중력 충돌 방지
        rb.isKinematic = false;

        // 캐릭터의 이동
        Vector3 forwardDirection = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        transform.Translate(forwardDirection * (speed * Time.deltaTime), Space.World);

        // 캐릭터의 방향을 정확히 전방으로 보정
        transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

        m_LaneCount =1;
    }

    void Update()
    {

        // 중력 업데이트
        if (IsGrounded())
        {
            verticalVelocity = 0f; // 바닥에 있을 때 Y축 속도 초기화
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // 중력 가속도
        }

        if (!GameSystem.Instance.IsGamestart) return;
        else animator.SetBool("Run", true);
        if (!GameSystem.Instance.touchable) return;
        if (!m_IsRunning) return;

        transform.Translate(transform.forward * (speed * Time.deltaTime), Space.World);

        // 입력 처리
        if (Input.GetKeyDown(KeyCode.UpArrow)) Jump();
        if (Input.GetKeyDown(KeyCode.DownArrow)) Slide();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) ChangeLane(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) ChangeLane(1);

        // 점프 상태 업데이트
        if (m_Jumping)
        {
            float jumpTime = Time.time - m_jumpStartTime; // 점프 경과 시간
            float jumpRatio = jumpTime / jumpDuration;
            if (jumpRatio >= 1.0f)
            {
                m_Jumping = false;
            }
            else
            {
                float yOffset = Mathf.Sin(jumpRatio * Mathf.PI) * jumpHeight; // Y축으로 점프
                transform.position = new Vector3(transform.position.x, m_JumpStart + yOffset, transform.position.z); // Y축만 수정
            }
        }

        if (m_chageLane)
        {
            float chageLaneTime = Time.time - m_chageLaneTime;
            float chageLane = chageLaneTime / laneDuration;

            Debug.Log("변화 시간 " + chageLaneTime);
            Debug.Log("변화 비율 " + chageLane);
            
            if (chageLaneTime > 0.2f)
            {
                m_chageLane = false;
                animator.SetBool("OnDragRight", false); // 오른쪽으로 변경
                animator.SetBool("OnDragLeft", false); // 오른쪽으로 변경
            }
            else
            {
                // X축 이동 계산
                Vector3 xOffset = m_LaneDirection * 2.0f * transform.right; // 이동 거리 계산

                transform.position = new Vector3(m_chageLaneStart, transform.position.y, transform.position.z) + xOffset;
            }
        }

        if (m_Sliding)
        {
            // 슬라이드 중인지 확인
            if (m_Sliding)
            {
                // 슬라이드 지속 시간이 지나면 슬라이드 종료
                if (Time.time - slideStartTime > slideDuration)
                {
                    m_Sliding = false;
                    //characterCollider.Slide(false);
                }
            }
        } 
        // Use touch input on mobile
        if (Input.touchCount == 1)
        {
			if(m_IsSwiping)
			{
				Vector2 diff = Input.GetTouch(0).position - m_StartingTouch;

				// Put difference in Screen ratio, but using only width, so the ratio is the same on both
                // axes (otherwise we would have to swipe more vertically...)
				diff = new Vector2(diff.x/Screen.width, diff.y/Screen.width);

				if(diff.magnitude > 0.01f) //we set the swip distance to trigger movement to 1% of the screen width
				{
					if(Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
					{
						if(diff.y < 0)
						{
							Slide();
						}
						else
						{
							Jump();
						}
					}
					else
					{
						if(diff.x < 0)
						{
							ChangeLane(-1);
						}
						else
						{
							ChangeLane(1);
						}
					}
						
					m_IsSwiping = false;
				}
            }

        	// Input check is AFTER the swip test, that way if TouchPhase.Ended happen a single frame after the Began Phase
			// a swipe can still be registered (otherwise, m_IsSwiping will be set to false and the test wouldn't happen for that began-Ended pair)
			if(Input.GetTouch(0).phase == TouchPhase.Began)
			{
				m_StartingTouch = Input.GetTouch(0).position;
				m_IsSwiping = true;
			}
			else if(Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				m_IsSwiping = false;
			}
        }
    }

    void Jump()
    {
        if (!m_IsRunning || m_Jumping) return;

        m_Jumping = true;
        m_jumpStartTime = Time.time; // 점프 시작 위치 저장
        m_JumpStart = transform.position.y;

        //애니메이션 재생
        animator.SetTrigger("OnJump");

        // 점프 사운드 재생
        AudioManager.Instance.SFXPlay(gameObject, 2);
    }

    void Slide()
    {
        // 이미 슬라이드 중이거나 캐릭터가 달리고 있지 않다면 슬라이드 시작하지 않음
        if (!m_IsRunning || m_Sliding) return;

        m_Sliding = true;
        slideStartTime = Time.time; // 슬라이드 시작 시간 기록

        //애니메이션 재생
        animator.SetTrigger("OnSliding");

        // 슬라이드 크기 조정
        //characterCollider.Slide(true);
    }

    void ChangeLane(int direction)
    {
        if (!m_IsRunning || m_chageLane) return; // 이미 레인 변경 중이라면 실행하지 않음

        m_LaneDirection = direction; // 이동 방향 설정

        m_chageLane = true;
        m_chageLaneTime = Time.time; // 레인 변경 시작 시간 저장
        m_chageLaneStart = transform.position.x; // 현재 X 위치 저장

        // 애니메이션 재생
        if (direction == 1)
            animator.SetBool("OnDragRight",true); // 오른쪽으로 변경
        else if (direction == -1)
            animator.SetBool("OnDragLeft", true); // 왼쪽으로 변경
    }

    private void OnAnimatorMove()
    {
        if (animator == null || rb == null)
            return;
        if (m_Jumping)
            return;

        // Root Motion에서 이동 값을 가져옴
        rootMotionVelocity = animator.deltaPosition / Time.deltaTime;

        // Rigidbody를 사용하여 이동 적용 (중력 포함)
        Vector3 newVelocity = new Vector3(rootMotionVelocity.x, verticalVelocity, rootMotionVelocity.z);
        rb.velocity = newVelocity;

        // Root Motion 회전 값도 적용
        rb.MoveRotation(animator.rootRotation);
    }

    // 바닥에 있는지 확인 (Raycast를 사용)
    private bool IsGrounded()
    {
        float CheckDistance = 0.1f; // 바닥 체크 거리
        Vector3 capsuleBottom = transform.position - new Vector3(0, 0.5f, 0); // 캡슐의 아래쪽
        Vector3 capsuleTop = transform.position + new Vector3(0, 0.5f, 0);   // 캡슐의 위쪽
        float capsuleRadius = 0.45f; // 캡슐의 반지름
        return Physics.CheckCapsule(capsuleBottom, capsuleTop, capsuleRadius);
    }

}
