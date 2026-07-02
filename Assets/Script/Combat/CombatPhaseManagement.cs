using System.Collections.Generic;

/// <summary>
/// 戦闘システムのフェーズ管理を行う
/// ・PlayerCombatSystemとCombatActionPhaseを繋ぐ
/// </summary>
public class CombatPhaseManagement
{
    /// <summary>
    /// プレイヤーの行える戦闘システムデータ
    /// </summary>
    private readonly List<CombatActionBase> _combatActions;
    /// <summary>
    /// 戦闘システムのフェーズ
    /// </summary>
    private CombatActionPhase _currentCombatActionPhase;

    public CombatPhaseManagement(List<CombatActionBase> combatActions)
    {
        _combatActions = combatActions;
    }

    /// <summary>
    /// 戦闘システムを発動できるか判定を行う
    /// </summary>
    /// <param name="inputSystem"></param>
    public void TryAction(PlayerInputSystem inputSystem)
    {
        foreach (var action in _combatActions)
        {
            if (action.IsPlayerInputAction(inputSystem) && _currentCombatActionPhase == null)
            {
                var phase = CombatActionPhase.ActionPhase.Start;
                _currentCombatActionPhase = new CombatActionPhase(phase, action);
            }
        }
    }

    /// <summary>
    /// 発動した戦闘システムのフェーズ更新を行う
    /// </summary>
    public void Tick()
    {
        _currentCombatActionPhase?.Action.PhaseUpdate(_currentCombatActionPhase);

        if (_currentCombatActionPhase?.Phase == CombatActionPhase.ActionPhase.Active)
        {
            _currentCombatActionPhase = null;
        }
    }
}

