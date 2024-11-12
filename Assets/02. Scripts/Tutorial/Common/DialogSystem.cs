using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using System;

public enum Speaker
{
    리틀릿 = 2,
    작고하얀무언가 = 1,
    페니 = 0
}


public class DialogSystem : MonoBehaviour
{
	[SerializeField]
	private	Dialog[]			dialogs;                        // 현재 분기의 대사 목록

	[SerializeField]
	private GameObject[]			portraits;

	[SerializeField]
	private TMP_Text[]            textDialogues;					// 현재 대사 출력 Text UI
	[SerializeField]
	private	float				typingSpeed;                    // 텍스트 타이핑 효과의 재생 속도
	[SerializeField]
	private GameObject          dialogPanel;                    // dialog UI들어 있는 Panel Object

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

        SetNextDialog();
	}

	public int UpdateDialog()
	{
		if ( Input.GetKeyDown(keyCodeSkip) || Input.GetMouseButtonDown(0) )
		{
			// 텍스트 타이핑 효과를 재생중일때 마우스 왼쪽 클릭하면 타이핑 효과 종료
			if ( isTypingEffect == true )
			{

				// 타이핑 효과를 중지하고, 현재 대사 전체를 출력한다
				StopCoroutine("TypingText");
				isTypingEffect = false;
				textDialogues[(int)currentSpeaker].text = dialogs[currentIndex].dialogue;

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
                // 전 화자의 초상화를 Active false
                portraits[(int)currentSpeaker].SetActive(false);
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

		// 현재 화자로 초상화 변경
		portraits[(int)currentSpeaker].SetActive(true);

		StartCoroutine(nameof(TypingText));
	}

    private IEnumerator TypingText()
	{
		int index = 0;
		
		isTypingEffect = true;

		// 텍스트를 한글자씩 타이핑치듯 재생
		while ( index < dialogs[currentIndex].dialogue.Length  + 1)
		{
			textDialogues[(int)currentSpeaker].text = dialogs[currentIndex].dialogue.Substring(0, index);
			if(dialogs[currentIndex].dialogue.Substring(index) != " ")
			{
               // AudioManager.Instance.InstantSFXPlay(typingSFX);
            }
			
			index ++;

			yield return new WaitForSeconds(typingSpeed);
		}

		isTypingEffect = false;

	}
}

[System.Serializable]
public struct Dialog
{
	public	Speaker		speaker;	// 화자
	[TextArea(3, 5)]
	public	string		dialogue;	// 대사
}

