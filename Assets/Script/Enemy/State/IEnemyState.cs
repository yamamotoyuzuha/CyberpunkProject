
/// <summary>
/// Enemyのステート
/// </summary>
public interface IEnemyState
{
    void Enter();
    void Update();
    void Exit();
}

/// <summary>
/// ステートの種類
/// </summary>
public enum EnemyState
{
    None,
    Idle,
    Move,
    Chase,
    Attack,
    Damage,
    Death
}
