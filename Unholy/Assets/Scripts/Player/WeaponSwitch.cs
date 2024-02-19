using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public class WeaponSwitch : MonoBehaviour
{
    [Header("Weapon Field")]
    internal int weaponIndex = -1;


    private void Awake()
    {
        if (weaponIndex < 0)
            GetIndexOfWeaponTypes(WeaponType.Unarmed);
    }

    /// <summary>
    /// 현재 player가 지니고 있는 무기 type을 index 값으로 바꿔주는 메소드입니다.
    /// </summary>
    /// <param name="currentType">현재 player가 지니고 있는 무기</param>
    public void GetIndexOfWeaponTypes(WeaponType currentType)
    {
        switch(currentType)
        {
            case WeaponType.Unarmed:
                weaponIndex = 1; break;
            case WeaponType.OneHand:
                weaponIndex = 2; break;
            case WeaponType.TwoHand:
                weaponIndex = 3; break;
            case WeaponType.Bow:
                weaponIndex = 4; break;
            default:
                weaponIndex = 0; break;
        }
    }
}
