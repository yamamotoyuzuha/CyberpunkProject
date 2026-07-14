using System;
using UnityEngine;

/// <summary>
/// 攻撃の情報
/// ・攻撃内容の情報
/// ・アニメーション関連で必要な情報
/// </summary>
[Serializable]
public class ActionInfoBase
{
    [Header("攻撃の種類"), SerializeField] private ActionType _actionType;
    [Header("ノックバックの設定"), SerializeField] private KnockbackInfo _knockbackInfo;
    [Header("打ち上げ攻撃の設定"), SerializeField] private LaunchInfo _launchInfo;
    [Header("アニメーション関連の設定"), SerializeField] private AnimationInfo _animationInfo;

    public ActionType ActionType => _actionType;
    public KnockbackInfo KnockbackInfo => _knockbackInfo;
    public LaunchInfo LaunchInfo => _launchInfo;
    public AnimationInfo AnimationInfo => _animationInfo;
}
