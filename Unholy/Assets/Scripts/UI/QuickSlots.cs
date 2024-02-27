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
    /// QuickSlots gameobject�� inspector â���� ���ϰ� �ִ� slot �迭 ���� enumȭ �߽��ϴ�. �� ���� ������ ��Ī�ǵ��� �Ͽ����ϴ�.
    /// </summary>
    enum Slots
    {
        None = 0,
        OneHand = 1,
        TwoHand = 2,
        Bow = 3
    }

    /// <summary>
    /// ������ ���⸦ ǥ���ϱ� ���� ǥ�õ� ���⸦ slected ������ �ٲٴ� �޼ҵ��Դϴ�. ���õ� ���Ⱑ slected ������ �ٲ�� �Ͱ� ���ÿ� ���õ��� ���� ����� �ٽ� ������ ������ �ʱ�ȭ �˴ϴ�.
    /// </summary>
    /// <param name="_weaponType">���� ������ ���� ������ ���մϴ�.</param>
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
    /// ��� ���� ������ ���õ��� ���� ������ �ʱ�ȭ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    public void InitializeColor()
    {
        foreach(var item in _images)
        {
            item.color = unselected; 
        }
    }

}
