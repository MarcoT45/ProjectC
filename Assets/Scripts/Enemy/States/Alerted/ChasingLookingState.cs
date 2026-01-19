using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChasingLookingState : EnemyState {

    private float alertDuration = 5f;
    private float timeAlertRemaining;

    private float lookingDuration = 0.5f;
    private float timeLookingRemaining;

    public ChasingLookingState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        timeAlertRemaining = alertDuration;
        timeLookingRemaining = lookingDuration;
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
        base.FrameUpdate();

        Vector2 lineOfSightDirection = enemy.forwardDirection;
        enemy.isSearching = enemy.SearchLineOfSight(lineOfSightDirection);
        enemy.isAlerted = enemy.AlertLineOfSight(lineOfSightDirection);

        if (enemy.isSearching || enemy.isAlerted) {
            enemy.StateMachine.ChangeState(enemy.AlertedState);
        } else {
            TimerAlert();
            TimerLooking();

            if(timeLookingRemaining < 0) {
                int random = Random.Range(0, 4);

                switch (random) {
                    case 0:
                        enemy.forwardDirection = Vector2.up;
                        break;
                    case 1:
                        enemy.forwardDirection = Vector2.down;
                        break;
                    case 2:
                        enemy.forwardDirection = Vector2.left;
                        break;
                    case 3:
                        enemy.forwardDirection = Vector2.right;
                        break;
                }

                timeLookingRemaining = lookingDuration;
            }
        }

        if (enemy.isKnockedBack) {
            enemy.StateMachine.ChangeState(enemy.StunState);
        }
    }

    public void TimerLooking() {
        timeLookingRemaining -= Time.deltaTime;
    }

    public void TimerAlert() {
        if(timeAlertRemaining > 0) {
            timeAlertRemaining -= Time.deltaTime;
        } else {
            enemy.transform.DOLocalJump(enemy.transform.position, 0.5f, 3, 0.5f).SetEase(Ease.InOutQuint);
            enemy.StateMachine.ChangeState(enemy.PatrolState);
        }
    }

}