using CleverCrow.Fluid.BTs.Tasks.Actions;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

public class CustomAction : ActionBase
{
    int _animationHash;
    private bool _isEnter;
    Animator _animator;

    public CustomAction(string stateName)
    {
        _animationHash = Animator.StringToHash(stateName);
    }

    protected override void OnInit()
    {
        _animator = Owner.GetComponent<Animator>();
    }


    protected override TaskStatus OnUpdate()
    {
        if (_isEnter == false)
        {
            _isEnter = true;
            _animator.Play(_animationHash, 0, 0);

            return TaskStatus.Continue;
        }

        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            _isEnter = false;

            return TaskStatus.Success;
        }

        return TaskStatus.Continue;
    }
}