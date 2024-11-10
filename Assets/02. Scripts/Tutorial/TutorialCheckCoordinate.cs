 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TutorialCheckCoordinate : TutorialBase
{
    [SerializeField]
    private TMP_Text displayText;            // 모르스 부호가 보여지는 Text
    [SerializeField]
    private GameObject explanationObject;      // 어떻게 입력해야지 설명해주는 GameObject
    [SerializeField]
    private string WordsToCheck;           // 검사 할 문장

    public override void Enter()
    {
        explanationObject.SetActive(true);
    }

    public override void Execute(TutorialController controller)
    {
        try
        {
            if (WordsToCheck != "" && displayText.text.Contains(WordsToCheck))
            {
                controller.SetNextTutorial();
            }
            else if(displayText.text.Length == 2)
            {
                displayText.text = displayText.text.Substring(0, displayText.text.Length-2);
            }
        }
        catch
        {
            return;
        }
    }

    public override void Exit()
    {
        explanationObject.SetActive(false);
    }

}
