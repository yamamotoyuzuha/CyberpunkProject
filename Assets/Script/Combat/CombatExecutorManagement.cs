using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 戦闘システムのフェーズ管理を行う
/// ・PlayerCombatSystemとCombatActionPhaseを繋ぐ
/// </summary>
public class CombatExecutorManagement
{
    /// <summary>
    /// プレイヤーの行える戦闘システムデータ
    /// </summary>
    private readonly List<CombatActionBase> _combatActions;

    /// <summary>
    /// 戦闘システムのフェーズ
    /// </summary>
    private CombatActionExecutor _combatActionExecute;

    public CombatExecutorManagement(List<CombatActionBase> combatActions)
    {
        _combatActions = combatActions;
    }

    /// <summary>
    /// 戦闘アクションを発動できるか判定を行う
    /// </summary>
    /// <param name="inputSystem">プレイヤーの入力</param>
    /// <param name="context">戦闘アクションに必要な情報</param>
    public void TryAction(PlayerInputSystem inputSystem, CombatSystemContext context)
    {
        if(context == null) return;
        
        foreach (var action in _combatActions)
        {
            // 戦闘アクションの入力を判定し、実行可能なら処理を行う
            if (action.IsPlayerInputAction(inputSystem) && _combatActionExecute == null)
            {
                _combatActionExecute = action.Create(context, action);
                break;
            }
        }
    }

    /// <summary>
    /// 発動した戦闘システムのフェーズ更新を行う
    /// </summary>
    public void Tick()
    {
        _combatActionExecute?.PhaseUpdate();
        if (_combatActionExecute != null && _combatActionExecute.IsFinished)
        {
            _combatActionExecute = null;
            Debug.Log("戦闘システム終了");
        }  
    }
}

