using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class WeaponSwitch : MonoBehaviour
{
    [Header("Component")]
    private Animator _animator;
    public Transform _twoHand;
    public Transform _leftHandAttachPoint;

    [Header("Object")]
    public GameObject[] _prefabs;
    public GameObject[] _weapons;

    [Header("IK")]
    [Range(0, 1)] public float leftHandPositionWeight;
    [Range(0, 1)] public float leftHandRotationWeight;
    private Transform blendToTransform;
    private const float DELAYTIME = 0.75f;

    [Header("Weapon Field")]
    // ���� �� �������� �����ϱ⿡ �ε��� num�� -1���� ����
    internal int weaponIndex = -1;
    internal int weaponDamage = default;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        blendToTransform = _twoHand.transform.GetChild(0);

        if (weaponIndex < 0)
            GetIndexOfWeaponTypes(WeaponType.Unarmed);

        foreach(GameObject element in _weapons)
        {
            if (element != null) element.SetActive(false);
        }
    }

    private void Update()
    {
        if (!_animator.GetBool(PlayerAnimParameter.IsDefense))
            _animator.SetInteger(PlayerAnimParameter.WeaponType, weaponIndex);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(weaponIndex == (int)WeaponType.TwoHand)
        {
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandPositionWeight);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandRotationWeight);

            _animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandAttachPoint.position);
            _animator.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandAttachPoint.rotation);
        }
    }

    public void BlendIK(float timeToBlend, int weaponIndex)
    {
        StopAllCoroutines();
        StartCoroutine(_BlendIK(timeToBlend, weaponIndex));
    }

    public IEnumerator _BlendIK(float timeToBlend, int weaponIndex)
    {
        var t = 0f;
        var blendTo = 0;
        var blendFrom = 0;
        
        if(weaponIndex == (int)WeaponType.TwoHand)
        {
            leftHandPositionWeight = 0;
            leftHandRotationWeight = 0;
        }
        else
        {
            leftHandPositionWeight = 1;
            leftHandRotationWeight = 1;
        }

        yield return new WaitForSeconds(DELAYTIME);

        if (weaponIndex == (int)WeaponType.TwoHand) blendTo = 1;
        else blendFrom = 1;
        
        while(t < 1)
        {
            t += Time.deltaTime / timeToBlend;
            _leftHandAttachPoint = blendToTransform;
            leftHandPositionWeight = Mathf.Lerp(blendFrom, blendTo, t);
            leftHandRotationWeight = Mathf.Lerp(blendFrom, blendTo, t);
            yield return null;
        }
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
                weaponIndex = 1;
                weaponDamage = 0;
                break;
            case WeaponType.OneHand:
                weaponIndex = 2;
                weaponDamage = 25;
                break;
            case WeaponType.TwoHand:
                weaponIndex = 3;
                weaponDamage = 100;
                break;
            case WeaponType.Bow:
                weaponIndex = 4;
                break;
            default:
                weaponIndex = 0;
                break;
        }
    }

    /// <summary>
    /// ���� player�� ���ϰ� �ִ� ���� type�� Melee(One Hand, Two Hand) ���� �ƴ����� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// </summary>
    /// <returns>true : Melee�̴�. false : Melee�� �ƴϴ�.</returns>
    public bool IsWeaponMelee()
    {
        if (weaponIndex == (int)WeaponType.OneHand || weaponIndex == (int)WeaponType.TwoHand)
            return true;
        else return false;
    }
}
