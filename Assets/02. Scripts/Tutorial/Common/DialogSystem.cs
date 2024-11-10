using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Speaker { 레마르크 = 0, 낵캐러웨이 = 1, 로버트헤이즈 }

public class DialogSystem : MonoBehaviour
{
	[SerializeField]
	private	Dialog[]			dialogs;                        // 현재 분기의 대사 목록

	[SerializeField]
	private Sprite[]			portraits;

	[SerializeField]
	private	Image				imageDialogs;					// 대화창 Image UI
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
	private	KeyCode				keyCodeSkip = KeyCode.Space;	// 타이핑 효과를 스킵하는 키

	private	int					currentIndex = -1;
	private	bool				isTypingEffect = false;			// 텍스트 타이핑 효과를 재생중인지
	private	Speaker				currentSpeaker = Speaker.낵캐러웨이;

	public void Setup()
	{
		dialogPanel.SetActive(true);
        objectArrows.SetActive(false);

        SetNextDialog();
	}

	public bool UpdateDialog()
	{
		if ( Input.GetKeyDown(keyCodeSkip) || Input.GetMouseButtonDown(0) )
		{
			// 텍스트 타이핑 효과를 재생중일때 마우스 왼쪽 클릭하면 타이핑 효과 종료
			if ( isTypingEffect == true )
			{
				//ClickSFX Play
				AudioManager.Instance.SFXPlay(gameObject, "ClickSFX", ClickSFX);

				// 타이핑 효과를 중지하고, 현재 대사 전체를 출력한다
				StopCoroutine("TypingText");
				isTypingEffect = false;
				textDialogues.text = dialogs[currentIndex].dialogue;
				// 대사가 완료되었을 때 출력되는 커서 활성화
				objectArrows.SetActive(true);

				return false;
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

				return true;
			}
		}

		return false;
	}

	private void SetNextDialog()
	{
		currentIndex ++;

		// 현재 화자 설정
		currentSpeaker = dialogs[currentIndex].speaker;

		// 대화창 활성화
		imageDialogs.gameObject.SetActive(true);

		// 현재 화자 이름 텍스트 활성화 및 설정
		textNames.gameObject.SetActive(true);
		textNames.text = dialogs[currentIndex].speaker.ToString();

		// 현재 화자로 초상화 변경
		imageDialogs.sprite = portraits[(int)currentSpeaker];

		// 화자의 대사 텍스트 활성화 및 설정 (Typing Effect)
		textDialogues.gameObject.SetActive(true);
		StartCoroutine(nameof(TypingText));
	}

	private IEnumerator TypingText()
	{
		int index = 0;
		
		isTypingEffect = true;

		// 텍스트를 한글자씩 타이핑치듯 재생
		while ( index < dialogs[currentIndex].dialogue.Length )
		{
			textDialogues.text = dialogs[currentIndex].dialogue.Substring(0, index);
			if(dialogs[currentIndex].dialogue.Substring(index) != " ")
			{
                AudioManager.Instance.InstantSFXPlay(typingSFX);
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

