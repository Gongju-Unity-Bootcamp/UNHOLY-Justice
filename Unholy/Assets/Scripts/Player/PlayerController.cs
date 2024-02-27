using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Data;

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
    public InputAction _twohand;

    [Header("Component")]
    public Transform _monsterObject;
    private WeaponSwitch _weaponSwitch;
    private Rigidbody _rigidbody;
    private Camera _playerCamera;
    private Animator _animator;

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
    internal bool isAttacking = false;
    internal bool isJumpAttack = false;
    internal bool isJumpAttacking = false;
    internal bool isAbleComboAttack = false;
    internal bool isTargeting = false;
    internal bool isTargetingDodge = false;
    internal bool isDefense = false;
    internal bool weaponSwitch = false;
    internal bool isSwitchDone = false;
    internal bool isParry = false;
    internal bool isStun = false;
    internal bool isDead = false;

    [Header("Const")]
    private const float DAMPTIME = 0.25f;

    [Header("ETC")]
    [SerializeField] private GameObject _oneHandCol;
    [SerializeField] private GameObject _twoHandCol;
    public GameObject _parrySpark;
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
        _twohand = _playerInput.actions.FindAction("TwoHand");

        moveSpeed = walkSpeed;
        sprintSpeed = walkSpeed * 2f;

        _oneHandCol.SetActive(false);
        _twoHandCol.SetActive(false);
        
        isDodging = false;

        InitializeInputSystem();
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            isDodging = true;
            return;
        }

        if (!isDamage && !isStun)
        {
            PlayerMove(moveDirection);
            PlayerRotate(isRotate, horizontalMovement, verticalMovement);
        }

        if (CombatManager._currentPlayerST <= 0)
            isSprinting = false;

        if (CombatManager._currentPlayerHP <= 0)
            isDead = true;
    }

    void Update()
    {
        if (CombatManager._checkParrying)
        {
            PlayerParrying();
        }

        if (isAttack && Time.time - prevAttackInputTime > 0.2f)
        {
            isAttack = false;
        }

        CombatManager.CheckTime(isDefense);

        if (!isTargeting) isTargeting = CombatManager._isIdle;

        ControlAnimation();
    }

    private void ControlAnimation()
    {
        _animator.SetFloat(PlayerAnimParameter.PlayerHP, CombatManager._currentPlayerHP);
        _animator.SetFloat(PlayerAnimParameter.PlayerST, CombatManager._currentPlayerST);

        if (isTargeting)
        {
            _animator.SetFloat(PlayerAnimParameter.HorizontalMovement, horizontalMovement, DAMPTIME, Time.deltaTime);
            _animator.SetFloat(PlayerAnimParameter.VerticalMovement, verticalMovement, DAMPTIME, Time.deltaTime);
        }
        else
        {
            _animator.SetFloat(PlayerAnimParameter.HorizontalMovement, horizontalMovement);
            _animator.SetFloat(PlayerAnimParameter.VerticalMovement, verticalMovement);
        }

        _animator.SetBool(PlayerAnimParameter.IsStun, isStun);
        _animator.SetBool(PlayerAnimParameter.IsDead, isDead);

        if(!isDead) 
            _animator.SetBool(PlayerAnimParameter.IsDamage, isDamage);

        if (!isDamage || !isDead)
        {
            if (isJumping && !isAir)
                _animator.SetTrigger(PlayerAnimParameter.IsJump);

            if (isDodge)
                _animator.SetTrigger(PlayerAnimParameter.IsDodge);

            if(!isStun)
                _animator.SetBool(PlayerAnimParameter.IsDefense, isDefense);

            _animator.SetBool(PlayerAnimParameter.IsAttack, isAttack);
            _animator.SetBool(PlayerAnimParameter.IsAttacking, isAttacking);
            _animator.SetBool(PlayerAnimParameter.IsWalk, isWalking);
            _animator.SetBool(PlayerAnimParameter.IsSprint, isSprinting);
            _animator.SetBool(PlayerAnimParameter.IsAir, isAir);
            _animator.SetBool(PlayerAnimParameter.IsTargeting, isTargeting);
            _animator.SetBool(PlayerAnimParameter.IsSwitchDone, isSwitchDone);
            _animator.SetBool(PlayerAnimParameter.WeaponSwitch, weaponSwitch);
        }
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

        _defenseAction.started += context => isDefense = true;
        _defenseAction.canceled += context => isDefense = false;

        _unarmed.started += context =>
        {
            if (CombatManager._isIdle || isDefense || weaponSwitch || isSwitchDone || isDodging || _weaponSwitch.weaponIndex == (int)WeaponType.Unarmed) return;

            _weaponSwitch.GetIndexOfWeaponTypes(WeaponType.Unarmed);
            weaponSwitch = true;
            isSwitchDone = true;
        };
        _unarmed.canceled += context => weaponSwitch = false;

        _onehand.started += context =>
        {
            if (isDefense || weaponSwitch || isSwitchDone || isDodging || _weaponSwitch.weaponIndex == (int)WeaponType.OneHand) return;

            _weaponSwitch.GetIndexOfWeaponTypes(WeaponType.OneHand);
            weaponSwitch = true;
            isSwitchDone = true;
        };
        _onehand.canceled += context => weaponSwitch = false;

        _twohand.started += context =>
        {
            if (isDefense || weaponSwitch || isSwitchDone || isDodging || _weaponSwitch.weaponIndex == (int)WeaponType.TwoHand) return;

            _weaponSwitch.GetIndexOfWeaponTypes(WeaponType.TwoHand);
            weaponSwitch = true;
            isSwitchDone = true;
        };
        _twohand.canceled += context => weaponSwitch = false;
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

    void PlayerParrying()
    {
        Vector3 _tempPos = transform.position + transform.forward + (transform.up * 2f);
        Instantiate(_parrySpark, _tempPos, Quaternion.identity);

        CombatManager.RecoveryStamina((float)StaminaValues.parry);
    }

    /// <summary>
    /// 공격 범위 콜라이더를 활성화 시키는 메소드입니다.
    /// 애니메이션 이벤트에서 호출되며, float 값을 받아 콜라이더 종료 시간을 설정합니다.
    /// </summary>
    /// <param name="time"></param>
    public void ActivateCollider(float time)
    {
        if (_weaponSwitch.weaponIndex == (int)WeaponType.OneHand)
        {
            PlayerAttackCollision._disableTime = time;
            _oneHandCol.SetActive(true);
        }
        else if (_weaponSwitch.weaponIndex == (int)WeaponType.TwoHand)
        {
            PlayerAttackCollision._disableTime = time;
            _twoHandCol.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isAir = false;
            isJumpAttack = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isAir = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            float damage = _weaponSwitch.weaponDamage;
            if (isJumpAttacking) damage *= 1.2f;

            CombatManager.TakeDamage(gameObject.tag, damage);
        }
    }

    public void PlayerSound(string name)
    {
        SoundManager.Instance.Play(SoundType.Effect, name);
    }
}