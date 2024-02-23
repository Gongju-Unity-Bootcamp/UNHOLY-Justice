using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animation parameter�� �����س��� Ŭ�����Դϴ�.
/// </summary>
public class PlayerAnimParameter
{
    public static readonly int IsAttack = Animator.StringToHash("IsAttack");
    public static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    public static readonly int IsAbleComboAttack = Animator.StringToHash("IsAbleComboAttack");
    public static readonly int IsWalk = Animator.StringToHash("IsWalk");
    public static readonly int IsSprint = Animator.StringToHash("IsSprint");
    public static readonly int IsJump = Animator.StringToHash("IsJump");
    public static readonly int IsAir = Animator.StringToHash("IsAir");
    public static readonly int IsDodge = Animator.StringToHash("IsDodge");
    public static readonly int IsDefense = Animator.StringToHash("IsDefense");
    public static readonly int IsParry = Animator.StringToHash("IsParry");
    public static readonly int IsDamage = Animator.StringToHash("IsDamage");
    public static readonly int IsTargeting = Animator.StringToHash("IsTargeting");
    public static readonly int WeaponType = Animator.StringToHash("WeaponType");
    public static readonly int WeaponSwitch = Animator.StringToHash("WeaponSwitch");
    public static readonly int IsSwitchDone = Animator.StringToHash("IsSwitchDone");
    public static readonly int HorizontalMovement = Animator.StringToHash("HorizontalMovement");
    public static readonly int VerticalMovement = Animator.StringToHash("VerticalMovement");
}
