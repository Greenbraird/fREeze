using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class CharaterCollider : MonoBehaviour
{
    [Header("Ragdoll")]
    public GameObject charaterRagdoll;
    public Rigidbody spine; 

     SplineAnimate _splineAnimate;

    bool animationFinished = false;

    private void Start()
    {
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

                // 애니메이션 종료 시의 추가 행동을 여기서 정의 가능
                SplineEndInitY(transform.position.y);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            AudioManager.Instance.SFXPlay(gameObject, 0);
        }
        else if (other.gameObject.tag == "Spline")
        {
            animationFinished = false;
            GameSystem.Instance.touchable = false;

            if (_splineAnimate == null)
            {
                _splineAnimate = gameObject.AddComponent<SplineAnimate>();
            }
            _splineAnimate.Loop = SplineAnimate.LoopMode.Once;

            SplineContainer otherspline = other.gameObject.GetComponent<SplineContainer>();

            _splineAnimate.Container = otherspline;
            _splineAnimate.Play();
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

            charaterRagdoll.transform.position = gameObject.transform.position;

            Camera.main.transform.SetParent(null);

            charaterRagdoll.SetActive(true);
            spine.AddForce(charaterRagdoll.transform.forward * -200f, ForceMode.Impulse);
            gameObject.SetActive(false);

            Invoke("callEndMassage", 2f);
        }
     }

    void callEndMassage()
    {
        GameSystem.Instance.GameEnd();
    }

     
}
