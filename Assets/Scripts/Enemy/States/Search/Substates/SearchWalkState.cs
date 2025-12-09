using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SearchWalkState : EnemyState {

    public SearchWalkState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        enemy.SetTargetPosition(enemy.playerSeachPosition);
        enemy.OnMoveDestinationReached += OnDestinationReached;
    }

    public override void ExitState() {
        base.ExitState();

        enemy.OnMoveDestinationReached -= OnDestinationReached;
    }

    public override void FrameUpdate() {
        base.FrameUpdate();

        Vector2 lineOfSightDirection = (enemy.player.position - enemy.transform.position).normalized;
        enemy.isSearching = enemy.SearchLineOfSight(lineOfSightDirection);
        enemy.isAlerted = enemy.AlertLineOfSight(lineOfSightDirection);

        if (enemy.isSearching && !enemy.isAlerted) {
            enemy.SetTargetPosition(enemy.playerSeachPosition);
        } else if (enemy.isAlerted){
            enemy.transform.DOLocalJump(enemy.transform.position, 1f, 1, 0.5f).SetEase(Ease.InOutQuint);
            enemy.StateMachine.ChangeState(enemy.AlertedState);
        }

        enemy.Move();

        if (enemy.isKnockedBack) {
            enemy.StateMachine.ChangeState(enemy.AlertedState);
        }
    }

    private void OnDestinationReached() {
        enemy.playerSeachPosition = new Vector3(1000, 1000, 1000);
        enemy.searchLookCounter = 4;
        enemy.StateMachine.ChangeState(enemy.SearchState);
    }

}