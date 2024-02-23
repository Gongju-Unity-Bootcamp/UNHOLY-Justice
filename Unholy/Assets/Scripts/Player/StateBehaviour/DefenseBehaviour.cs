using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBehaviour : StateMachineBehaviour
{
    private PlayerController _playerController;

    public float endTime = 0.2f;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController = animator.GetComponent<PlayerController>();
        _playerController.isParry = true;
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= endTime) _playerController.isParry = false;

        animator.SetBool(PlayerAnimParameter.IsParry, _playerController.isParry);
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController.isParry = false;
    }
}
