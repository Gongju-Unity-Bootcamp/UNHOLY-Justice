using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParryingBehaviour : StateMachineBehaviour
{
    [Range(0, 1)]public float StartTime = 0f;
    [Range(0, 1)]public float EndTime = 0f;

    [Range(0, 1)]public float StartTime2 = 0f;
    [Range(0, 1)]public float EndTime2 = 0f;

    [Range(0, 1)]public float StartCol = 0f;
    [Range(0, 1)]public float EndCol = 0f;

    [Range(0, 1)]public float StartCol2 = 0f;
    [Range(0, 1)]public float EndCol2 = 0f;

    [Range(0, 1)]public float StartCol3 = 0f;
    [Range(0, 1)]public float EndCol3 = 0f;
    MinoCollider _minoCollider;
    PlayerController _playerController;
    

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _minoCollider = FindObjectOfType<MinoCollider>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CombatManager._canParrying = false;
        _minoCollider._canAttack = false;

        if ((stateInfo.normalizedTime >= StartCol && stateInfo.normalizedTime <= EndCol) || (stateInfo.normalizedTime >= StartCol2 && stateInfo.normalizedTime <= EndCol2) || (stateInfo.normalizedTime >= StartCol3 && stateInfo.normalizedTime <= EndCol3))
        {
            if (_playerController.isParry == false)
            {
                _minoCollider._canAttack = true;
            }
        }

        if ((stateInfo.normalizedTime > StartTime && stateInfo.normalizedTime < EndTime) && _playerController.isParry)
        {
            CombatManager._canParrying = true;
            _minoCollider._canAttack = false;
        }

        if (stateInfo.IsTag("ComboAttack"))
        {
            if ((stateInfo.normalizedTime > StartTime2 && stateInfo.normalizedTime < EndTime2) && _playerController.isParry)
            {
                CombatManager._canParrying = true;
                _minoCollider._canAttack = false;
            }
        }
    }
}
