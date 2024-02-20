using CleverCrow.Fluid.BTs.Tasks.Actions;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

public class CustomAction : ActionBase
{
    int _animationHash;


    public CustomAction(string stateName)
    {
        _animationHash = Animator.StringToHash(stateName);
    }

    protected override void OnInit()
    {
        var animator = Owner.GetComponent<Animator>();
        animator.Play(_animationHash);
    }

    protected override TaskStatus OnUpdate()
    {
        if (CheckEnd())
        {
            Debug.Log("Success");
            return TaskStatus.Success;
        }
        else
        {
            Debug.Log("Continue");
            return TaskStatus.Continue;
        }
    }

    private bool CheckEnd()
    {
        // 여기에 MinoController의 CheckEnd() 함수 호출
        var minoController = Owner.GetComponent<MinoController>();
        if (minoController != null)
        {
            return MinoController.ck;
        }
        else
        {
            return true;
        }
    }
}