using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedBehaviour : StateMachineBehaviour
{
    private PlayerController _playerController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController = animator.GetComponent<PlayerController>();
        animator.applyRootMotion = true;

        if (_playerController.isDead)
        {
            _playerController.isDamage = false;
            return;
        }

        if (animator.GetBool(PlayerAnimParameter.IsDefense))
        {
            CombatManager.ConsumeStamina((float)StaminaValues.defenseHit);
            Debug.Log("COUNT");
        }

        if (CombatManager._currentPlayerST <= (float)StaminaValues.defenseHit && animator.GetBool(PlayerAnimParameter.IsDefense))
        {
            _playerController.isStun = true;
            _playerController.isDefense = false;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController.isDamage = false;

        animator.applyRootMotion = false;
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        _playerController.isStun = false;
    }
}
