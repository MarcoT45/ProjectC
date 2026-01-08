using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PatrolWalkState : EnemyState {
    
    public PatrolWalkState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        enemy.SetTargetPosition(enemy.gridManager.FindRandomWalkableInRange(enemy.transform.position, enemy.alertDistance));
        enemy.OnMoveDestinationReached += OnDestinationReached;
    }

    public override void ExitState() {
        base.ExitState();

        enemy.OnMoveDestinationReached -= OnDestinationReached;
    }

    public override void FrameFixedUpdate() {
        base.FrameUpdate();

        Vector2 lineOfSightDirection = enemy.forwardDirection;
        enemy.isSearching = enemy.SearchLineOfSight(lineOfSightDirection);
        enemy.isAlerted = enemy.AlertLineOfSight(lineOfSightDirection);

        if (enemy.isSearching && !enemy.isAlerted) {
            enemy.StateMachine.ChangeState(enemy.SearchState);
        } else if (enemy.isAlerted){
            enemy.transform.DOLocalJump(enemy.transform.position, 1f, 1, 0.5f).SetEase(Ease.InOutQuint);
            enemy.StateMachine.ChangeState(enemy.AlertedState);
        } else {
            enemy.Move();
        }

        if (enemy.isKnockedBack) {
            enemy.StateMachine.ChangeState(enemy.AlertedState);
        }
    } 

    private void OnDestinationReached() {
        enemy.StateMachine.ChangeState(enemy.PatrolState);
    }

}