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


    [Header("Component")]
    private PlayerBehaviour _playerBehaviour;


    [Header("Position")]
    public Vector2 direction;
    public Vector3 moveDirection;
    private float verticalMovement;
    private float horizontalMovement;


    [Header("Etc")]
    internal bool isWalking = false;
    internal bool isRotate = false;
    internal bool isSprinting = false;


    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerBehaviour = GetComponent<PlayerBehaviour>();

        _playerActionMap = _playerInput.actions.FindActionMap("Player");
        _moveAction = _playerInput.actions.FindAction("Move");
        _sprintAction = _playerInput.actions.FindAction("Sprint");

        InitializeInputSystem();
    }

    void Update()
    {
        _playerBehaviour.PlayerWalk(moveDirection);
        _playerBehaviour.PlayerRotate(isRotate, horizontalMovement, verticalMovement);
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

            isWalking = true;
            isRotate = true;
        };

        _moveAction.canceled += context =>
        {
            moveDirection = Vector3.zero;
            
            isWalking = false;
            isRotate = false;
        };
    }
}