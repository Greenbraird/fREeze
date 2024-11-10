using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using System;

public enum Speaker
{
    [Description("<color=#cc0000>리틀릿</color=#cc0000>")]
    리틀릿 = 0,

    [Description("작고 하얀 무언가")]
    작고하얀무언가 = 1,

    [Description("페니")]
    페니 = 2
}


public class DialogSystem : MonoBehaviour
{
	[SerializeField]
	private	Dialog[]			dialogs;                        // 현재 분기의 대사 목록

	[SerializeField]
	private GameObject[]			portraits;

	[SerializeField]
	private	TMP_Text	        textNames;						// 현재 대사중인 캐릭터 이름 출력 Text UI
	[SerializeField]
	private TMP_Text            textDialogues;					// 현재 대사 출력 Text UI
	[SerializeField]
	private	GameObject  		objectArrows;					// 대사가 완료되었을 때 출력되는 커서 오브젝트
	[SerializeField]
	private	float				typingSpeed;                    // 텍스트 타이핑 효과의 재생 속도
	[SerializeField]
	private GameObject          dialogPanel;                    // dialog UI들어 있는 Panel Object
	[SerializeField]
	private AudioClip			typingSFX;                      // 타이핑 animation이 나올 때, 재생됨
	[SerializeField]
	private AudioClip			ClickSFX;						// Click 소리 SFX

    [SerializeField]
	private	KeyCode				keyCodeSkip = KeyCode.Space;    // 타이핑 효과를 스킵하는 키

	[SerializeField]
    private bool IsskipDialog = false;          // Ture면 다음 Dialog는 건너 뜀

    private	int					currentIndex = -1;
	private	bool				isTypingEffect = false;			// 텍스트 타이핑 효과를 재생중인지
	private	Speaker				currentSpeaker = Speaker.리틀릿;

	

	public void Setup()
	{
		dialogPanel.SetActive(true);
        objectArrows.SetActive(false);

        SetNextDialog();
	}

	public int UpdateDialog()
	{
		if ( Input.GetKeyDown(keyCodeSkip) || Input.GetMouseButtonDown(0) )
		{
			// 텍스트 타이핑 효과를 재생중일때 마우스 왼쪽 클릭하면 타이핑 효과 종료
			if ( isTypingEffect == true )
			{
				//ClickSFX Play
				//AudioManager.Instance.SFXPlay(gameObject, "ClickSFX", ClickSFX);

				// 타이핑 효과를 중지하고, 현재 대사 전체를 출력한다
				StopCoroutine("TypingText");
				isTypingEffect = false;
				textDialogues.text = dialogs[currentIndex].dialogue;
				// 대사가 완료되었을 때 출력되는 커서 활성화
				objectArrows.SetActive(true);

				return 0;
			}

			// 다음 대사 진행
			if ( dialogs.Length > currentIndex + 1 )
			{
				SetNextDialog();
			}
			// 대사가 더 이상 없을 경우 true 반환
			else
			{
				dialogPanel.SetActive(false);
				if (IsskipDialog == true) {
                    return 2;
                }
				else { return 1; }
				
			}
		}

		return 0;
	}

	private void SetNextDialog()
	{
	
		// 전 화자의 초상화를 Active false
        portraits[(int)currentSpeaker].SetActive(false);

        currentIndex ++;

		// 현재 화자 설정
		currentSpeaker = dialogs[currentIndex].speaker;

		// 현재 화자 이름 텍스트 활성화 및 설정
		textNames.gameObject.SetActive(true);
		textNames.text = GetDescription(currentSpeaker);

		// 현재 화자로 초상화 변경
		portraits[(int)currentSpeaker].SetActive(true);

		// 화자의 대사 텍스트 활성화 및 설정 (Typing Effect)
		textDialogues.gameObject.SetActive(true);
		StartCoroutine(nameof(TypingText));
	}

    public static string GetDescription(Speaker speaker)
    {
        var field = speaker.GetType().GetField(speaker.ToString());
        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        return attribute != null ? attribute.Description : speaker.ToString();
    }

    private IEnumerator TypingText()
	{
		int index = 0;
		
		isTypingEffect = true;

		// 텍스트를 한글자씩 타이핑치듯 재생
		while ( index < dialogs[currentIndex].dialogue.Length  + 1)
		{
			textDialogues.text = dialogs[currentIndex].dialogue.Substring(0, index);
			if(dialogs[currentIndex].dialogue.Substring(index) != " ")
			{
               // AudioManager.Instance.InstantSFXPlay(typingSFX);
            }
			
			index ++;

			yield return new WaitForSeconds(typingSpeed);
		}

		isTypingEffect = false;

		// 대사가 완료되었을 때 출력되는 커서 활성화
		objectArrows.SetActive(true);
	}
}

[System.Serializable]
public struct Dialog
{
	public	Speaker		speaker;	// 화자
	[TextArea(3, 5)]
	public	string		dialogue;	// 대사
}

