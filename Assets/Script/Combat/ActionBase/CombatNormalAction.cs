using UnityEngine;

/// <summary>
/// 通常攻撃
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/CombatNormalAction")]
public class CombatNormalAction : CombatActionBase
{
    public override CombatActionExecutor Create(CombatSystemContext combatSystemContext, CombatActionBase actionBase)
    {
        return new CombatNormalActionExecutor(combatSystemContext, actionBase);
    }

    public override bool IsPlayerInputAction(PlayerInputSystem inputSystem)
    {
        return inputSystem.Player.Attack.triggered;
    }
}
