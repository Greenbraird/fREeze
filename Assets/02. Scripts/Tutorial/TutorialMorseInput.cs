using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TutorialMorseInput : TutorialBase
{
    [SerializeField]
    private TMP_Text        displayText;            // �𸣽� ��ȣ�� �������� Text
    [SerializeField]
    private GameObject      explanationObject;      // ��� �Է��ؾ��� �������ִ� GameObject
    [SerializeField]
    private string          WordsToCheck;           // �˻� �� ����

    [SerializeField]
    private GameObject      platoon;                // �����ϴ� EnemyObject
    [SerializeField]
    private GameObject      reactionUI;             // Instance �� UI
    [SerializeField]
    private Transform      tutorialCanver;         // Instance �Ǿ��� ��, �θ� �� Canver


    public override void Enter()
    {
        explanationObject.SetActive(true);
    }

    public override void Execute(TutorialController controller)
    {
        try
        {
            if(displayText.text.Split(" ")[0] == WordsToCheck)
            {
                displayText.text = "";
                StartCoroutine(nameof(EnemyReaction));
                controller.SetNextTutorial();
            }
            else
            {
                displayText.text = "";
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

    IEnumerator EnemyReaction()
    {
        print("����");
        if (reactionUI != null)
        {
            GameObject instanceReaction = Instantiate(reactionUI, tutorialCanver);
            

            RectTransform rectPosition = instanceReaction.GetComponent<RectTransform>();


            Vector3 screenPosition = Camera.main.WorldToScreenPoint(platoon.transform.position);

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                tutorialCanver.GetComponent<RectTransform>(), 
                screenPosition,                          
                Camera.main,                               
                out localPoint                           
            );

            rectPosition.localPosition = localPoint + new Vector2(0, 170);

            rectPosition.DOAnchorPosY(rectPosition.localPosition.y + 30, 2);
            rectPosition.GetComponent<Image>().DOFade(1, 2);
            yield return new WaitForSeconds(2);
            rectPosition.GetComponent<Image>().DOFade(0, 2).OnComplete(()=> Destroy(instanceReaction));
        }
        
    }
}
