using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.TextCore.Text;

public class TutorialCollider : TutorialBase
{
    bool isfoodEnter;
    public override void Enter()
    {
        isfoodEnter = false;
    }

    public override void Execute(TutorialController controller)
    {
        if (isfoodEnter) { controller.SetNextTutorial(); }
    }

    public override void Exit()
    {
        isfoodEnter = false;
    }

    public override void Skip(TutorialController controller)
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            AudioManager.Instance.SFXPlay(gameObject, 0);
        }

        else if(other.gameObject.tag == "Food")
        {
            isfoodEnter = true;
        }


    }
}
