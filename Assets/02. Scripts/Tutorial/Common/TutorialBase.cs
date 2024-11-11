using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialBase : MonoBehaviour
{
    /// <summary>
    /// 각 Tutorial Step Enter 되었을 때 실행하는 함수
    /// </summary>
    public abstract void Enter();

    /// <summary>
    /// 각 Tutorial Step Enter 되었을 때 실행하는 함수
    /// </summary>
    /// <param name="controller">Base가 되는 TutorialController</param>
    public abstract void Execute(TutorialController controller);
    public abstract void Skip(TutorialController controller);
    public abstract void Exit();
}
