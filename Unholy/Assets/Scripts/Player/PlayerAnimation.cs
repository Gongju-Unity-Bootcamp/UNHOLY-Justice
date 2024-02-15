using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [Header("Animator")]
    private Animator _animator;

    [Header("Component")]
    private PlayerController _playerController;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationControl();
    }

    private void AnimationControl()
    {
        _animator.SetBool(PlayerAnimParameter.IsWalk, _playerController.isWalking);

        _animator.SetBool(PlayerAnimParameter.IsSprint, _playerController.isSprinting);
    }
}
