using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;

public class FridgeInputCotroller : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Animator pointLightAnimaor;
    [SerializeField] private Animator spotLightAnimaor;
    private float Main_Start_LowPassValue;

    Camera _camera;

    Rigidbody rbFridgeDoor;

    private Vector3 touchStartPos;
    private bool touchable = true;

    void Start()
    {
        _camera = Camera.main;
        rbFridgeDoor = GetComponent<Rigidbody>();
        audioMixer.SetFloat("BGMLowpass", 500f);

    }

    void OnMouseDown()
    {
        touchStartPos = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        float slideDistanceX = (Input.mousePosition.x - touchStartPos.x) / Screen.width;
        if (Mathf.Abs(slideDistanceX) > 0.1f)
        {
            if (slideDistanceX > 0 && touchable)
            {
                // ���������� �����̵�
                Debug.Log("Right Slide");
                OpenFridge();

            }
        }
    }
    /// <summary>
    /// ����� ���� ���� ���� �Լ�
    /// </summary>
    void OpenFridge()
    {

        Transform fridgeHandle = transform.GetChild(0);

        fridgeHandle.DOLocalRotate(new Vector3(0, 0, -160),0.5f);

        AudioManager.Instance.SFXPlay(gameObject, 1);
        touchable = false;

        //ī�޶� ������ �ٰ���
        _camera.transform.DOMoveZ(-12.9f, 2f).SetEase(Ease.OutCirc).OnComplete(()=> 
        {
            rbFridgeDoor.freezeRotation = true;
        });

        pointLightAnimaor.SetTrigger("OpneTrigger");
        spotLightAnimaor.SetTrigger("OpneTrigger");

        //Audio LowPass ����
        audioMixer.GetFloat("BGMLowpass", out Main_Start_LowPassValue);
        DOTween.To(() => Main_Start_LowPassValue, x => Main_Start_LowPassValue = x, 5000f, 2f).OnUpdate(() =>
        {
            // AudioMixer�� LowPassCutoff �Ķ���� ���� �ǽð����� ������Ʈ
            audioMixer.SetFloat("BGMLowpass", Main_Start_LowPassValue);
        });

        // Rigidbody�� ���� ���� (���� ��ǥ ����)
        rbFridgeDoor.AddForce(-Vector3.forward.normalized * 7, ForceMode.Impulse);
    }

}
