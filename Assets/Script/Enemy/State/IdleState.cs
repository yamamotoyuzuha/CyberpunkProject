
using UnityEngine;

public class IdleState : IEnemyState
{
    public void Enter()
    {
        Debug.Log("Entering IdleState");
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        Debug.Log("Exit IdleState");
    }
}
