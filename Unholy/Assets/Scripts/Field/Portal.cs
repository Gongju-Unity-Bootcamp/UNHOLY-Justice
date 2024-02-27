using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    PlayerController _playerController;
    public Slider _slider;

    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _slider.gameObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.Play(SoundType.Bgm, "BattleBGM");
            gameObject.GetComponent<BoxCollider>().isTrigger = false;
            CombatManager._isIdle = true;
            _playerController.weaponSwitch = true;
            _playerController.isSwitchDone = true;
            _playerController.GetComponent<WeaponSwitch>().GetIndexOfWeaponTypes(WeaponType.OneHand);

            _slider.gameObject.SetActive(true);
        }
    }
}
