using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyState {

    public PatrolState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        enemy.bubbleSearch.SetActive(false);
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameFixedUpdate() {
        base.FrameFixedUpdate();

        int random = Random.Range(0, 100);

        if(random > 79) {
            enemy.StateMachine.ChangeState(enemy.PatrolIdleState);
        } else {
            enemy.StateMachine.ChangeState(enemy.PatrolWalkState);
        }

        if(enemy.isKnockedBack) {
            enemy.StateMachine.ChangeState(enemy.StunState);
        }
    }

}