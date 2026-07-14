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
    [Header("ノックバックの設定"), SerializeField] private KnockbackInfo _knockbackInfo;
    [Header("アニメーション関連の設定"), SerializeField] private AnimationInfo _animationInfo;
    
    public KnockbackInfo KnockbackInfo => _knockbackInfo;
    public AnimationInfo AnimationInfo => _animationInfo;
}
