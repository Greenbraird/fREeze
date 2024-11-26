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
        // �浹�� ��ü�� �±� ��������
        string collidedObjectTag = collision.gameObject.tag;

        //// �±׿� ���� �ٸ� ���� ����
        if (collidedObjectTag == "Food")
        {
            Debug.Log("��ֹ��̶� �����߽��ϴ�!");
            //�浹 ��  �ʿ��� event

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
