using Data;
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

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rigidbody = animator.GetComponent<Rigidbody>();
        _playerController = animator.GetComponent<PlayerController>();

        if(animator.GetInteger(PlayerAnimParameter.WeaponType) == (int)WeaponType.OneHand)
            CombatManager.ConsumeStamina((float)StaminaValues.jump + (float)StaminaValues.onehand);
        else if(animator.GetInteger(PlayerAnimParameter.WeaponType) == (int)WeaponType.TwoHand)
            CombatManager.ConsumeStamina((float)StaminaValues.jump + (float)StaminaValues.twohand);

        _playerController.isAttacking = true;
        _playerController.isJumpAttacking = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.GetBool(PlayerAnimParameter.IsAttack))
            _rigidbody.AddForce(Vector3.down * attackPower, ForceMode.Impulse);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController.isAttacking = false;
        _playerController.isJumpAttacking = false;
    }
}
