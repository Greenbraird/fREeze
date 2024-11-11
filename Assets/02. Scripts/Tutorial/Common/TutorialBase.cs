using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialBase : MonoBehaviour
{
    /// <summary>
    /// �� Tutorial Step Enter �Ǿ��� �� �����ϴ� �Լ�
    /// </summary>
    public abstract void Enter();

    /// <summary>
    /// �� Tutorial Step Enter �Ǿ��� �� �����ϴ� �Լ�
    /// </summary>
    /// <param name="controller">Base�� �Ǵ� TutorialController</param>
    public abstract void Execute(TutorialController controller);
    public abstract void Skip(TutorialController controller);
    public abstract void Exit();
}
