using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollision : MonoBehaviour
{
    [Header("Component")]
    public WeaponSwitch _weaponSwitch;
    public PlayerController _playerController;

    [Header("Prefab")]
    public GameObject _sparkPrefab;

    [Header("Time")]
    public static float _disableTime = default;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(AutoDisable(_disableTime));
    }

    private IEnumerator AutoDisable(float time)
    {
        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            var collisionPoint = other.ClosestPoint(transform.position);
            var surfaceNormal = other.ClosestPointOnBounds(transform.position) -    transform.position;
            Vector3 spawnPosition = collisionPoint + surfaceNormal;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);

            Instantiate(_sparkPrefab, spawnPosition, rotation);
        }
    }
}
