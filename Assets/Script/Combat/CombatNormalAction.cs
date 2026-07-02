using UnityEngine;

/// <summary>
/// 通常攻撃
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/CombatNormalAction")]
public class CombatNormalAction : CombatActionBase
{
    
    
    // TODO：これをもっといい方法でやった方がいい
    // TODO：今のままだと、プレイヤーが入力をして終了までをやってしまうことになるから
    
    
    public override void PhaseUpdate(CombatActionPhase phase)
    {
        switch (phase.Phase)
        {
            case CombatActionPhase.ActionPhase.Start:
                OnStart();
                phase.Phase = CombatActionPhase.ActionPhase.Update;
                break;
            case CombatActionPhase.ActionPhase.Update:
                OnUpdate();
                phase.Phase = CombatActionPhase.ActionPhase.End;
                break;
            case CombatActionPhase.ActionPhase.End:
                OnEnd();
                phase.Phase = CombatActionPhase.ActionPhase.Active;
                break;
        }
    }

    public override bool IsPlayerInputAction(PlayerInputSystem inputSystem)
    {
        return inputSystem.Player.Attack.triggered;
    }

    protected override void OnStart()
    {
        base.OnStart();
        Debug.Log($"{this.name}: Start");
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        Debug.Log($"{this.name}: Update");
    }

    protected override void OnEnd()
    {
        base.OnEnd();
        Debug.Log($"{this.name}: End");
    }
}
