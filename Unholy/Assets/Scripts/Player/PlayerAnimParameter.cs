using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animation parameter를 정의해놓은 클래스입니다.
/// </summary>
public class PlayerAnimParameter
{
    public static readonly int IsWalk = Animator.StringToHash("IsWalk");
    public static readonly int IsSprint = Animator.StringToHash("IsSprint");
    public static readonly int IsJump = Animator.StringToHash("IsJump");
    public static readonly int IsPlayingJumpAnimation = Animator.StringToHash("IsPlayingJumpAnimation");
    public static readonly int IsAir= Animator.StringToHash("IsAir");

}
