using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinoCollider : MonoBehaviour
{
    internal bool _canAttack = false;

    PlayerController _playerController;


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
        if (other.gameObject.CompareTag("Player"))
        {
            _enterCol = true;
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
        CombatManager.TakeDamage(gameObject.tag, 25);
        _playerController.isDamage = true;
    }
}
