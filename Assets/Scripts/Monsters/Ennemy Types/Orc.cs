using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Ennemy
{
    protected override void Awake()
    {
        base.Awake();

        //IdleState = new OrcIdleState(this, StateMachine);
        ChasingState = new OrcChasingState(this, StateMachine);
    }
}
