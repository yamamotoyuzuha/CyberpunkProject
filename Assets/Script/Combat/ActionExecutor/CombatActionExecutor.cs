using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦闘アクションの実行クラス
/// </summary>
public abstract class CombatActionExecutor
{
    /// <summary>
    /// 戦闘アクションの実行フェーズ
    /// </summary>
    protected enum Phase
    {
        /// <summary>アクション開始</summary>
        Start,
        /// <summary>アクション実行中</summary>
        Update,
        /// <summary>アクション終了</summary>
        End,
        /// <summary>アクション完了</summary>
        Finished
    }
    
    /// <summary>
    /// 現在の実行フェーズ
    /// </summary>
    protected Phase CurrentPhase;

    /// <summary>
    /// 実装する戦闘アクション
    /// </summary>
    protected CombatActionBase CombatActionBase;
    /// <summary>
    /// 戦闘アクションに必要な情報
    /// </summary>
    protected CombatSystemContext Context;

    /// <summary>
    /// ダメージを与えるキャラクターを保持する
    /// </summary>
    protected GameObject[] TakeDamageCharacter;
    
    /// <summary>
    /// 戦闘アクションが終了したか
    /// </summary>
    public bool IsFinished => CurrentPhase == Phase.Finished;

    protected CombatActionExecutor(CombatSystemContext combatSystem, CombatActionBase actionBase)
    {
        CurrentPhase = Phase.Start;
        Context = combatSystem;
        CombatActionBase = actionBase;
    }

    /// <summary>
    /// 現在のフェーズを更新する
    /// </summary>
    public void PhaseUpdate()
    {
        switch (CurrentPhase)
        {
            case Phase.Start:
                OnStart();
                CurrentPhase = Phase.Update;
                break;
            
            case Phase.Update:
                OnUpdate();
                CurrentPhase = Phase.End;
                break;
            
            case Phase.End:
                OnEnd();
                CurrentPhase = Phase.Finished;
                break;
        }
    }
    
    /// <summary>
    /// 戦闘アクション開始時
    /// </summary>
    protected virtual void OnStart() { }
    
    /// <summary>
    /// 戦闘アクション実行中
    /// </summary>
    protected virtual void OnUpdate() { }
    
    /// <summary>
    /// 戦闘アクション終了
    /// </summary>
    protected virtual void OnEnd() { }
}
