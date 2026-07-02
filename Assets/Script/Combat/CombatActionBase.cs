using UnityEngine;

/// <summary>
/// 戦闘アクションの基底クラス
/// </summary>
public abstract class CombatActionBase : ScriptableObject
{
    [Header("入力のクールダウンタイム"), SerializeField] protected float _cooldownTime;
    [Header("攻撃対象人数"), SerializeField] protected int _attackCount;

    
    // TODO：各戦闘アクションにフェーズを持たせて、コンボ攻撃を行えるようにする
    // TODO：設計を考えておく
    
    // TODO：ここはあくまで、基底クラスであり派生先で使用する関数のみ記述する


    public abstract void PhaseUpdate(CombatActionPhase phase);
    
    /// <summary>
    /// プレイヤーが戦闘アクションを入力したか判定を行う
    /// </summary>
    /// <param name="inputSystem">プレイヤーのInputSystem</param>
    /// <returns>true：入力をした　false：入力をしていない</returns>
    public abstract bool IsPlayerInputAction(PlayerInputSystem inputSystem);
    
    /// <summary>
    /// 戦闘アクション開始
    /// </summary>
    protected virtual void OnStart(){}
    /// <summary>
    /// 戦闘アクション開始中
    /// </summary>
    protected virtual void OnUpdate(){}
    /// <summary>
    /// 戦闘アクション終了
    /// </summary>
    protected virtual void OnEnd(){}
}
