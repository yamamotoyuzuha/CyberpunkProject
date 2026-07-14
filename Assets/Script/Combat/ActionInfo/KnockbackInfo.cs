using System;
using UnityEngine;

/// <summary>
/// ノックバック情報
/// </summary>
[Serializable]
public class KnockbackInfo
{
    [Header("ノックバックを行うか"), SerializeField] private bool _isKnockback;
    [Header("攻撃方向への吹き飛ばし力"), SerializeField] private float _knockbackPower;
    [Header("上方向への吹き飛ばし力"), SerializeField] private float _knockbackUpPower;
    [Header("ノックバックの硬直時間"), SerializeField] private float _knockbackDuration;
    
    public bool IsKnockback => _isKnockback;
    public float KnockbackPower => _knockbackPower;
    public float KnockbackUpPower => _knockbackUpPower;
    public float KnockbackDuration => _knockbackDuration;
}
