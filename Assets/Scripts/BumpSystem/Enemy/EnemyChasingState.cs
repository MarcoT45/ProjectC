using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyState {

    private GameObject target;
    private Vector3 targetPosition;

    private float aggroDuration = 5f;
    private float timeRemaining;
    private bool timerIsRunning = false;

    public EnemyChasingState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy,enemyStateMachine) {}

    public override void EnterState() {
        base.EnterState();

        //Changer par le player du GM, autre façon de faire avec le joueur comme direction
        target = GameObject.FindWithTag("Player");
        targetPosition = target.transform.position;
        timeRemaining = aggroDuration;
    }

    public override void FrameUpdate() {
        base.FrameUpdate();

        ManageAggro(targetPosition);
        //Mouvement
        //enemy.Move(targetPosition);
    }

    private void ManageAggro(Vector3 playerCellPostion) {
        TimerAggro();
        Vector2 lineOfSightDirection = (enemy.player.position - enemy.transform.position).normalized;
        //enemy.isAlerted = enemy.HasLineOfSight(lineOfSightDirection);

        //Update de la position que si vision sur le joueur
        if(enemy.isAlerted) {
            targetPosition = target.transform.position;
        }

        if (enemy.isAlerted || timeRemaining > 0) {
            if (enemy.isAlerted && timerIsRunning) {
                timerIsRunning = false;
                timeRemaining = aggroDuration;
            } else {
                timerIsRunning = true;
            }
        }
    }

    public void TimerAggro() {
        if(timerIsRunning) {
            if(timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
            } else {
                //enemy.StateMachine.ChangeState(enemy.IdleState);
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    public override void AnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }

}