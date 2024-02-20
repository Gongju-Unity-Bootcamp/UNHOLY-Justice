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
    /// Animator parameter ���� �޾� animation�� ��ȯ���ִ� �޼ҵ��Դϴ�.
    /// </summary>
    private void ControlAnimation()
    {
        //���� player�� ��� �ִ� weapon�� ������ ��Ÿ���ϴ�.
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
    /// ���� ����ǰ� �ִ� animation�� ������ �� �ִ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="animationClipName">�����ϰ� ���� animation clip�� �̸��� ���ϴ�.</param>
    /// <returns></returns>
    public bool CheckAnimationPlaying(string animationClipName)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(animationClipName))
            return true;
        else return false;
    }
}
