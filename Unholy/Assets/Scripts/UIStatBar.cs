using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatBar : MonoBehaviour
{
    private Slider _slider;
    private RectTransform _rectTransform;

    private int widthScaleMultiplier = 2;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _rectTransform = GetComponent<RectTransform>();
        SetMaxStat(Mathf.RoundToInt(CombatManager._playerMaxStamina));
    }

    private void Update()
    {
        SetStat(Mathf.RoundToInt(CombatManager._currentPlayerST));
    }

    public virtual void SetStat(int currentValue)
    {
        _slider.value = currentValue;
    }

    public virtual void SetMaxStat(int maxValue)
    {
        _slider.maxValue = maxValue;
        _slider.value = maxValue;

        _rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, _rectTransform.sizeDelta.y);
    }
}
