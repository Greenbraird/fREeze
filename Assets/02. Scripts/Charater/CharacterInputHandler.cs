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

    private Vector3 rootMotionVelocity; // Root Motion���� ������ �̵� �ӵ�
    public float gravity = -5f;      // �߷� ��
    private float verticalVelocity;    // �߷¿� ���� Y�� �ӵ�



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        m_IsRunning = true;
        m_Jumping = false;
        m_Sliding = false;

        // Rigidbody ����: �߷� Ȱ��ȭ, ���� ��ȣ�ۿ� ����
        rb.useGravity = false; // Root Motion�� �߷� �浹 ����
        rb.isKinematic = false;

        // ĳ������ �̵�
        Vector3 forwardDirection = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        transform.Translate(forwardDirection * (speed * Time.deltaTime), Space.World);

        // ĳ������ ������ ��Ȯ�� �������� ����
        transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

        m_LaneCount =1;
    }

    void Update()
    {

        // �߷� ������Ʈ
        if (IsGrounded())
        {
            verticalVelocity = 0f; // �ٴڿ� ���� �� Y�� �ӵ� �ʱ�ȭ
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // �߷� ���ӵ�
        }

        if (!GameSystem.Instance.IsGamestart) return;
        else animator.SetBool("Run", true);
        if (!GameSystem.Instance.touchable) return;
        if (!m_IsRunning) return;

        transform.Translate(transform.forward * (speed * Time.deltaTime), Space.World);

        // �Է� ó��
        if (Input.GetKeyDown(KeyCode.UpArrow)) Jump();
        if (Input.GetKeyDown(KeyCode.DownArrow)) Slide();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) ChangeLane(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) ChangeLane(1);

        // ���� ���� ������Ʈ
        if (m_Jumping)
        {
            float jumpTime = Time.time - m_jumpStartTime; // ���� ��� �ð�
            float jumpRatio = jumpTime / jumpDuration;
            if (jumpRatio >= 1.0f)
            {
                m_Jumping = false;
            }
            else
            {
                float yOffset = Mathf.Sin(jumpRatio * Mathf.PI) * jumpHeight; // Y������ ����
                transform.position = new Vector3(transform.position.x, m_JumpStart + yOffset, transform.position.z); // Y�ุ ����
            }
        }

        if (m_chageLane)
        {
            float chageLaneTime = Time.time - m_chageLaneTime;
            float chageLane = chageLaneTime / laneDuration;

            Debug.Log("��ȭ �ð� " + chageLaneTime);
            Debug.Log("��ȭ ���� " + chageLane);
            
            if (chageLaneTime > 0.2f)
            {
                m_chageLane = false;
                animator.SetBool("OnDragRight", false); // ���������� ����
                animator.SetBool("OnDragLeft", false); // ���������� ����
            }
            else
            {
                // X�� �̵� ���
                Vector3 xOffset = m_LaneDirection * 2.0f * transform.right; // �̵� �Ÿ� ���

                transform.position = new Vector3(m_chageLaneStart, transform.position.y, transform.position.z) + xOffset;
            }
        }

        if (m_Sliding)
        {
            // �����̵� ������ Ȯ��
            if (m_Sliding)
            {
                // �����̵� ���� �ð��� ������ �����̵� ����
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
        m_jumpStartTime = Time.time; // ���� ���� ��ġ ����
        m_JumpStart = transform.position.y;

        //�ִϸ��̼� ���
        animator.SetTrigger("OnJump");

        // ���� ���� ���
        AudioManager.Instance.SFXPlay(gameObject, 2);
    }

    void Slide()
    {
        // �̹� �����̵� ���̰ų� ĳ���Ͱ� �޸��� ���� �ʴٸ� �����̵� �������� ����
        if (!m_IsRunning || m_Sliding) return;

        m_Sliding = true;
        slideStartTime = Time.time; // �����̵� ���� �ð� ���

        //�ִϸ��̼� ���
        animator.SetTrigger("OnSliding");

        // �����̵� ũ�� ����
        //characterCollider.Slide(true);
    }

    void ChangeLane(int direction)
    {
        if (!m_IsRunning || m_chageLane) return; // �̹� ���� ���� ���̶�� �������� ����

        m_LaneDirection = direction; // �̵� ���� ����

        m_chageLane = true;
        m_chageLaneTime = Time.time; // ���� ���� ���� �ð� ����
        m_chageLaneStart = transform.position.x; // ���� X ��ġ ����

        // �ִϸ��̼� ���
        if (direction == 1)
            animator.SetBool("OnDragRight",true); // ���������� ����
        else if (direction == -1)
            animator.SetBool("OnDragLeft", true); // �������� ����
    }

    private void OnAnimatorMove()
    {
        if (animator == null || rb == null)
            return;
        if (m_Jumping)
            return;

        // Root Motion���� �̵� ���� ������
        rootMotionVelocity = animator.deltaPosition / Time.deltaTime;

        // Rigidbody�� ����Ͽ� �̵� ���� (�߷� ����)
        Vector3 newVelocity = new Vector3(rootMotionVelocity.x, verticalVelocity, rootMotionVelocity.z);
        rb.velocity = newVelocity;

        // Root Motion ȸ�� ���� ����
        rb.MoveRotation(animator.rootRotation);
    }

    // �ٴڿ� �ִ��� Ȯ�� (Raycast�� ���)
    private bool IsGrounded()
    {
        float CheckDistance = 0.1f; // �ٴ� üũ �Ÿ�
        Vector3 capsuleBottom = transform.position - new Vector3(0, 0.5f, 0); // ĸ���� �Ʒ���
        Vector3 capsuleTop = transform.position + new Vector3(0, 0.5f, 0);   // ĸ���� ����
        float capsuleRadius = 0.45f; // ĸ���� ������
        return Physics.CheckCapsule(capsuleBottom, capsuleTop, capsuleRadius);
    }

}
