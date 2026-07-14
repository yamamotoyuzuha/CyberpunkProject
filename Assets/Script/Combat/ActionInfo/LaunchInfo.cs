using System;
using UnityEngine;

/// <summary>
/// 打ち上げ攻撃の情報
/// </summary>
[Serializable]
public class LaunchInfo
{
    [Header("打ちあがる高さ"), SerializeField] private float _launchHeight;
    [Header("打ち上げ時間"), SerializeField] private float _launchTime;
    [Header("ヒットストップの時間"), SerializeField] private float _hitStopTime;
    
    public float LaunchHeight => _launchHeight;
    public float LaunchTime => _launchTime;
    public float HitStopTime => _hitStopTime;
}
