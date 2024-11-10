using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TutorialMorseInput : TutorialBase
{
    [SerializeField]
    private TMP_Text        displayText;            // 모르스 부호가 보여지는 Text
    [SerializeField]
    private GameObject      explanationObject;      // 어떻게 입력해야지 설명해주는 GameObject
    [SerializeField]
    private string          WordsToCheck;           // 검사 할 문장

    [SerializeField]
    private GameObject      platoon;                // 반응하는 EnemyObject
    [SerializeField]
    private GameObject      reactionUI;             // Instance 할 UI
    [SerializeField]
    private Transform      tutorialCanver;         // Instance 되었을 때, 부모가 될 Canver


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
        print("들어옴");
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
