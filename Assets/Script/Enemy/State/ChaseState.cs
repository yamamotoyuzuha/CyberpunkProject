
using UnityEngine;

public class ChaseState : IEnemyState
{
    private EnemyContext _context;

    public ChaseState(EnemyContext context)
    {
        _context = context;
    }
    
    public void Enter()
    {
        
    }

    public void Update()
    {
        if (_context.Detection.Target == null)
        {
            _context.StateMachine.SwitchState(EnemyState.Move);
            return;
        }
        
        var distance = Vector3.Distance(_context.Detection.Target.transform.position, _context.Transform.position);
        if (distance <= 1.5f)
        {
            _context.StateMachine.SwitchState(EnemyState.Attack);
            return;
        }
        
        _context.Movement.Chase(_context.Detection.Target.transform);
    }

    public void Exit()
    {
        
    }
}
