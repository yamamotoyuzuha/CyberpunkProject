using System;
using UnityEngine;

/// <summary>
/// アニメーションの情報
/// </summary>
[Serializable]
public class AnimationInfo
{
    [Header("アニメーションクリップ"), SerializeField] private AnimationClip _animationClip;
    [Header("BlendTreeに渡す値"), SerializeField] private int _animParameter;
    [Header("コンボ攻撃の受付時間")]
    [SerializeField, Range(0, 1)] private float _startInputReceptionTime;
    [SerializeField, Range(0, 1)] private float _endInputReceptionTime;
    [Header("入力予約時間")]
    [SerializeField, Range(0, 1)] private float _inputReservationTime;
    [Header("攻撃入力の受付時間")] 
    [SerializeField, Range(0, 1)] private float _inputAttackStartTime;
    
    /// <summary>
    /// BlendTreeに渡す値
    /// </summary>
    public int AnimParameter => _animParameter;
    /// <summary>
    /// コンボ攻撃の入力受付開始時間
    /// </summary>
    public float StartInputReceptionTime => 
        _animationClip.length * _startInputReceptionTime;
    /// <summary>
    /// コンボ攻撃の入力終了時間
    /// </summary>
    public float EndInputReceptionTime => 
        _animationClip.length * _endInputReceptionTime;
    /// <summary>
    /// 入力予約受付時間
    /// </summary>
    public float InputReservationTime => 
        _animationClip.length * _inputReservationTime;
    /// <summary>
    /// 攻撃入力の受付開始時間
    /// </summary>
    public float InputAttackStartTime =>
        _animationClip.length * _inputAttackStartTime;
    /// <summary>
    /// アニメーション終了時間
    /// </summary>
    public float EndTime => _animationClip.length;
}
