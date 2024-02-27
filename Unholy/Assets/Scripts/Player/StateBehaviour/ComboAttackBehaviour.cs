using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttackBehaviour : StateMachineBehaviour
{
    [Header("Attakable Range")]
    [Range(0, 1)] public float StartTime = 0.5f;
    [Range(0, 1)] public float EndTime = 0.7f;

    bool _isOneTime = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _isOneTime = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (StartTime <= stateInfo.normalizedTime && stateInfo.normalizedTime <= EndTime)
        {
            if (_isOneTime == false)
            {
                _isOneTime = true;
                SoundManager.Instance.Play(SoundType.Effect, "test-sword");
            }

            // 키 입력이 있으면 전환.
            if (animator.GetBool(PlayerAnimParameter.IsAttack))
            {
                animator.SetTrigger(PlayerAnimParameter.IsAbleComboAttack);
            }
        }
    }
}
