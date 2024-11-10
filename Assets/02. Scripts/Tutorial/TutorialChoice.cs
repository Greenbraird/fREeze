using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialChoice : TutorialBase
{
    [SerializeField] private GameObject choiceUi;
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TMP_Text[] choices;
    [SerializeField] private string[] texts;

    bool isCleck;
    bool isSkip;

    public override void Enter()
    {
        choiceUi.SetActive(true);
        dialogPanel.SetActive(true);
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
        dialogPanel.SetActive(false);
    }

    public override void Skip(TutorialController controller)
    {
        return;
    }

    public void clickButton(bool i)
    {
        isSkip = i;
        isCleck = true;
    }
}
