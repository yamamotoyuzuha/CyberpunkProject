using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// 各戦闘アクションの基底クラス（ScriptableObject）
/// ・CombatActionExecutorの生成と戦闘アクションの入力条件の定義
/// </summary>
public abstract class CombatActionBase : ScriptableObject
{
    [Header("アニメーターのパラメータ名"), SerializeField] private ActionAnimParameter _actionAnimParameter;
    [Header("攻撃情報"), SerializeField] private List<ActionInfoBase> _actionInfos;
    [Header("攻撃対象人数"), SerializeField] private int _attackCount;
    
    /// <summary>
    /// アニメーターのパラメータ名
    /// </summary>
    public ActionAnimParameter AnimParameter => _actionAnimParameter;
    /// <summary>
    /// 攻撃情報
    /// </summary>
    public List<ActionInfoBase> ActionInfos => _actionInfos;
    /// <summary>
    /// 攻撃対象人数
    /// </summary>
    public int AttackCount => _attackCount;

    /// <summary>
    /// この戦闘アクションを実行するためのCombatActionExecutorを生成する
    /// </summary>
    /// <param name="combatSystemContext">戦闘アクションに必要な情報</param>
    /// <returns>この戦闘アクションのExecutor</returns>
    public abstract CombatActionExecutor Create(CombatSystemContext combatSystemContext, CombatActionBase actionBase);
    
    /// <summary>
    /// プレイヤーが戦闘アクションを入力したか判定を行う
    /// </summary>
    /// <param name="inputSystem">プレイヤーのInputSystem</param>
    /// <returns>true：入力をした　false：入力をしていない</returns>
    public abstract bool IsPlayerInputAction(PlayerInputSystem inputSystem);

    /// <summary>
    /// indexに応じた攻撃情報を取得する
    /// </summary>
    /// <param name="index">インデックス</param>
    /// <returns>攻撃情報を返す</returns>
    public ActionInfoBase GetActionInfo(int index)
    {
        if(_actionInfos.Count == 0) return null;

        var loopIndex = index % _actionInfos.Count;
        return _actionInfos[loopIndex];
    }
}

/// <summary>
/// アニメーターのパラメータ名
/// </summary>
[Serializable]
public class ActionAnimParameter
{
    [Header("アニメーターのパラメータ名（Trigger）"), SerializeField] private string _parameterTriggerName;
    [Header("アニメーターのパラメータ名（float）"), SerializeField] private string _parameterFloatName;
    public string ParameterTriggerName => _parameterTriggerName;
    public string ParameterFloatName => _parameterFloatName;
}
