using UnityEngine;

/// <summary>
/// Enemyの基底クラス
/// </summary>
public class Enemy : CharacterBase, IDamageable
{
    [Header("EnemyMovement"), SerializeField] private EnemyMovement _movement;
    [Header("EnemyStateMachine"), SerializeField] private EnemyStateMachine _stateMachine;
    [Header("EnemyUI"), SerializeField] private EnemyUI _enemyUI;

    private CharacterStatus _status;
    private EnemyContext _context;
    
    private void Awake()
    {
        _status = new CharacterStatus(100, 100);
        _context = new EnemyContext(_movement, _stateMachine);
        _stateMachine.Initialization(_context);
        _enemyUI.Initialization(_status);
        
        EnemyDamageUI.Instance.Initialization(_status);
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
        _status.TakeDamage(transform, context.Damage);
    }
}
