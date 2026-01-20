using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PatrolIdleState : EnemyState {

    private float lookingDuration = 3f;
    private float timeRemaining;
    private bool timerIsRunning = false;
    
    public PatrolIdleState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        timeRemaining = lookingDuration;
        timerIsRunning = false;
        enemy.bubbleIdle.SetActive(true);
    }

    public override void ExitState() {
        base.ExitState();

        enemy.bubbleIdle.SetActive(false);
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
            if (!timerIsRunning) {
                timerIsRunning = true;
            }
        }

        if (enemy.isKnockedBack) {
            enemy.StateMachine.ChangeState(enemy.StunState);
        }
    }

    public void TimerLooking() {
        if (timerIsRunning) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
            } else {
                enemy.StateMachine.ChangeState(enemy.PatrolState);
            }
        }
    }

}