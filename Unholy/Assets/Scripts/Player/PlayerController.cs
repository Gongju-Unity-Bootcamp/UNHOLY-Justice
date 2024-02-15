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

    [Header("Component")]
    private Rigidbody _rigidbody;

    [Header("Position")]
    public Vector2 direction;
    public Vector3 moveDirection;
    private float verticalMovement;
    private float horizontalMovement;

    [Header("Etc")]
    [SerializeField] private float moveSpeed;



    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();

        _playerActionMap = _playerInput.actions.FindActionMap("Player");
        _moveAction = _playerInput.actions.FindAction("Move");


        _moveAction.performed += context =>
        {
            direction = context.ReadValue<Vector2>();

            horizontalMovement = direction.x;
            verticalMovement = direction.y;

            moveDirection = new Vector3(horizontalMovement, 0, verticalMovement).normalized;
            moveDirection.y = 0;
        };

        _moveAction.canceled += context =>
        {
            moveDirection = Vector3.zero;
        };
    }

    void Update()
    {
        PlayerMove();
    }

    public void PlayerMove()
    {
        _rigidbody.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);
    }
}
