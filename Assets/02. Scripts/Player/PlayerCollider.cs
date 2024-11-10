using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerCollider : MonoBehaviour
{
     SplineAnimate _splineAnimate;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
        }
        else
        {
            _splineAnimate = gameObject.AddComponent<SplineAnimate>();
            _splineAnimate.Loop = SplineAnimate.LoopMode.Once;

            SplineContainer otherspline = other.gameObject.GetComponent<SplineContainer>();

            _splineAnimate.Container = otherspline;
            _splineAnimate.Play();

            float initpositionY = transform.position.y;

            StartCoroutine(SplineEndInitY(initpositionY));
        }
        
        
    }

    IEnumerator SplineEndInitY(float positionY)
    {
        yield return new WaitForSeconds(3);
        transform.position = new Vector3(transform.position.x, positionY, transform.position.z);
        Destroy(_splineAnimate);
    }

    /**
     void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        // 충돌한 물체의 태그 가져오기
        string collidedObjectTag = collision.gameObject.tag;

        //// 태그에 따라 다른 동작 수행
        if (collidedObjectTag == "cube")
        {
            Debug.Log("장애물이랑 접촉했습니다!");
            //충돌 시  필요한 event
        }
        else if(collidedObjectTag == "Star")
        {
            collision.gameObject.SetActive(false);
            Debug.Log("물체와 충돌했습니다");
            UIManager.Instance.GetCoinSliderEvent();
            AudioManager.Instance.GetCoinSFXPlay();
        }
    }
     **/
}
