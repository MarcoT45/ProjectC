using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SearchLookUpState : EnemyState {

    private float lookingDuration = 1.5f;
    private float timeRemaining;
    private bool timerIsRunning = false;

    public SearchLookUpState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        timeRemaining = lookingDuration;
        timerIsRunning = false;
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
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
            TimerLooking();
            enemy.forwardDirection = Vector2.up;

            if (!timerIsRunning) {
                timerIsRunning = true;
            }
        }

        if (enemy.isKnockedBack) {
            enemy.StateMachine.ChangeState(enemy.AlertedState);
        }
    }

    public void TimerLooking() {
        if (timerIsRunning) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
            } else {
                enemy.searchLookCounter--;
                enemy.StateMachine.ChangeState(enemy.SearchState);
            }
        }
    }

}