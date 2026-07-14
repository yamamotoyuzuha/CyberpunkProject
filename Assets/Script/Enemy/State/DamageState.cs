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
        switch (damageContext.HitReactionContext.ActionType)
        {
            case ActionType.None:
                // 何も割り当てられていない場合、待機に遷移s
                _context.StateMachine.SwitchState(EnemyState.Idle);
                break;
            
            case ActionType.Knockback:
                // ノックバックを実行
                //_context.Movement.KnockbackExecute(damageContext);
                _context.Movement.DamageExecute(damageContext);
                break;
            
            case ActionType.Launch:
                // 打ち上げを実行（ダメージを受けたEnemyとダメージを与えたプレイヤー両方に）
                _context.Movement.LaunchExecute(damageContext);
                _context.DamageContext.PlayerAttacked.LaunchExecute(damageContext);
                break;
        }
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        
        // TODO：ここもノックバック依存になってるから修正を行う
        // 硬直時間を超えてて、ノックバックありの場合は待機に戻る
        if(_timer >= _context.DamageContext.HitReactionContext.Duration
           && ActionType.Knockback == _context.DamageContext.HitReactionContext.ActionType)
            _context.StateMachine.SwitchState(EnemyState.Idle);
        /*
        if(_timer >= _context.DamageContext.Duration && _context.DamageContext.IsKnockback) 
            _context.StateMachine.SwitchState(EnemyState.Idle);
        */
    }

    public void Exit()
    {
        Debug.Log("Exit DamageState");
        
        _timer = 0f;
    }
}
