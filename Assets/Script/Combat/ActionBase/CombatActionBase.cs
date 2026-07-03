using UnityEngine;

/// <summary>
/// 各戦闘アクションの基底クラス（ScriptableObject）
/// ・CombatActionExecutorの生成と戦闘アクションの入力条件の定義
/// </summary>
public abstract class CombatActionBase : ScriptableObject
{
    [Header("入力のクールダウンタイム"), SerializeField] protected float _cooldownTime;
    [Header("攻撃対象人数"), SerializeField] protected int _attackCount;

    /// <summary>
    /// 攻撃対象人数
    /// </summary>
    public int AttackCount => _attackCount;


    // TODO：各戦闘アクションにフェーズを持たせて、コンボ攻撃を行えるようにする
    // TODO：設計を考えておく
    
    // TODO：ここはあくまで、基底クラスであり派生先で使用する関数のみ記述する

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
}
