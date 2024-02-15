using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class testController : MonoBehaviour
{
    PlayerInput _playerInput;
    TestInputs _testInput;
    InputActionMap _playerActionMap;
    InputAction move;

    Rigidbody _rigidbody;
    CharacterController _characterController;

    [SerializeField] private Vector2 movement;
    [SerializeField] private Vector3 moveDirection;

    public float moveSpeed = 3f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _characterController = GetComponent<CharacterController>();        
    }

    private void OnEnable()
    {
        if (_testInput == null)
        {
            _testInput = new TestInputs();

            _testInput.PlayerMovement.Movement.performed += context =>
            {
                movement = context.ReadValue<Vector2>();
                moveDirection = new Vector3(movement.x, 0, movement.y);
                moveDirection.Normalize();
                moveDirection.y = 0;
            };

            _testInput.PlayerMovement.Movement.canceled += context =>
            {
                moveDirection = Vector3.zero;
            };
        }

        _testInput.Enable();
    }

    //void Start()
    //{
    //    _testInput = GetComponent<TestInputs>();

    //    _testInput.PlayerMovement.Movement.performed += i => movement = i.ReadValue<Vector2>();

    //    //_playerActionMap = _testInput.actions.FindActionMap("PlayerMovement");
    //}

    void Update()
    {
        PlayerMoveCC();
        //PlayerMoveRB();
    }

    public void PlayerMoveRB()
    {
        //_rigidbody.velocity = inputDir * moveSpeed * Time.deltaTime;
        //transform.LookAt(transform.position + inputDir);
        _rigidbody.MovePosition(transform.position + moveDirection * Time.deltaTime * moveSpeed);
    }

    public void PlayerMoveCC()
    {
        _characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        Debug.Log(moveDirection);
    }
}
