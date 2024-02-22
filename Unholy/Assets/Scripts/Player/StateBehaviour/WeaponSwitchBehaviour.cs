using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitchBehaviour : StateMachineBehaviour
{
    private PlayerController _playerController;
    [Range(0, 1)] public float StartTime = 0.4f;
    [Range(0, 1)] public float EndTime = 0.6f;

    private WeaponSwitch weaponSwitch;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController = animator.GetComponent<PlayerController>();
        weaponSwitch = animator.GetComponent<WeaponSwitch>();

        weaponSwitch.BlendIK(0.25f, animator.GetInteger(PlayerAnimParameter.WeaponType));
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (StartTime <= stateInfo.normalizedTime && stateInfo.normalizedTime <= EndTime)
        {
            Debug.Log("HI");
        }
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController.isSwitchDone = false;
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
