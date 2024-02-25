using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedBehaviour : StateMachineBehaviour
{
    private PlayerController _playerController;

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        _playerController = animator.GetComponent<PlayerController>();

        _playerController.isDamage = false;

        animator.applyRootMotion = true;
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        animator.applyRootMotion = false;
    }
}
