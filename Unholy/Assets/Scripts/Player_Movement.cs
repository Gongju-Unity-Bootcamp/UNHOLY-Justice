using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player_Movement : MonoBehaviour
{
    [Header("Keybinds")]
    private PlayerInput playerInput;
    private InputActionMap playerActionMap;
    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction jumpAction;

    [Header("Speeds")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("ETC")]
    private Rigidbody rb;
    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        playerActionMap = playerInput.actions.FindActionMap("Player");
        moveAction = playerActionMap.FindAction("Move");
        sprintAction = playerActionMap.FindAction("Sprint");
        jumpAction = playerActionMap.FindAction("Jump");

        moveAction.performed += ctx => {
            Vector2 dir = ctx.ReadValue<Vector2>();
            moveDirection = new Vector3(dir.x, 0, dir.y);
        };

        moveAction.canceled += ctx => {
            moveDirection = Vector3.zero;
        };
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (moveDirection != Vector3.zero)
        {
            Vector3 worldMoveDirection = transform.TransformDirection(moveDirection);
            Vector3 flatMoveDirection = worldMoveDirection;
            flatMoveDirection.y = 0;

            rb.AddForce(flatMoveDirection * moveSpeed * 10f, ForceMode.Force);
        }
    }
}
