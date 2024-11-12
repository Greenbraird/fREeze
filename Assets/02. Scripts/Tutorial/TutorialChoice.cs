using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialChoice : TutorialBase
{
    [SerializeField] private GameObject choiceUi;
    [SerializeField] private TMP_Text[] choices;
    [SerializeField] private string[] texts;

    bool isCleck;
    bool isSkip;

    public override void Enter()
    {
        choiceUi.SetActive(true);
        isCleck = false;    
        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].text = texts[i];
        }
    }

    public override void Execute(TutorialController controller)
    {
        if(isCleck== true ) 
        {
            if(isSkip == true)
            {
                controller.SetNextTutorial(2);
            }
            else if(isSkip == false)
            {
                controller.SetNextTutorial();
            }
        }
    }
        

    public override void Exit()
    {
        choiceUi.SetActive(false);
    }

    public override void Skip(TutorialController controller)
    {
        return;
    }

    public void clickButton(bool i)
    {
        //ClickSFX Play
        AudioManager.Instance.SFXPlay(gameObject, 4);
        isSkip = i;
        isCleck = true;
    }
}
