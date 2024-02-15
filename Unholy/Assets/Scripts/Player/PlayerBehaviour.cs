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
    /// Player �̵� �޼ҵ�. �Է�Ű�� �Է� ������ �ش� �������� �̵���.
    /// </summary>
    /// <param name="moveDirection"> Vector3 ������ Player�� �̵��ϰ��� �ϴ� �����̴�. </param>
    public void PlayerWalk(Vector3 moveDirection)
    {
        if (moveDirection != Vector3.zero)
        {
            moveSpeed = walkSpeed;
            _rigidbody.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Player ȸ�� �޼ҵ�. �Է�Ű �������� �ε巴�� ȸ���Ѵ�.
    /// </summary>
    /// <param name="isRotate">player�� ȸ�� ���θ� �˷��ִ� bool�� ����</param>
    /// <param name="horizontalMovement">player�� �̵��ϰ��� �ϴ� ������ x��</param>
    /// <param name="verticalMovement">player�� �̵��ϰ��� �ϴ� ������ y��</param>
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