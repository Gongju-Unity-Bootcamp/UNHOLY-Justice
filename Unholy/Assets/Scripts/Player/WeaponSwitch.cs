using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public class WeaponSwitch : MonoBehaviour
{
    [Header("Weapon Field")]
    // 무기 미 장착으로 시작하기에 인덱스 num이 -1부터 시작
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

    /// <summary>
    /// 현재 player가 지니고 있는 무기 type이 Melee(One Hand, Two Hand) 인지 아닌지를 반환하는 메소드입니다.
    /// </summary>
    /// <returns>true : Melee이다. false : Melee가 아니다.</returns>
    public bool IsWeaponMelee()
    {
        if (weaponIndex == 2 || weaponIndex == 3)
            return true;
        else return false;
    }
}
