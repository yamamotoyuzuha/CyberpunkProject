
using UnityEngine;

/// <summary>
/// Enemyの情報を保持する
/// </summary>
public class EnemyContext
{
    public Transform Transform { get; private set; }
    public EnemyMovement Movement { get; private set; }
    public EnemyStateMachine StateMachine { get; private set; }
    public EnemyDetection Detection { get; private set; }
    public EnemyCombat Combat { get; private set; }
    public DamageContext DamageContext { get; set; }

    public EnemyContext(Transform transform, EnemyMovement movement, EnemyStateMachine stateMachine, EnemyDetection detection, EnemyCombat combat)
    {
        Transform = transform;
        Movement = movement;
        StateMachine = stateMachine;
        Detection = detection;
        Combat = combat;
    }
}
