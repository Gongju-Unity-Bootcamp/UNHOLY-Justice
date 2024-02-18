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
    /// Player �̵� �޼ҵ�. �̵�Ű�� �Է� ������ �ش� �������� �̵��մϴ�.
    /// </summary>
    /// <param name="moveDirection"> Vector3 ������ Player�� �̵��ϰ��� �ϴ� �����̴�. </param>
    public void PlayerMove(Vector3 moveDirection, bool isState = false)
    {
        //dodge animation�� �����ϰ� ���� ��, �̵��� �Ұ��մϴ�.
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
    /// Player ȸ�� �޼ҵ�. �Էµ� �̵�Ű �������� ȸ���մϴ�.
    /// </summary>
    /// <param name="isRotate">player�� ȸ�� ���θ� �˷��ִ� bool�� ����</param>
    /// <param name="horizontalMovement">player�� �̵��ϰ��� �ϴ� ������ x��</param>
    /// <param name="verticalMovement">player�� �̵��ϰ��� �ϴ� ������ y��</param>
    public void PlayerRotate(bool isRotate, float horizontalMovement, float verticalMovement)
    {
        //dodge animation�� �����ϰ� ���� ��, ȸ���� �Ұ��մϴ�.
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
    /// Player ���� �޼ҵ�. ����Ű(Space)�� ������ player�� ��ŭ ���߿� ��ϴ�.
    /// </summary>
    public void PlayerJump()
    {
        //dodge animation�� �����ϰ� ���� ��, ������ �Ұ����մϴ�.
        if (_playerAnimation.isPlayingDodgeAnimation)
            return;

        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpPower, _rigidbody.velocity.z);
        _playerAnimation.isPlayingJumpAnimation = false;
    }

    /// <summary>
    /// Player ȸ�� �޼ҵ�. ȸ��Ű(Left Control)�� ������ �÷��̾ �ٶ󺸴� ������ dodgePower��ŭ �̵��մϴ�.
    /// </summary>
    /// <param name="lastMoveDirection">player�� ���� �ٶ󺸴� �����Դϴ�.</param>
    public void PlayerDodge(Vector3 lastMoveDirection)
    {
        if(_playerController.isDodging)
        {
            _rigidbody.velocity = lastMoveDirection + transform.forward * dodgePower;
        }
    }
}