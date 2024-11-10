 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TutorialCheckCoordinate : TutorialBase
{
    [SerializeField]
    private TMP_Text displayText;            // �𸣽� ��ȣ�� �������� Text
    [SerializeField]
    private GameObject explanationObject;      // ��� �Է��ؾ��� �������ִ� GameObject
    [SerializeField]
    private string WordsToCheck;           // �˻� �� ����

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
