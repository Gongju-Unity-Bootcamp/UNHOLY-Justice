using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryingBehaviour : StateMachineBehaviour
{
    [Range(0, 1)]public float StartTime = 0.5f;
    [Range(0, 1)]public float EndTime = 0.7f;

    [Range(0, 1)]public float StartTime2 = 0.5f;
    [Range(0, 1)]public float EndTime2 = 0.7f;

    MinoController _minoController;
    PlayerController _playerController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _minoController = animator.GetComponent<MinoController>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _minoController._canParrying = false;


        //  && _minoController.isParry 추가 해야함
        if ((stateInfo.normalizedTime > StartTime && stateInfo.normalizedTime < EndTime) && _playerController.isParry)
        {
            _minoController._canParrying = true;
        }

        if (stateInfo.IsTag("ComboAttack"))
        {
            if ((stateInfo.normalizedTime > StartTime2 && stateInfo.normalizedTime < EndTime2) && _playerController.isParry)
            {
                _minoController._canParrying = true;
            }
        }
    }
}
