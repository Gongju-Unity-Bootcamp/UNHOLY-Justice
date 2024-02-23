using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeBehaviour : StateMachineBehaviour
{
    [Header("Component")]
    private Rigidbody _rigidbody;
    private Camera _playerCamera;
    private PlayerController _playerController;

    [Header("Power")]
    private float dodgePower = 8.5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerCamera = Camera.main;
        _rigidbody = animator.GetComponent<Rigidbody>();
        _playerController = animator.GetComponent<PlayerController>();

        Vector3 rollDirection = new Vector3(animator.transform.localRotation.x, 0, animator.transform.localRotation.z);

        float horizontal = animator.GetFloat(PlayerAnimParameter.HorizontalMovement);
        float vertical = animator.GetFloat(PlayerAnimParameter.VerticalMovement);

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        moveDirection.y = 0;

        _playerController.isTargetingDodge = true;
        _playerController.isDodging = true;

        PlayerDodgeRotate(animator, _playerController.isRotate, moveDirection);
        PlayerDodge(animator, rollDirection);
    }

    private void PlayerDodgeRotate(Animator animator, bool isRotate, Vector3 moveDirection)
    {
        if (isRotate && animator.GetBool(PlayerAnimParameter.IsTargeting))
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            animator.transform.rotation = Quaternion.Euler(0, _playerCamera.transform.eulerAngles.y, 0) * targetRotation;
        }
    }

    public void PlayerDodge(Animator animator, Vector3 lastMoveDirection)
    {
         _rigidbody.velocity = lastMoveDirection + animator.transform.forward * dodgePower;

    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController.isTargetingDodge = false;
        _playerController.isDodging = false;
    }
}
