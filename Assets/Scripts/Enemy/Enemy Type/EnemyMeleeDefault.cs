using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeDefault : EnemyAI {

    protected override void Awake() {
        base.Awake();

        PatrolState = new PatrolState(this, StateMachine);
        PatrolIdleState = new PatrolIdleState(this, StateMachine);
        PatrolWalkState = new PatrolWalkState(this, StateMachine);

        SearchState = new SearchState(this, StateMachine);
        SearchLookUpState = new SearchLookUpState(this, StateMachine);
        SearchLookDownState = new SearchLookDownState(this, StateMachine);
        SearchLookLeftState = new SearchLookLeftState(this, StateMachine);
        SearchLookRightState = new SearchLookRightState(this, StateMachine);
        SearchWalkState = new SearchWalkState(this, StateMachine);

        AlertedState = new ChasingState(this, StateMachine);
        AlertedLookingState = new ChasingLookingState(this, StateMachine);

        AttackState = new AttackSlashState(this, StateMachine);
    }

    protected override void Start() {
        this.gridManager = GameObject.FindWithTag("GridManager").GetComponent<GridManager>();
        base.Start();
    }

}