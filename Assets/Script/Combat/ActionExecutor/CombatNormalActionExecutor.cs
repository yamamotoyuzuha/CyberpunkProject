using UnityEngine;

/// <summary>
/// 通常攻撃の実行処理を行うクラス
/// <para>CombatActionExecutor（基底クラス）を継承していて、通常攻撃のフェーズごとの処理を実装する</para>
/// </summary>
public class CombatNormalActionExecutor : CombatActionExecutor
{
    public CombatNormalActionExecutor(CombatSystemContext combatSystemContext, CombatActionBase actionBase) 
        : base(combatSystemContext, actionBase){}

    protected override void Initialization() => base.Initialization();
    
    protected override void OnStart()
    {
        Debug.Log("Normal Start");
        
        // 攻撃対象のキャラクターを取得し、プレイヤーの向きをその方向へと向ける
        TakeDamageCharacter = Context.CombatSystem.GetAttackTarget(CombatActionBase.AttackCount);
        Context.CombatSystem.DirectionClosestEnemy(TakeDamageCharacter);
        
        // 移動を不可能にする
        Context.Player.PlayerState.SetCanMove(false);
        // アニメーションの再生を行う
        var info = CombatActionBase.GetActionInfo(Index);
        PlayAnimation(info);
    }

    protected override void OnUpdate()
    {
        Timer += Time.deltaTime;
        if (Timer <= ActionInfo.InputReservationTime && IsAttackExecuted) // 入力予約受け付け時間内か判定
        {
            if (Context.InputSystem.Player.Attack.triggered)
            {
                InputReservation = true;
                Debug.LogWarning("入力予約を受け付けました。");
            }
        }
        
        // 攻撃を行う
        if (Timer >= ActionInfo.InputAttackStartTime && !IsAttackExecuted)
        {
            AttackExecute();
            IsAttackExecuted = true;
        }

        // コンボ攻撃の受け付け時間に入力があったら、次の攻撃に移る
        if (Timer >= ActionInfo.StartInputReceptionTime && Timer <= ActionInfo.EndInputReceptionTime)
        {
            if (InputReservation || Context.InputSystem.Player.Attack.triggered)
            {
                NextAttack();
            }
        }
        
        // 現在の攻撃アニメーションが終了したら、攻撃を終了する
        if (Timer >= ActionInfo.EndTime)
        {
            CurrentPhase = Phase.End;
        }
    }
    
    protected override void OnEnd()
    {
        Debug.Log("Normal End");
        // 移動を可能にする
        Context.Player.PlayerState.SetCanMove(true);
    }

    /// <summary>
    /// 次の攻撃へと移行する
    /// </summary>
    private void NextAttack()
    {
        Index++;
        
        // 攻撃アニメーションの情報を取得
        var info = CombatActionBase.GetActionInfo(Index);
        if (info == null) // もし、取得出来なかった場合は攻撃を終了する
        {
            CurrentPhase = Phase.End;
            return;
        }
        
        // アニメーション前に攻撃前の踏み込みを行う
        StartAttackMovement();
        PlayAnimation(info);
        Timer = 0;
        InputReservation = false;
        IsAttackExecuted = false;
    }
}
