using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;
using UnityEditor.Overlays;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [Header("Component")]
    private PlayerController _playerController;
    private PlayerBehaviour _playerBehaviour;
    private WeaponSwitch _weaponSwitch;
    private Animator _animator;

    [Header("Check Animation Clips")]
    [SerializeField] internal bool isPlayingDodgeAnimation = false;
    [SerializeField] internal bool isPlayingAttackAnimation = false;
    [SerializeField] internal bool isPlayingJumpAttackAnimation = false;
    [SerializeField] internal bool isPlayingFallAnimation = false;


    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerBehaviour = GetComponent<PlayerBehaviour>();
        _weaponSwitch = GetComponent<WeaponSwitch>();   
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        ControlAnimation();

        isPlayingDodgeAnimation = CheckAnimationPlaying("Dodge");
        isPlayingFallAnimation = CheckAnimationPlaying("P_OneHand_Fall");
        isPlayingJumpAttackAnimation = CheckAnimationPlaying("P_OneHand_Jump-Attack");
        isPlayingAttackAnimation = CheckAnimationPlaying("P_OneHand_Attack1") || CheckAnimationPlaying("P_OneHand_Attack2") || CheckAnimationPlaying("P_OneHand_Attack3");
    }

    /// <summary>
    /// Animator parameter 값을 받아 animation을 전환해주는 메소드입니다.
    /// </summary>
    private void ControlAnimation()
    {
        //현재 player가 들고 있는 weapon의 종류를 나타냅니다.
        _animator.SetInteger(PlayerAnimParameter.WeaponType, _weaponSwitch.weaponIndex);

        _animator.SetBool(PlayerAnimParameter.AbleToJump, _playerBehaviour.ableToJump);
        _animator.SetBool(PlayerAnimParameter.AbleToDodge, _playerBehaviour.ableToDodge);
        _animator.SetBool(PlayerAnimParameter.IsWalk, _playerController.isWalking);
        _animator.SetBool(PlayerAnimParameter.IsSprint, _playerController.isSprinting);
        _animator.SetBool(PlayerAnimParameter.IsAir, _playerController.isAir);

        if (!_playerBehaviour.ableToJump)
            _animator.ResetTrigger(PlayerAnimParameter.IsJump);

        if (_playerController.isJumping && !_playerController.isAir)
            _animator.SetTrigger(PlayerAnimParameter.IsJump);

        if (_playerController.isDodging && !isPlayingDodgeAnimation)
        {
            _animator.SetTrigger(PlayerAnimParameter.IsDodge);
            _playerController.isDodging = false;
        }

        if (_playerController.isDamage)
            _animator.SetTrigger(PlayerAnimParameter.IsDamage);

        if (_playerController.isAttack)
        {
            _animator.SetTrigger(PlayerAnimParameter.IsAttack);
            _playerController.isAttack = false;
        }

        if (_playerController.isAbleComboAttack)
        {
            _animator.SetTrigger(PlayerAnimParameter.IsAbleComboAttack);
            _playerController.isAbleComboAttack = false;
        }
    }


    /// <summary>
    /// 현재 재생되고 있는 animation을 감시할 수 있는 메소드입니다.
    /// </summary>
    /// <param name="animationClipName">감시하고 싶은 animation clip의 이름을 씁니다.</param>
    /// <returns></returns>
    public bool CheckAnimationPlaying(string animationClipName)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(animationClipName))
            return true;
        else return false;
    }
}
