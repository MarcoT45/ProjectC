using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : EnemyState {

    public SearchState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        enemy.bubbleSearch.SetActive(true);
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
        base.FrameUpdate();

        if (enemy.playerSearchPosition == new Vector3(1000, 1000, 1000)) {
            if (enemy.searchLookCounter == 0) {
                enemy.StateMachine.ChangeState(enemy.PatrolState);
            } else {
                int random = Random.Range(0, 4);

                switch (random) {
                    case 0:
                        enemy.StateMachine.ChangeState(enemy.SearchLookUpState);
                        break;
                    case 1:
                        enemy.StateMachine.ChangeState(enemy.SearchLookDownState);
                        break;
                    case 2:
                        enemy.StateMachine.ChangeState(enemy.SearchLookLeftState);
                        break;
                    case 3:
                        enemy.StateMachine.ChangeState(enemy.SearchLookRightState);
                        break;
                }
            }
        } else {
            enemy.StateMachine.ChangeState(enemy.SearchWalkState);
        }
    }

}