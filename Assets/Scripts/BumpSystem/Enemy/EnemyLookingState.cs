using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyLookingState : EnemyState {

    private int tmpLook = 0;
    private float lookingDuration = 1f;
    private float timeRemaining;
    private bool timerIsRunning = false;

    public EnemyLookingState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        Debug.Log("Looking");

        tmpLook = 0;
        timerIsRunning = false;
        timeRemaining = lookingDuration;
    }

    public override void FrameUpdate() {
        base.FrameUpdate();

        Vector2 lineOfSightDirection = enemy.forwardDirection;
        enemy.isAggroed = enemy.HasLineOfSight(lineOfSightDirection);

        if (enemy.isAggroed) {
            //Tween animation du saut
            enemy.transform.DOLocalJump(enemy.transform.position, 1f, 1, 0.5f).SetEase(Ease.InOutQuint);
            enemy.StateMachine.ChangeState(enemy.ChasingState);
        }

        ManageLooking();
    }

    // Il regarde dans les quatres directions dans le même ordre, mais peut être vaut il mieux randomiser un peu cela ?
    private void ManageLooking() {
        TimerLooking();
        
        if(!timerIsRunning) {
            switch (tmpLook) {
                case 0:
                    enemy.forwardDirection = Vector2.up;
                    break;
                case 1:
                    enemy.forwardDirection = Vector2.right;
                    break;
                case 2:
                    enemy.forwardDirection = Vector2.down;
                    break;
                case 3:
                    enemy.forwardDirection = Vector2.left;
                    break;
                case 4:
                    enemy.StateMachine.ChangeState(enemy.IdleState);
                    break;
            }

            timerIsRunning = true;
            timeRemaining = lookingDuration;
        }
    }

    public void TimerLooking() {
        if(timerIsRunning) {
            if(timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
            } else {
                tmpLook++;
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

}