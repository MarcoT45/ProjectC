using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefault : EnemyAI {

    protected override void Awake() {
        base.Awake();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChasingState = new EnemyChasingState(this, StateMachine);
        LookingState = new EnemyLookingState(this, StateMachine);
    }

    protected override void Start() {
        this.gridManager = GameObject.FindWithTag("GridManager").GetComponent<GridManager>();
        base.Start();
    }

}