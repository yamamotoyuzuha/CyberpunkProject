
public class MoveState : IEnemyState
{
    private EnemyContext _context;
    
    public MoveState(EnemyContext context)
    {
        _context = context;
    }
    
    public void Enter()
    {
        
    }

    public void Update()
    {
        // ターゲットを発見したら、追跡に移る
        if (_context.Detection.IsTarget) 
        {
            _context.StateMachine.SwitchState(EnemyState.Chase);
            return;
        }
        
        _context.Movement.Move();
    }

    public void Exit()
    {
        
    }
}
