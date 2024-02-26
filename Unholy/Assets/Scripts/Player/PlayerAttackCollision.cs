using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollision : MonoBehaviour
{
    [Header("Component")]
    public WeaponSwitch _weaponSwitch;
    public PlayerController _playerController;

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
}
