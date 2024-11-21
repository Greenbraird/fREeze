using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.TextCore.Text;

public class TutorialCollider : TutorialBase
{
    bool isfoodEnter;

    public GameObject mainCharacter;

    private void Update()
    {
        transform.position = mainCharacter.transform.position;
    }
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
        print(other.gameObject.tag);

        if (other.gameObject.tag == "Food")
        {
            isfoodEnter = true;
        }


    }
}
