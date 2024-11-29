using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class CharaterCollider : MonoBehaviour
{
    Animator _animator;


    [Header("Event")]
    public CoinEvent coinEvent;
    public GameEvent gameEvent;
    
    //spline Animate
    SplineAnimate _splineAnimate;

    //animation Finished bool
    bool animationFinished = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _splineAnimate = null;
    }

    void Update()
    {
        // SplineAnimate�� ������ �� ���൵�� Ȯ��
        if (_splineAnimate != null && !animationFinished)
        {
            // NormalizedTime�� 1.0f�� �����ϸ� �ִϸ��̼� ����� ����
            if (_splineAnimate.NormalizedTime >= 1.0f)
            {
                Debug.Log("Spline animation finished.");
                animationFinished = true;
                _animator.applyRootMotion = false;

                // �ִϸ��̼� ���� ���� �߰� �ൿ�� ���⼭ ���� ����
                SplineEndInitY(transform.position.y);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Coin")
        {
            other.gameObject.SetActive(false);
            coinEvent.Increase(1);

            AudioManager.Instance.SFXPlay(gameObject, 0);
        }
        else if (other.gameObject.tag == "Spline")
        {
            _animator.applyRootMotion = true;
            GameSystem.Instance.touchable = false;
            animationFinished = false;

            if (_splineAnimate == null)
            {
                _splineAnimate = gameObject.AddComponent<SplineAnimate>();
            }
            _splineAnimate.Loop = SplineAnimate.LoopMode.Once;

            SplineContainer otherspline = other.gameObject.GetComponent<SplineContainer>();

            _splineAnimate.Container = otherspline;

            _splineAnimate.Play();

            other.gameObject.SetActive(false);
            other.gameObject.SetActive(true);


        }
        
        
    }

    void SplineEndInitY(float positionY)
    {
        
        transform.position = new Vector3(transform.position.x, positionY, transform.position.z);
        Destroy(_splineAnimate);
        GameSystem.Instance.touchable = true;    
    }

   
     void OnCollisionEnter(Collision collision)
     {
        // �浹�� ��ü�� �±� ��������
        string collidedObjectTag = collision.gameObject.tag;

        //// �±׿� ���� �ٸ� ���� ����
        if (collidedObjectTag == "Food")
        {
            Debug.Log("��ֹ��̶� �����߽��ϴ�!");

            //�浹 ��  �ʿ��� event
            Camera.main.transform.SetParent(null);

            _animator.SetBool("OnDragRight", false); // ���������� ����
            _animator.SetBool("OnDragLeft", false); // ���������� ����
            _animator.SetTrigger("Dead");
            GameSystem.Instance.IsGamestart = false;


            //charaterRagdoll.transform.position = gameObject.transform.position;

            //charaterRagdoll.SetActive(true);
            //spine.AddForce(charaterRagdoll.transform.forward * -200f, ForceMode.Impulse);
            //gameObject.SetActive(false);

            Invoke("callEndMassage", 2f);
        }
        else if(collidedObjectTag == "Finish")
        {
            _animator.SetBool("Run", false);
            GameSystem.Instance.IsGamestart = false;
            gameEvent.FinishEvent();

        }
     }

    void callEndMassage()
    {
        GameSystem.Instance.GameEnd();
    }

     
}
