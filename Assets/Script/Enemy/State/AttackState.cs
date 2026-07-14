using UnityEngine;

public class AttackState : IEnemyState
{
    private EnemyContext _context;

    public AttackState(EnemyContext context)
    {
        _context = context;
    }
    
    public void Enter()
    {
        Debug.LogWarning("攻撃をはじめます");
        
        _context.Combat.AttackStart(_context.Detection.Target);
    }

    public void Update()
    {
        var target = _context.Detection.Target;
        if (target == null)
        {
            _context.StateMachine.SwitchState(EnemyState.Move);
            return;
        }

        var distance = Vector3.Distance(_context.Detection.Target.transform.position, _context.Transform.position);
        if (distance <= 1f)
        {
            _context.StateMachine.SwitchState(EnemyState.Chase);
            return;
        }

        if (_context.Combat.IsAttackFinished)
        {
            _context.StateMachine.SwitchState(EnemyState.Chase);
        }
    }

    public void Exit()
    {
        
    }
}
