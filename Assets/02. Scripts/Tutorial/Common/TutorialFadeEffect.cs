using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialFadeEffect : TutorialBase
{
    [SerializeField]
    private bool isFadeIn = false;
    private bool isCompleted = false;

    [SerializeField]
    private Image fadeImage;

    public override void Enter()
    {
        if(isFadeIn == true)
        {
            fadeImage.DOFade(0, 1).From().OnComplete(() => isCompleted = true);
        }
        else
        {
            fadeImage.DOFade(0, 1).OnComplete(() => isCompleted = true);
        }
    }

    public override void Execute(TutorialController controller)
    {
        if(isCompleted== true)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
    }

    public override void Skip(TutorialController controller)
    {
        controller.SetNextTutorial(2);
    }
}
