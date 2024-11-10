using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : TutorialBase
{
    [SerializeField]
    private GameObject arrow;

    private bool isPointerExit = false;
    public override void Enter()
    {
        if(arrow != null) { arrow.SetActive(true); }

    }

    public override void Execute(TutorialController controller)
    {
        if(isPointerExit)
        {
            controller.SetNextTutorial();
        }
        
    }

    public override void Exit()
    {
        if (arrow != null) arrow.SetActive(false);
    }

    public void ExitTrigger()
    {
         isPointerExit = true;
    }

    public override void Skip(TutorialController controller)
    {
        controller.SetNextTutorial(2);
    }
}
