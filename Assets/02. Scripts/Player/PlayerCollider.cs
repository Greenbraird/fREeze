using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerCollider : MonoBehaviour
{
     SplineAnimate _splineAnimate;

    void Start()
    {
        _splineAnimate = GetComponent<SplineAnimate>();
    }

    private void OnTriggerEnter(Collider other)
    {
        SplineContainer otherspline = other.gameObject.GetComponent<SplineContainer>();

        _splineAnimate.Container = otherspline;
        _splineAnimate.Play();

        float initpositionY = transform.position.y;

        StartCoroutine(SplineEndInitY(initpositionY));
        
    }

    IEnumerator SplineEndInitY(float positionY)
    {
        yield return new WaitForSeconds(3);
        transform.position = new Vector3(transform.position.x, positionY, transform.position.z);

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
