using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Component")]
    private Rigidbody _rigidbody;
    private PlayerController _playerController;
    private PlayerAnimation _playerAnimation;
    private Transform _playerCamera;
    public GameObject monsterObject;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private float dodgePower = 8.5f;
    [SerializeField] private float attackPower = 120f;

    [Header("Possibility")]
    internal bool ableToJump = false;
    internal bool ableToDodge = false;
    internal bool ableToCombo = false;
    internal bool isTargeting = false;

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
    /// Player 이동 메소드. 이동키를 입력 받으면 해당 방향으로 이동합니다.
    /// </summary>
    /// <param name="moveDirection"> Vector3 값으로 Player가 이동하고자 하는 방향이다. </param>
    public void PlayerMove(Vector3 moveDirection, bool isState = false)
    {
        //dodge animation이 동작하고 있을 때 이동이 불가합니다.
        if (_playerAnimation.isPlayingDodgeAnimation)
            return;

        //attack animation이 동작하고 있을 때 이동이 불가합니다.
        if (_playerAnimation.isPlayingAttackAnimation)
            return;

        // jump attack animation이 동작하고 있을 때 이동이 불가합니다.
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
    /// Player 회전 메소드. 입력된 이동키 방향으로 회전합니다.
    /// </summary>
    /// <param name="isRotate">player의 회전 여부를 알려주는 bool형 변수</param>
    /// <param name="horizontalMovement">player가 이동하고자 하는 방향의 x값</param>
    /// <param name="verticalMovement">player가 이동하고자 하는 방향의 y값</param>
    public void PlayerRotate(bool isRotate, float horizontalMovement, float verticalMovement)
    {
        // dodge animation이 동작하고 있을 때 회전이 불가합니다.
        if (_playerAnimation.isPlayingDodgeAnimation)
            return;

        // attack animation이 동작하고 있을 때 회전이 불가합니다.
        if (_playerAnimation.isPlayingAttackAnimation)
            return;

        // jump attack animation이 동작하고 있을 때 회전이 불가합니다.
        if (_playerAnimation.isPlayingJumpAttackAnimation)
            return;

        // 회피 불가능 상태일 때 회전이 불가합니다.
        if (!ableToDodge)
            return;

        // isRotate를 사용한 이유 : 사용하지 않을 경우 키보드 입력 뿐만 아니라 카메라 회전에도 캐릭터가 회전하게 됩니다.
        if ((isRotate && !isTargeting) || (_playerController.isSprinting && _playerController.isWalking))
        {
            Vector3 targetDirection = new Vector3(horizontalMovement, 0, verticalMovement).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion interpolateRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, _playerCamera.eulerAngles.y, 0) * targetRotation, rotationSpeed * Time.deltaTime);

            _rigidbody.MoveRotation(interpolateRotation);
        }
        // isTargeting의 경우 항상 타겟을 주시하고 있기 때문에 isRotate가 필요하지 않습니다.
        // Slerp를 이용해 회전이 부드럽게 이루어지도록 하였습니다.
        else if(isTargeting)
        {
            Vector3 targetDirection = (monsterObject.transform.position - transform.position).normalized;

            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// 타겟팅 중 회피 시 입력 중인 이동 값에 따라 회전 값을 변경하는 메소드입니다.
    /// </summary>
    /// <param name="isRotate"></param>
    /// <param name="moveDirection"></param>
    public void PlayerDodgeRotate(bool isRotate, Vector3 moveDirection)
    {
        // dodge animation이 동작하고 있을 때 회전이 불가합니다.
        if (_playerAnimation.isPlayingDodgeAnimation)
            return;

        // attack animation이 동작하고 있을 때 회전이 불가합니다.
        if (_playerAnimation.isPlayingAttackAnimation)
            return;

        // jump attack animation이 동작하고 있을 때 회전이 불가합니다.
        if (_playerAnimation.isPlayingJumpAttackAnimation)
            return;

        // 키보드 입력 중(isRotate)이고 타겟팅이 활성화(isTargeting) 되었을 때만 실행됩니다.
        // 입력 중인 방향 값에 따라 회전 값을 변경합니다.
        if (isRotate && isTargeting)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Euler(0, _playerCamera.eulerAngles.y, 0) * targetRotation;
        }
    }

    /// <summary>
    /// Player 점프 메소드. 점프키(Space)를 누르면 player가 만큼 공중에 뜹니다.
    /// </summary>
    public void PlayerJump()
    {
        // ableToJump : 평소, 이동 중 혹은 회피 동작 이후 일정한 시간이 지났을 때 true가 됩니다.
        // ableToJump가 true일 경우에만 점프가 가능합니다.
        if (!ableToJump)
            return;

        // jump attack animation이 동작하고 있을 때 점프가 불가합니다.
        if (_playerAnimation.isPlayingJumpAttackAnimation)
            return;

        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpPower, _rigidbody.velocity.z);
    }

    /// <summary>
    /// 점프 공격 기능을 구현한 메소드 입니다.
    /// </summary>
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

    // 점프 가능 상태를 초기화하는 메소드로 Invoke와 함께 사용합니다.
    private void ChangeJumpState()
    {
        ableToJump = true;
    }

    // 연속 공격 타이밍 구현을 위한 애니메이션용 이벤트입니다.
    public void AbleToCombo()
    {
        ableToCombo = true;
    }

    // 연속 공격 타이밍 구현을 위한 애니메이션용 이벤트입니다.
    public void NotAbleToCombo()
    {
        ableToCombo = false;
    }

    /// <summary>
    /// Player 회피 메소드. 회피키(Left Control)를 누르면 플레이어가 바라보는 방향대로 dodgePower만큼 이동합니다.
    /// </summary>
    /// <param name="lastMoveDirection">player가 현재 바라보는 방향입니다.</param>
    public IEnumerator PlayerDodge(Vector3 lastMoveDirection)
    {
        // ableToDodge : 평소, 이동 중 혹은 회피 동작 이후 일정한 시간이 지났을 때 true가 됩니다.
        // ableToDodge true일 경우에만 회피가 가능합니다.
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