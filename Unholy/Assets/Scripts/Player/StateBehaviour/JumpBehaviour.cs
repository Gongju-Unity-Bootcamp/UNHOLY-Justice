using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBehaviour : StateMachineBehaviour
{
    [Header("Component")]
    private Rigidbody _rigidbody;
    private PlayerController _playerController;

    [Header("Poewr")]
    private float jumpPower = 5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rigidbody = animator.GetComponent<Rigidbody>();
        _playerController = animator.GetComponent<PlayerController>();
        
        CombatManager.ConsumeStamina((float)StaminaValues.jump);

        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpPower, _rigidbody.velocity.z);

        _playerController.isJumping = false;
    }
}
