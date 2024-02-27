using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlots : MonoBehaviour
{
    [SerializeField] public Image[] _images;
    [SerializeField] Animator _animator;

    Color selected = new Color32(225, 225, 225, 225);
    Color unselected = new Color32(100, 100, 100, 225);


    /// <summary>
    /// QuickSlots gameobject가 inspector 창에서 지니고 있는 slot 배열 값을 enum화 했습니다. 각 무기 종류와 매칭되도록 하였습니다.
    /// </summary>
    enum Slots
    {
        None = 0,
        OneHand = 1,
        TwoHand = 2,
        Bow = 3
    }

    /// <summary>
    /// 선택한 무기를 표시하기 위해 표시된 무기를 slected 색으로 바꾸는 메소드입니다. 선택된 무기가 slected 색으로 바뀌는 것과 동시에 선택되지 않은 무기는 다시 원래의 색으로 초기화 됩니다.
    /// </summary>
    /// <param name="_weaponType">현재 선택한 무기 종류를 뜻합니다.</param>
    public void TurnOnSlots(int _weaponType)
    {
        InitializeColor();

        switch(_weaponType)
        {
            case (int)WeaponType.OneHand:
                _images[(int)Slots.OneHand].color = selected; 
                break;
            case (int)WeaponType.TwoHand:
                _images[(int)Slots.TwoHand].color = selected;
                break;
            case (int)WeaponType.Bow:
                _images[(int)Slots.Bow].color = selected;
                break;
            default:
                break;
        }   
    }

    /// <summary>
    /// 모든 무기 슬롯을 선택되지 않은 색으로 초기화하는 메소드입니다.
    /// </summary>
    public void InitializeColor()
    {
        foreach(var item in _images)
        {
            item.color = unselected; 
        }
    }

}
