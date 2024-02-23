using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitchBehaviour : StateMachineBehaviour
{
    [Header("Component")]
    private PlayerController _playerController;
    private WeaponSwitch _weaponSwitch;

    [Header("Range")]
    [Range(0, 1)] public float StartTime = 0.7f;
    [Range(0, 1)] public float EndTime = 0.9f;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController = animator.GetComponent<PlayerController>();
        _weaponSwitch = animator.GetComponent<WeaponSwitch>();

        _weaponSwitch.BlendIK(0.25f, animator.GetInteger(PlayerAnimParameter.WeaponType));
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (StartTime <= stateInfo.normalizedTime && stateInfo.normalizedTime <= EndTime)
        {
            SelectWeapon(animator);
        }
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController.isSwitchDone = false;
    }

    private void SelectWeapon(Animator animator)
    {
        if (animator.GetInteger(PlayerAnimParameter.WeaponType) == 1)
        {
            _weaponSwitch._prefabs[0].SetActive(true);
            _weaponSwitch._prefabs[1].SetActive(true);
            _weaponSwitch._prefabs[2].SetActive(true);

            _weaponSwitch._weapons[0].SetActive(false);
            _weaponSwitch._weapons[1].SetActive(false);
            _weaponSwitch._weapons[2].SetActive(false);
        }
        else if (animator.GetInteger(PlayerAnimParameter.WeaponType) == 2)
        {
            _weaponSwitch._prefabs[0].SetActive(false);
            _weaponSwitch._prefabs[1].SetActive(false);
            _weaponSwitch._prefabs[2].SetActive(true);

            _weaponSwitch._weapons[0].SetActive(true);
            _weaponSwitch._weapons[1].SetActive(true);
            _weaponSwitch._weapons[2].SetActive(false);
        }
        else if (animator.GetInteger(PlayerAnimParameter.WeaponType) == 3)
        {
            _weaponSwitch._prefabs[0].SetActive(true);
            _weaponSwitch._prefabs[1].SetActive(true);
            _weaponSwitch._prefabs[2].SetActive(false);

            _weaponSwitch._weapons[0].SetActive(false);
            _weaponSwitch._weapons[1].SetActive(false);
            _weaponSwitch._weapons[2].SetActive(true);
        }
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
