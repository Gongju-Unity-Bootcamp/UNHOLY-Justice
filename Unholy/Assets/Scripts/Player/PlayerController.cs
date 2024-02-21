using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Types;

public class PlayerController : MonoBehaviour
{
    [Header("PlayerInputSystem")]
    private PlayerInput _playerInput;
    private InputActionMap _playerActionMap;
    public InputAction _moveAction;
    public InputAction _sprintAction;
    public InputAction _jumpAction;
    public InputAction _dodgeAction;
    public InputAction _attackAction;
    public InputAction _defenseAction;
    public InputAction _unarmed;
    public InputAction _onehand;

    [Header("Component")]
    private WeaponSwitch _weaponSwitch;
    private Rigidbody _rigidbody;
    private Camera _playerCamera;
    private Animator _animator;
    private Transform _monsterObject;

    [Header("Position")]
    private Vector2 direction;
    private Vector3 moveDirection;
    [SerializeField] internal float verticalMovement;
    [SerializeField] internal float horizontalMovement;

    [Header("Speed")]
    private float moveSpeed;
    private float walkSpeed = 7.5f;
    private float sprintSpeed;
    private float rotationSpeed = 15f;

    [Header("Behaviour bool")]
    internal bool isRotate = false;
    internal bool isWalking = false;
    internal bool isSprinting = false;
    internal bool isJumping = false;
    internal bool isAir = false;
    internal bool isDodge = false;
    internal bool isDodging = false;
    internal bool isDamage = false;
    internal bool isAttack = false;
    internal bool isJumpAttack = false;
    internal bool isJumpAttacking = false;
    internal bool isAbleComboAttack = false;
    internal bool isTargeting = false;
    internal bool isTargetingDodge = false;

    internal bool isRightClick = false;


    [Header("ETC")]
    float prevAttackInputTime = 0;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _playerInput = GetComponent<PlayerInput>();
        _weaponSwitch = GetComponent<WeaponSwitch>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _playerCamera = Camera.main;

        _playerActionMap = _playerInput.actions.FindActionMap("Player");
        _moveAction = _playerInput.actions.FindAction("Move");
        _sprintAction = _playerInput.actions.FindAction("Sprint");
        _jumpAction = _playerInput.actions.FindAction("Jump");
        _dodgeAction = _playerInput.actions.FindAction("Dodge");
        _attackAction = _playerInput.actions.FindAction("Attack");
        _defenseAction = _playerInput.actions.FindAction("Defense");
        _unarmed = _playerInput.actions.FindAction("Unarmed");
        _onehand = _playerInput.actions.FindAction("OneHand");

        _monsterObject = GameObject.FindGameObjectWithTag("Monster").transform;

        moveSpeed = walkSpeed;
        sprintSpeed = walkSpeed * 2f;

        InitializeInputSystem();
    }

    void FixedUpdate()
    {
        PlayerMove(moveDirection);
        PlayerRotate(isRotate, horizontalMovement, verticalMovement);
    }

    void Update()
    {
        // 1. 입력이 됐다면 시간 갱신
        // 2. 입력이 되지 않았다면 시간
        if (isAttack && Time.time - prevAttackInputTime > 0.2f)
        {
            isAttack = false;
        }

        ControlAnimation();
    }

    private void ControlAnimation()
    {
        if (isTargeting)
        {
            _animator.SetFloat(PlayerAnimParameter.HorizontalMovement, horizontalMovement, 0.25f, Time.deltaTime);
            _animator.SetFloat(PlayerAnimParameter.VerticalMovement, verticalMovement, 0.25f, Time.deltaTime);
        }
        else
        {
            _animator.SetFloat(PlayerAnimParameter.HorizontalMovement, horizontalMovement);
            _animator.SetFloat(PlayerAnimParameter.VerticalMovement, verticalMovement);
        }

        _animator.SetBool(PlayerAnimParameter.IsAttack, isAttack);
        _animator.SetBool(PlayerAnimParameter.IsWalk, isWalking);
        _animator.SetBool(PlayerAnimParameter.IsSprint, isSprinting);
        _animator.SetBool(PlayerAnimParameter.IsAir, isAir);
        _animator.SetBool(PlayerAnimParameter.IsTargeting, isTargeting);
        _animator.SetBool(PlayerAnimParameter.IsRightClick, isRightClick);

        if (isJumping && !isAir)
            _animator.SetTrigger(PlayerAnimParameter.IsJump);

        if (isDodge)
            _animator.SetTrigger(PlayerAnimParameter.IsDodge);

        if (isDamage)
            _animator.SetTrigger(PlayerAnimParameter.IsDamage);
    }

    /// <summary>
    /// Input event binding을 하는 메소드입니다.
    /// </summary>
    private void InitializeInputSystem()
    {
        _moveAction.performed += context =>
        {
            direction = context.ReadValue<Vector2>();

            horizontalMovement = direction.x;
            verticalMovement = direction.y;

            moveDirection = new Vector3(horizontalMovement, 0, verticalMovement).normalized;
            moveDirection.y = 0;

            isRotate = true;
            isWalking = true;
        };

        _moveAction.canceled += context =>
        {
            moveDirection = Vector3.zero;

            isRotate = false;
            isWalking = false;
        };

        _sprintAction.performed += context => isSprinting = true;
        _sprintAction.canceled += context => isSprinting = false;

        _jumpAction.started += context =>
        {
            if (isAir || isDodging || isJumpAttacking) return;

            isJumping = true;
        };

        _jumpAction.canceled += context => isJumping = false;

        _dodgeAction.started += context =>
        {
            if (isAir || _animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) return;

            isDodge = true;
        };
        _dodgeAction.canceled += context =>
        {
            isDodge = false;
        };

        _attackAction.started += context =>
        {
            isAttack = true;
            prevAttackInputTime = Time.time;

            if (_weaponSwitch.IsWeaponMelee() && isAir) isJumpAttack = true;
        };

        _defenseAction.started += context => isRightClick = true;
        _defenseAction.canceled += context => isRightClick = false;

        _unarmed.started += context =>
        {
            isTargeting = false;
            _weaponSwitch.GetIndexOfWeaponTypes(WeaponType.Unarmed);
        };
        _onehand.started += context =>
        {
            isTargeting = true;
            _weaponSwitch.GetIndexOfWeaponTypes(WeaponType.OneHand);
        };
    }

    private void PlayerMove(Vector3 moveDirection)
    {
        // dodge animation이 동작하고 있을 때 이동이 불가합니다.
        if (isDodging)
            return;

        // attack 태그를 가진 애니메이션이 동작하고 있을 때 이동이 불가합니다.
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            return;

        if (moveDirection != Vector3.zero)
        {
            if (isSprinting) moveSpeed = sprintSpeed;
            else moveSpeed = walkSpeed;

            moveDirection = Quaternion.Euler(0, _playerCamera.transform.eulerAngles.y, 0) * moveDirection;

            _rigidbody.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    private void PlayerRotate(bool isRotate, float horizontal, float vertical)
    {
        // dodge animation이 동작하고 있을 때 회전이 불가합니다.
        if (isDodging)
            return;

        // attack 태그를 가진 애니메이션이 동작하고 있을 때 회전이 불가합니다.
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            return;

        // isRotate를 사용한 이유 : 사용하지 않을 경우 키보드 입력 뿐만 아니라 카메라 회전에도 캐릭터가 회전하게 됩니다.
        if ((isRotate && !isTargeting) || isSprinting)
        {
            Vector3 targetDirection = new Vector3(horizontal, 0, vertical).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion interpolateRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, _playerCamera.transform.eulerAngles.y, 0) * targetRotation, rotationSpeed * Time.deltaTime);
    
            _rigidbody.MoveRotation(interpolateRotation);
        }
        // isTargeting의 경우 항상 타겟을 주시하고 있기 때문에 isRotate가 필요하지 않습니다.
        // Slerp를 이용해 회전이 부드럽게 이루어지도록 하였습니다.
        else if (isTargeting && !isTargetingDodge)
        {
            Vector3 targetDirection = (_monsterObject.position - transform.position).normalized;

            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void ResetCondition()
    {
        isDamage = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isAir = false;
            isJumpAttack = false;
        }

        if (collision.transform.CompareTag("Monster"))
        {
            isDamage = true;
            Invoke(nameof(ResetCondition), 0.5f); // 애니메이션 테스트용 임시 코드
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isAir = true;
        }
    }
}