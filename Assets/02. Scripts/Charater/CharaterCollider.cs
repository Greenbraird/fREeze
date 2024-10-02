using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class CharaterCollider : MonoBehaviour
{
    [Header("Ragdoll")]
    public GameObject charater;
    public GameObject charaterRagdoll;
    public Rigidbody spine; 

     SplineAnimate _splineAnimate;

    bool animationFinished = false;

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

                // �ִϸ��̼� ���� ���� �߰� �ൿ�� ���⼭ ���� ����
                SplineEndInitY(transform.position.y);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
        }
        else
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
        // �浹�� ��ü�� �±� ��������
        string collidedObjectTag = collision.gameObject.tag;

        //// �±׿� ���� �ٸ� ���� ����
        if (collidedObjectTag == "Food")
        {
            Debug.Log("��ֹ��̶� �����߽��ϴ�!");
            //�浹 ��  �ʿ��� event

            charaterRagdoll.transform.position = charater.transform.position;

            charaterRagdoll.SetActive(true);
            spine.AddForce(charaterRagdoll.transform.forward * -200f, ForceMode.Impulse);
            charater.SetActive(false);

            Invoke("callEndMassage", 2f);
        }
     }

    void callEndMassage()
    {
        GameSystem.Instance.GameEnd();
    }

     
}
