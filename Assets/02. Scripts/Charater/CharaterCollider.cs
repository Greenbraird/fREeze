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
        // SplineAnimate가 존재할 때 진행도를 확인
        if (_splineAnimate != null && !animationFinished)
        {
            // NormalizedTime이 1.0f에 도달하면 애니메이션 종료로 간주
            if (_splineAnimate.NormalizedTime >= 1.0f)
            {
                Debug.Log("Spline animation finished.");
                animationFinished = true;
                _animator.applyRootMotion = false;

                // 애니메이션 종료 시의 추가 행동을 여기서 정의 가능
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
        // 충돌한 물체의 태그 가져오기
        string collidedObjectTag = collision.gameObject.tag;

        //// 태그에 따라 다른 동작 수행
        if (collidedObjectTag == "Food")
        {
            Debug.Log("장애물이랑 접촉했습니다!");

            //충돌 시  필요한 event
            Camera.main.transform.SetParent(null);

            _animator.SetBool("OnDragRight", false); // 오른쪽으로 변경
            _animator.SetBool("OnDragLeft", false); // 오른쪽으로 변경
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
