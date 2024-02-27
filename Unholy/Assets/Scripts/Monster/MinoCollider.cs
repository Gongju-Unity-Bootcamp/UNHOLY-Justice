using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinoCollider : MonoBehaviour
{
    internal bool _canAttack = false;

    PlayerController _playerController;

    [Header("Prefab")]
    public GameObject _bloodPrefab;

    Vector3 spawnPosition;
    Quaternion spawnRotation;


    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (_canAttack && _enterCol && !CombatManager._canParrying)
        {
            AttackSuccess();
            _enterCol = false;
        }
    }

    bool _enterCol = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _enterCol = true;

            var collisionPoint = other.ClosestPoint(transform.position);
            var surfaceNormal = other.ClosestPointOnBounds(transform.position) - transform.position;
            spawnPosition = collisionPoint + surfaceNormal;
            spawnRotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _enterCol = false;
        }
    }

    private void AttackSuccess()
    {
        if (_playerController.isDodging) return;

        if(!_playerController.isDefense)
        {
            CombatManager.TakeDamage(gameObject.tag, 25);
            Instantiate(_bloodPrefab, spawnPosition, spawnRotation);
        }
        _playerController.isDamage = true;
    }
}
