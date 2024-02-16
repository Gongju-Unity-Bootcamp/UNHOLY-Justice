using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [Header("Component")]
    private PlayerController _playerController;
    private Animator _animator;


    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        ControlAnimation();
    }

    /// <summary>
    /// Animator parameter 값을 받아 animation을 전환해주는 메소드입니다.
    /// </summary>
    private void ControlAnimation()
    {
        _animator.SetBool(PlayerAnimParameter.IsWalk, _playerController.isWalking);
    }
}
