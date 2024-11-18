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


    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 물체의 태그 가져오기
        string collidedObjectTag = collision.gameObject.tag;

        // 태그에 따라 다른 동작 수행
        if (collidedObjectTag == "Food")
        {
            isfoodEnter = true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            AudioManager.Instance.SFXPlay(gameObject, 0);
        }


    }
}
