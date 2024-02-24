using System.Collections;
using System.Collections.Generic;
using Types;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    private PlayerController _playerController;
    public string _weaponTag = default;

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        _playerController = animator.GetComponent<PlayerController>();

        animator.applyRootMotion = true;

        _playerController.isAttacking = true;
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        animator.applyRootMotion = false;

        _playerController.isAttacking = false;
    }
}
