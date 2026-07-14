using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Enemyのステートを管理するクラス
/// </summary>
public class EnemyStateMachine : MonoBehaviour
{
    private Dictionary<EnemyState, IEnemyState> _enemyStates;
    private IEnemyState _currentState;

    private void Awake()
    {
        
    }

    private void Update()
    {
        _currentState?.Update();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="context">Enemyの情報</param>
    public void Initialization(EnemyContext context)
    {
        // ステートのインスタンス化
        _enemyStates = new Dictionary<EnemyState, IEnemyState>()
        {
            { EnemyState.Idle, new IdleState() },
            { EnemyState.Move, new MoveState(context) },
            { EnemyState.Chase , new ChaseState(context) },
            { EnemyState.Attack , new AttackState(context) },
            { EnemyState.Damage, new DamageState(context) }
        };

        _currentState = _enemyStates[EnemyState.Move];
        _currentState?.Enter();
    }

    public void SwitchState(EnemyState type)
    {
        _currentState?.Exit();
        
        // 新しいステートを取得
        if (_enemyStates.TryGetValue(type, out var newState))
        {
            _currentState = newState;
            _currentState?.Enter();
        }
        else
        {
            Debug.LogError("ステートが取得できませんでした");
        }
    }
}
