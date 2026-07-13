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
    /// 実行する戦闘アクション
    /// </summary>
    protected readonly CombatActionBase CombatActionBase;
    /// <summary>
    /// 戦闘アクションに必要な情報
    /// </summary>
    protected readonly CombatSystemContext Context;

    /// <summary>
    /// ダメージを与えるキャラクターを保持する
    /// </summary>
    protected GameObject[] TakeDamageCharacter;
    
    /// <summary>
    /// 攻撃アニメーションの情報
    /// </summary>
    protected ActionInfoBase ActionInfo;
    /// <summary>
    /// 現在の攻撃アニメーションのインデックス
    /// </summary>
    protected int Index;
    /// <summary>
    /// 攻撃入力タイマー
    /// </summary>
    protected float Timer;
    /// <summary>
    /// 入力予約
    /// <para>true:入力予約あり　false：入力予約なし</para>
    /// </summary>
    protected bool InputReservation;
    /// <summary>
    /// 攻撃の実行
    /// <para>true：実行済み　false：実行未済</para>
    /// </summary>
    protected bool IsAttackExecuted;
    
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
                Initialization();
                OnStart();
                CurrentPhase = Phase.Update;
                break;
            
            case Phase.Update:
                OnUpdate();
                break;
            
            case Phase.End:
                OnEnd();
                CurrentPhase = Phase.Finished;
                break;
        }
    }

    /// <summary>
    /// 攻撃アニメーターを再生する
    /// </summary>
    /// <param name="actionInfo">攻撃アニメーションの情報</param>
    protected void PlayAnimation(ActionInfoBase actionInfo)
    {
        ActionInfo = actionInfo;
        Context.CharacterAnimator.PlayAttackAnimation(CombatActionBase.AnimParameter, actionInfo);
    }

    /// <summary>
    /// 攻撃を実行する
    /// </summary>
    protected void AttackExecute()
    {
        // 攻撃対象を取得
        var targets = Context.CombatSystem.GetAttackTarget(CombatActionBase.AttackCount);
        if (targets == null) return;

        foreach (var target in targets)
        {
            // 攻撃対象にダメージを与える
            if(!target.TryGetComponent<IDamageable>(out var damageable)) continue;
            
            var direction = (target.transform.position - Context.Object.transform.position).normalized;

            var context = new DamageContext(ActionInfo.IsKnockback, direction, ActionInfo.KnockbackPower, 
                ActionInfo.KnockbackUpPower, ActionInfo.KnockbackDuration);
            damageable.TakeDamage(context);
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected virtual void Initialization()
    {
        Index = 0;
        Timer = 0;
        InputReservation = false;
        IsAttackExecuted = false;
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
