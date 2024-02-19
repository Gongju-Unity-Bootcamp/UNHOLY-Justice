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
    /// ���� player�� ���ϰ� �ִ� ���� type�� index ������ �ٲ��ִ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="currentType">���� player�� ���ϰ� �ִ� ����</param>
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
