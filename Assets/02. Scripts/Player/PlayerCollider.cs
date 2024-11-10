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
        // �浹�� ��ü�� �±� ��������
        string collidedObjectTag = collision.gameObject.tag;

        //// �±׿� ���� �ٸ� ���� ����
        if (collidedObjectTag == "cube")
        {
            Debug.Log("��ֹ��̶� �����߽��ϴ�!");
            //�浹 ��  �ʿ��� event
        }
        else if(collidedObjectTag == "Star")
        {
            collision.gameObject.SetActive(false);
            Debug.Log("��ü�� �浹�߽��ϴ�");
            UIManager.Instance.GetCoinSliderEvent();
            AudioManager.Instance.GetCoinSFXPlay();
        }
    }
     **/
}
