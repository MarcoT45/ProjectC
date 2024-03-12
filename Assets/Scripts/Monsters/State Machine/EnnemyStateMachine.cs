using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyStateMachine
{
    public EnnemyState currentEnnemyState {  get; set; }

    public void Initialize( EnnemyState startingState)
    {
        currentEnnemyState = startingState;
        currentEnnemyState.EnterState();

    }

    public void ChangeState(EnnemyState newState)
    {
        currentEnnemyState.ExitState();
        currentEnnemyState = newState;
        currentEnnemyState.EnterState();
    }
}
