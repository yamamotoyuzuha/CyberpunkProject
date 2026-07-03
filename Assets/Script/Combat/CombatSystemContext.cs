using UnityEngine;

/// <summary>
/// CombatActionExecutor（戦闘アクションの実行基底クラス）に対して
/// 必要な情報を提供するクラス
/// </summary>
public class CombatSystemContext
{
    /// <summary>
    /// 戦闘システム
    /// </summary>
    public PlayerCombatSystem CombatSystem { get; private set; }
    /// <summary>
    /// 戦闘アクションを実行しているキャラクター
    /// </summary>
    public GameObject Character { get; private set; }

    public CombatSystemContext(PlayerCombatSystem combatSystem, GameObject character)
    {
        CombatSystem = combatSystem;
        Character = character;
    }
}
