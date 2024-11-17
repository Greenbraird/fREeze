using UnityEngine;

[RequireComponent(typeof(DialogSystem))]
public class TutorialDialog : TutorialBase
{
	// 캐릭터들의 대사를 진행하는 DialogSystem
	private	DialogSystem dialogSystem;

	public override void Enter()
	{
		dialogSystem = GetComponent<DialogSystem>();
		dialogSystem.Setup();
	}

	public override void Execute(TutorialController controller)
	{
		// 현재 분기에 진행되는 대사 진행
		int isCompleted = dialogSystem.UpdateDialog();

        //AudioManager.Instance.SFXPlay(gameObject, 4);
        // 현재 분기의 대사 진행이 완료되면
        if (isCompleted == 1)
		{
            //ClickSFX Play
            
            // 다음 튜토리얼로 이동
            controller.SetNextTutorial();
		}
		else if (isCompleted == 2) { controller.SetSkipNextTutorial(); }
	}

	public override void Exit()
	{

	}
    public override void Skip(TutorialController controller)
    {
        controller.SetNextTutorial(2);
    }
}

