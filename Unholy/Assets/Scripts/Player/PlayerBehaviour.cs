using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Component")]
    private Rigidbody _rigidbody;
    private PlayerController _playerController;
    private PlayerAnimation _playerAnimation;

    [Header("ETC")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private float dodgePower = 7f;



    private void Awake()
    {
        moveSpeed = walkSpeed;
        sprintSpeed = walkSpeed * 2f;

        _rigidbody = GetComponent<Rigidbody>();
        _playerController = GetComponent<PlayerController>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// Player 이동 메소드. 이동키를 입력 받으면 해당 방향으로 이동합니다.
    /// </summary>
    /// <param name="moveDirection"> Vector3 값으로 Player가 이동하고자 하는 방향이다. </param>
    public void PlayerMove(Vector3 moveDirection, bool isState = false)
    {
        //dodge animation이 동작하고 있을 때, 이동이 불가합니다.
        if (_playerAnimation.isPlayingDodgeAnimation)
            return;

        if (moveDirection != Vector3.zero)
        {
            if (isState) moveSpeed = sprintSpeed;
            else moveSpeed = walkSpeed;

            _rigidbody.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Player 회전 메소드. 입력된 이동키 방향으로 회전합니다.
    /// </summary>
    /// <param name="isRotate">player의 회전 여부를 알려주는 bool형 변수</param>
    /// <param name="horizontalMovement">player가 이동하고자 하는 방향의 x값</param>
    /// <param name="verticalMovement">player가 이동하고자 하는 방향의 y값</param>
    public void PlayerRotate(bool isRotate, float horizontalMovement, float verticalMovement)
    {
        //dodge animation이 동작하고 있을 때, 회전이 불가합니다.
        if (_playerAnimation.isPlayingDodgeAnimation)
            return;

        if (isRotate)
        {
            Vector3 targetDirection = new Vector3(horizontalMovement, 0, verticalMovement).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion interpolateRotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            _rigidbody.MoveRotation(interpolateRotation);
        }
    }

    /// <summary>
    /// Player 점프 메소드. 점프키(Space)를 누르면 player가 만큼 공중에 뜹니다.
    /// </summary>
    public void PlayerJump()
    {
        //dodge animation이 동작하고 있을 때, 점프가 불가능합니다.
        if (_playerAnimation.isPlayingDodgeAnimation)
            return;

        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpPower, _rigidbody.velocity.z);
        _playerAnimation.isPlayingJumpAnimation = false;
    }

    /// <summary>
    /// Player 회피 메소드. 회피키(Left Control)를 누르면 플레이어가 바라보는 방향대로 dodgePower만큼 이동합니다.
    /// </summary>
    /// <param name="lastMoveDirection">player가 현재 바라보는 방향입니다.</param>
    public void PlayerDodge(Vector3 lastMoveDirection)
    {
        if(_playerController.isDodging)
        {
            _rigidbody.velocity = lastMoveDirection + transform.forward * dodgePower;
        }
    }
}