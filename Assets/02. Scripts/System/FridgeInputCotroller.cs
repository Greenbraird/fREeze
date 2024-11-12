using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;

public class FridgeInputCotroller : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private List<Animator> LightAnimaor = new List<Animator>();

    [SerializeField] private GameObject tutorialController;

    [SerializeField] private GameObject slideTutorialPanel;

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
                // 오른쪽으로 슬라이드
                Debug.Log("Right Slide");
                OpenFridge();

            }
        }
    }

    /// <summary>
    /// 냉장고 문이 열릴 때의 함수
    /// </summary>
    void OpenFridge()
    {

        // slideTutorialPanel UI를 비활성화 시킴
        slideTutorialPanel.SetActive(false);

        // 냉장고 Handle를 받아서 animation 시킴
        Transform fridgeHandle = transform.GetChild(0);
        fridgeHandle.DOLocalRotate(new Vector3(0, 0, -160),0.5f);

        // 냉장고 열리는 소리 SFX
        AudioManager.Instance.SFXPlay(gameObject, 1);

        touchable = false;

        // main 카메라의 포커스를 받음.
        float camera_force_leght = _camera.focalLength;

        // main 카메라의 포커스를 앞으로 땅기는 Animation
        DOTween.To(()=> camera_force_leght, x => camera_force_leght = x, 94, 2).OnUpdate(() =>
        {
            _camera.focalLength = camera_force_leght;
        }).OnComplete(()=>
        {
            // 문을 움직임을 freeze 시킨다.
            rbFridgeDoor.freezeRotation = true;

            // Tutorial 시작
            tutorialController.SetActive(true);


        }); //카메라가 앞으로 다가감

        foreach (Animator a in LightAnimaor)
        {
            a.SetTrigger("OpneTrigger");
        }

        //Audio LowPass 적용
        audioMixer.GetFloat("BGMLowpass", out Main_Start_LowPassValue);
        DOTween.To(() => Main_Start_LowPassValue, x => Main_Start_LowPassValue = x, 5000f, 2f).OnUpdate(() =>
        {
            // AudioMixer의 LowPassCutoff 파라미터 값을 실시간으로 업데이트
            audioMixer.SetFloat("BGMLowpass", Main_Start_LowPassValue);
        });

        // Rigidbody에 힘을 가함 (월드 좌표 기준)
        rbFridgeDoor.AddForce(-Vector3.forward.normalized * 7, ForceMode.Impulse);

        
        
    }

}
