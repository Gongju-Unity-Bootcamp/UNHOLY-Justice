using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [Header("Component")]
    private PlayerController _playerController;
    private PlayerBehaviour _playerBehaviour;
    private Animator _animator;

    [Header("Check Animation Clips")]
    [SerializeField] internal bool isPlayingLandAnimation = false;
    [SerializeField] internal bool isPlayingJumpAnimation = false;


    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerBehaviour = GetComponent<PlayerBehaviour>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        ControlAnimation();
    }

    /// <summary>
    /// Animator parameter ���� �޾� animation�� ��ȯ���ִ� �޼ҵ��Դϴ�.
    /// </summary>
    private void ControlAnimation()
    {
        _animator.SetBool(PlayerAnimParameter.IsWalk, _playerController.isWalking);
        _animator.SetBool(PlayerAnimParameter.IsSprint, _playerController.isSprinting);
        _animator.SetBool(PlayerAnimParameter.IsAir, _playerController.isAir);


        if (_playerController.isJumping && !_playerController.isAir) _animator.SetTrigger(PlayerAnimParameter.IsJump);
        _animator.SetBool(PlayerAnimParameter.IsPlayingJumpAnimation, isPlayingJumpAnimation);
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
