using UnityEngine;

/// <summary>
/// 通常攻撃の実行処理を行うクラス
/// <para>CombatActionExecutor（基底クラス）を継承していて、通常攻撃のフェーズごとの処理を実装する</para>
/// </summary>
public class CombatNormalActionExecutor : CombatActionExecutor
{
    public CombatNormalActionExecutor(CombatSystemContext combatSystemContext, CombatActionBase actionBase) 
        : base(combatSystemContext, actionBase){}

    protected override void OnStart()
    {
        Debug.Log("Normal Start");
        Debug.Log(Context.Character.name);
        
        // TODO：攻撃対象のキャラクターを取得するのと、プレイヤーの向きをその方向へと向ける
        TakeDamageCharacter = Context.CombatSystem.GetTakeDamageCharacter(CombatActionBase.AttackCount);
        Context.CombatSystem.DirectionClosestTakeDamageCharacter(TakeDamageCharacter);
    }

    protected override void OnUpdate()
    {
        Debug.Log("Normal Update");

        // TODO：ここで仮にダメージを与えるキャラクターを出力する
        // TODO：ここでダメージを与える
        foreach (var chara in TakeDamageCharacter)
        {
            Debug.Log(chara.name);
        }
    }
    
    protected override void OnEnd()
    {
        Debug.Log("Normal End");
    }
}
