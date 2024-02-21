using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttackBehaviour : StateMachineBehaviour
{
    [Header("Attakable Range")]
    [Range(0, 1)] public float StartTime = 0.5f;
    [Range(0, 1)] public float EndTime = 0.7f;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (StartTime <= stateInfo.normalizedTime && stateInfo.normalizedTime <= EndTime)
        {
            // Ű �Է��� ������ ��ȯ.
            if (animator.GetBool(PlayerAnimParameter.IsAttack))
            {
                animator.SetTrigger(PlayerAnimParameter.IsAbleComboAttack);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}