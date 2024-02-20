using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public class WeaponSwitch : MonoBehaviour
{
    [Header("Weapon Field")]
    // ���� �� �������� �����ϱ⿡ �ε��� num�� -1���� ����
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

    /// <summary>
    /// ���� player�� ���ϰ� �ִ� ���� type�� Melee(One Hand, Two Hand) ���� �ƴ����� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <returns>true : Melee�̴�. false : Melee�� �ƴϴ�.</returns>
    public bool IsWeaponMelee()
    {
        if (weaponIndex == 2 || weaponIndex == 3)
            return true;
        else return false;
    }
}
