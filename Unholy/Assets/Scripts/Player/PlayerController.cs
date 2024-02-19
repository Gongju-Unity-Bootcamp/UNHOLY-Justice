using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("PlayerInputSystem")]
    private PlayerInput _playerInput;
    private InputActionMap _playerActionMap;
    public InputAction _moveAction;
    public InputAction _sprintAction;
    public InputAction _jumpAction;
    public InputAction _dodgeAction;

    [Header("Component")]
    private PlayerBehaviour _playerBehaviour;
    private PlayerAnimation _playerAnimation;

    [Header("Position")]
    public Vector2 direction;
    public Vector3 moveDirection;
    private Vector3 rollDirection;
    private float verticalMovement;
    private float horizontalMovement;
    

    [Header("Behaviour bool")]
    private bool isRotate = false;
    internal bool isWalking = false;
    internal bool isSprinting = false;
    internal bool isJumping = false;
    internal bool isAir = false;
    internal bool isDodging = false;
    internal bool isDamage = false;



    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerBehaviour = GetComponent<PlayerBehaviour>();
        _playerAnimation = GetComponent<PlayerAnimation>();

        _playerActionMap = _playerInput.actions.FindActionMap("Player");
        _moveAction = _playerInput.actions.FindAction("Move");
        _sprintAction = _playerInput.actions.FindAction("Sprint");
        _jumpAction = _playerInput.actions.FindAction("Jump");
        _dodgeAction = _playerInput.actions.FindAction("Dodge");

        rollDirection = transform.forward;

        _playerBehaviour.ableToJump = true;
        _playerBehaviour.ableToDodge = true;

        InitializeInputSystem();
    }


    void Update()
    {
        _playerBehaviour.PlayerMove(moveDirection, isSprinting);
        _playerBehaviour.PlayerRotate(isRotate, horizontalMovement, verticalMovement);
        // _playerBehaviour.PlayerDodge(rollDirection);
    }


    private void InitializeInputSystem()
    {
        _moveAction.performed += context =>
        {
            direction = context.ReadValue<Vector2>();

            horizontalMovement = direction.x;
            verticalMovement = direction.y;

            moveDirection = new Vector3(horizontalMovement, 0, verticalMovement).normalized;
            moveDirection.y = 0;

            rollDirection = new Vector3(transform.localRotation.x, 0, transform.localRotation.z);

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
            if (isAir)
                return;

            isJumping = true;
            _playerBehaviour.PlayerJump();
        };

        _jumpAction.canceled += context => isJumping = false;

        _dodgeAction.started += context =>
        {
            if (isAir || !_playerBehaviour.ableToDodge) return;
            
            isDodging = true;
            _playerBehaviour.StopCoroutine(_playerBehaviour.PlayerDodge(rollDirection));
            _playerBehaviour.StartCoroutine(_playerBehaviour.PlayerDodge(rollDirection));
        };
        _dodgeAction.canceled += context => isDodging = false;

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
            _playerAnimation.isPlayingJumpAnimation = true;
        }
    }
}