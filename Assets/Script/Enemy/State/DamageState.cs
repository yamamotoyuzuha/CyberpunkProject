using UnityEngine;

public class DamageState : IEnemyState
{
    private readonly EnemyContext _context;

    private float _timer;
    
    public DamageState(EnemyContext context)
    {
        _context = context;
    }
    
    public void Enter()
    {
        Debug.Log("Enter DamageState");
        
        var damageContext = _context.DamageContext;
        if (!damageContext.IsKnockback) // ノックバックなしの場合、待機に戻る
        {
            _context.StateMachine.SwitchState(EnemyState.Idle);
            return;
        }
        
        // ノックバックを実行
        _context.Movement.KnockbackExecute(damageContext);
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        // 硬直時間を超えてて、ノックバックありの場合は待機に戻る
        if(_timer >= _context.DamageContext.Duration && _context.DamageContext.IsKnockback) 
            _context.StateMachine.SwitchState(EnemyState.Idle);
    }

    public void Exit()
    {
        Debug.Log("Exit DamageState");
        
        _timer = 0f;
    }
}
