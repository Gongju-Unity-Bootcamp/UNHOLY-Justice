using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JumpAttackBehaviour : StateMachineBehaviour
{
    [Header("Component")]
    private Rigidbody _rigidbody;
    private PlayerController _playerController;

    [Header("Power")]
    private float attackPower = 5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rigidbody = animator.GetComponent<Rigidbody>();
        _playerController = animator.GetComponent<PlayerController>();

        _playerController.isAttacking = true;
        _playerController.isJumpAttacking = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.GetBool(PlayerAnimParameter.IsAttack))
            _rigidbody.AddForce(Vector3.down * attackPower, ForceMode.Impulse);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController.isAttacking = false;
        _playerController.isJumpAttacking = false;
    }
}
