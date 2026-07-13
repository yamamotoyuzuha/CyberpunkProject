
/// <summary>
/// Enemyの情報を保持する
/// </summary>
public class EnemyContext
{
    public EnemyMovement Movement { get; private set; }
    public EnemyStateMachine StateMachine { get; private set; }
    public DamageContext DamageContext { get; set; }

    public EnemyContext(EnemyMovement movement, EnemyStateMachine stateMachine)
    {
        Movement = movement;
        StateMachine = stateMachine;
    }
}
