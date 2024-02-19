using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animation parameter를 정의해놓은 클래스입니다.
/// </summary>
public class PlayerAnimParameter
{
    public static readonly int IsAttack = Animator.StringToHash("IsAttack");
    public static readonly int IsWalk = Animator.StringToHash("IsWalk");
    public static readonly int IsSprint = Animator.StringToHash("IsSprint");
    public static readonly int IsJump = Animator.StringToHash("IsJump");
    public static readonly int IsAir = Animator.StringToHash("IsAir");
    public static readonly int IsDodge = Animator.StringToHash("IsDodge");
    public static readonly int IsDamage = Animator.StringToHash("IsDamage");
    public static readonly int AbleToJump = Animator.StringToHash("AbleToJump");
    public static readonly int AbleToDodge = Animator.StringToHash("AbleToDodge");
    public static readonly int WeaponType = Animator.StringToHash("WeaponType");
}
