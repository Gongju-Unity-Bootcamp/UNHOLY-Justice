using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Types;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Component")]
    private Rigidbody _rigidbody;
    private PlayerController _playerController;
    private PlayerAnimation _playerAnimation;
    private Transform _playerCamera;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private float dodgePower = 7f;
    [SerializeField] private float attackPower = 120f;

    [Header("Possibility")]
    internal bool ableToJump = false;
    internal bool ableToDodge = false;

    [Header("ETC")]
    private const float DELAYTIME = 1.15f;


    private void Awake()
    {
        moveSpeed = walkSpeed;
        sprintSpeed = walkSpeed * 2f;

        _rigidbody = GetComponent<Rigidbody>();
        _playerController = GetComponent<PlayerController>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerCamera = Camera.main.transform;
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
        //dodge animation�� �����ϰ� ���� �� �̵��� �Ұ��մϴ�.
        if (_playerAnimation.isPlayingDodgeAnimation)
            return;

        //attack animation�� �����ϰ� ���� �� �̵��� �Ұ��մϴ�.
        if (_playerAnimation.isPlayingAttackAnimation)
            return;

        if (_playerAnimation.isPlayingJumpAttackAnimation)
            return;

        if (moveDirection != Vector3.zero)
        {
            if (isState) moveSpeed = sprintSpeed;
            else moveSpeed = walkSpeed;

            moveDirection = Quaternion.Euler(0, _playerCamera.eulerAngles.y, 0) * moveDirection;

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
        //dodge animation�� �����ϰ� ���� �� ȸ���� �Ұ��մϴ�.
        if (_playerAnimation.isPlayingDodgeAnimation)
            return;

        //attack animation�� �����ϰ� ���� �� ȸ���� �Ұ��մϴ�.
        if (_playerAnimation.isPlayingAttackAnimation)
            return;

        if (_playerAnimation.isPlayingJumpAttackAnimation)
            return;

        if (isRotate)
        {
            Vector3 targetDirection = new Vector3(horizontalMovement, 0, verticalMovement).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion interpolateRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, _playerCamera.eulerAngles.y, 0) * targetRotation, rotationSpeed * Time.deltaTime);

            _rigidbody.MoveRotation(interpolateRotation);
        }
    }

    /// <summary>
    /// Player ���� �޼ҵ�. ����Ű(Space)�� ������ player�� ��ŭ ���߿� ��ϴ�.
    /// </summary>
    public void PlayerJump()
    {
        // ableToJump : ���, �̵� �� Ȥ�� ȸ�� ���� ���� ������ �ð��� ������ �� true�� �˴ϴ�.
        // ableToJump�� true�� ��쿡�� ������ �����մϴ�.
        if (!ableToJump)
            return;

        if (_playerAnimation.isPlayingJumpAttackAnimation)
            return;

        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpPower, _rigidbody.velocity.z);
    }

    public void PlayerJumpAttack()
    {
        if (_playerController.isJumpAttack && _playerAnimation.isPlayingFallAnimation)
        {
            _rigidbody.AddForce(Vector3.down * attackPower, ForceMode.Impulse);
            
            ableToJump = false;

            CancelInvoke();
            Invoke(nameof(ChangeJumpState), DELAYTIME);
        }
    }

    private void ChangeJumpState()
    {
        ableToJump = true;
    }

    /// <summary>
    /// Player ȸ�� �޼ҵ�. ȸ��Ű(Left Control)�� ������ �÷��̾ �ٶ󺸴� ������ dodgePower��ŭ �̵��մϴ�.
    /// </summary>
    /// <param name="lastMoveDirection">player�� ���� �ٶ󺸴� �����Դϴ�.</param>
    public IEnumerator PlayerDodge(Vector3 lastMoveDirection)
    {
        // ableToDodge : ���, �̵� �� Ȥ�� ȸ�� ���� ���� ������ �ð��� ������ �� true�� �˴ϴ�.
        // ableToDodge true�� ��쿡�� ȸ�ǰ� �����մϴ�.
        if (_playerController.isDodging && ableToDodge)
        {
            _rigidbody.velocity = lastMoveDirection + transform.forward * dodgePower;

            ableToJump = false;
            ableToDodge = false;
        }
        yield return new WaitForSeconds(DELAYTIME);

        if(!ableToJump && !ableToDodge)
        {
            ableToJump = true;
            ableToDodge = true;
        }
    }
}