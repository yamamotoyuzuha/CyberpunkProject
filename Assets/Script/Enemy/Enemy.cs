using UnityEngine;

/// <summary>
/// Enemyの基底クラス
/// </summary>
public class Enemy : CharacterBase, IDamageable
{
    [Header("EnemyMovement"), SerializeField] private EnemyMovement _movement;
    [Header("EnemyStateMachine"), SerializeField] private EnemyStateMachine _stateMachine;

    private EnemyContext _context;
    
    private void Awake()
    {
        _context = new EnemyContext(_movement, _stateMachine);
        _stateMachine.Initialization(_context);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    
    public void TakeDamage(DamageContext context)
    {
        Debug.LogWarning("ダメージを受けた" + name);
        
        _context.DamageContext = context;
        _stateMachine.SwitchState(EnemyState.Damage);
    }
}
