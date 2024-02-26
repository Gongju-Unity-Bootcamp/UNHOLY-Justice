using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool(PlayerAnimParameter.IsSprint))
            CombatManager.ConsumeStamina((float)StaminaValues.sprint * Time.deltaTime);

        if (!animator.GetBool(PlayerAnimParameter.IsSprint))
            CombatManager.RecoveryStamina((float)StaminaValues.recovery * Time.deltaTime);
    }
}
