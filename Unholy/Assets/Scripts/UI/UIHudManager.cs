using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHudManager : MonoBehaviour
{
    [Header("UI Components")]
    private UIStatBar _hpBar;
    private UIStatBar _staminaBar;
    private UIStatBar _bossHpBar;
    private QuickSlots _quickSlots;
    private UIPopUp _uiPopUp;

    [Header("Other Components")]
    [SerializeField] private Animator _playerAnimator; 

    private void Awake()
    {
        _hpBar = transform.GetChild(0).GetChild(0).GetComponent<UIStatBar>();
        _staminaBar = transform.GetChild(0).GetChild(1).GetComponent<UIStatBar>();
        _bossHpBar = transform.GetChild(1).GetChild(0).GetComponent<UIStatBar>();
        _quickSlots = transform.GetChild(2).GetComponent<QuickSlots>();
        _uiPopUp = transform.GetChild(3).GetComponent<UIPopUp>();
        
        _hpBar.SetMaxStat(maxValue: CombatManager._playerMaxHP);
        _staminaBar.SetMaxStat(maxValue: (int)CombatManager._playerMaxStamina);
        _bossHpBar.SetMaxStat(CombatManager._bossMaxHP, true);

        _quickSlots.InitializeColor();
        _uiPopUp.InitializeUI();

    }

    void Update()
    {
        _hpBar.SetStat((int)CombatManager._currentPlayerHP);
        _staminaBar.SetStat((int)CombatManager._currentPlayerST);
        _bossHpBar.SetStat((int)CombatManager._currentBossHP);

        _quickSlots.TurnOnSlots(_playerAnimator.GetInteger(PlayerAnimParameter.WeaponType));

        if (CombatManager._currentBossHP <= 0 || CombatManager._currentPlayerHP <= 0)
            _uiPopUp.EndProcess();
    }
}
