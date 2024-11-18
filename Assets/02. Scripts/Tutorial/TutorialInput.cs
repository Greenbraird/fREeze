using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public enum RequestInput
{
    Right=0, Left=1, Up=2, Down=3
}

public class TutorialInput : TutorialBase
{

    [SerializeField]
    private Animator characteranimator;
    [SerializeField]
    private CharaterMovement characteraterMovement;

    [SerializeField]
    private CharacterInputHandler characterInputHandler;

    [SerializeField]
    private GameObject slidePanel;

    [SerializeField]
    private GameObject[] slideText;

    [SerializeField]
    private RequestInput requestinput;
    
    int speedtmp = 0;


    // Start is called before the first frame update
    void Start()
    {
        characterInputHandler.isRight = false;
        characterInputHandler.isLeft = false;
        characterInputHandler.isUp = false;
        characterInputHandler.isDown = false;

        speedtmp = characteraterMovement.speed;
    }

    public override void Enter()
    {
        slidePanel.SetActive(true);
        slideText[(int)requestinput].SetActive(true);

        characteranimator.SetBool("Run", false);
        characteraterMovement.speed = 0;
    }

    public override void Execute(TutorialController controller)
    {
        if (requestinput == RequestInput.Right && characterInputHandler.isRight)
        {
            controller.SetNextTutorial();
        }
        else if (requestinput == RequestInput.Left && characterInputHandler.isLeft)
        {
            controller.SetNextTutorial();
        }
        else if (requestinput == RequestInput.Up && characterInputHandler.isUp)
        {
            controller.SetNextTutorial();
        }
        else if (requestinput == RequestInput.Down && characterInputHandler.isDown)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        slidePanel.SetActive(false);
        slideText[(int)requestinput].SetActive(false);
        characteranimator.SetBool("Run", true);
        characteraterMovement.speed = speedtmp;
    }

    public override void Skip(TutorialController controller)
    {
        throw new System.NotImplementedException();
    }
}
