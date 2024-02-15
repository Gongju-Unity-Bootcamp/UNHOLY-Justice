using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Component")]
    private Rigidbody _rigidbody;

    [Header("ETC")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float rotationSpeed;
    private float stopRotation = 0.1f;


    private void Awake()
    {
        moveSpeed = walkSpeed;
        sprintSpeed = walkSpeed * 2f;

        _rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Player 이동 메소드. 입력키를 입력 받으면 해당 방향으로 이동함.
    /// </summary>
    /// <param name="moveDirection"> Vector3 값으로 Player가 이동하고자 하는 방향이다. </param>
    public void PlayerWalk(Vector3 moveDirection)
    {
        if (moveDirection != Vector3.zero)
        {
            moveSpeed = walkSpeed;
            _rigidbody.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Player 회전 메소드. 입력키 방향으로 부드럽게 회전한다.
    /// </summary>
    /// <param name="isRotate">player의 회전 여부를 알려주는 bool형 변수</param>
    /// <param name="horizontalMovement">player가 이동하고자 하는 방향의 x값</param>
    /// <param name="verticalMovement">player가 이동하고자 하는 방향의 y값</param>
    public void PlayerRotate(bool isRotate, float horizontalMovement, float verticalMovement)
    {
        if (isRotate)
        {
            Vector3 targetDirection = new Vector3(horizontalMovement, 0, verticalMovement).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion interpolateRotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            _rigidbody.MoveRotation(interpolateRotation);
        }
    }
}