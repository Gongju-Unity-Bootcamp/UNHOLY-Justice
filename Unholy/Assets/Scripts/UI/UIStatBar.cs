using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private RectTransform _rectTransform;

    private int widthScaleMultiplier = 2;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetStat(int newValue)
    {
        _slider.value = newValue;
    }

    public void SetMaxStat(int maxValue, bool isBoss = false)
    {
        _slider.maxValue = maxValue;
        
        if(!isBoss)
        {
            _rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, _rectTransform.sizeDelta.y);
        }
    }
}
