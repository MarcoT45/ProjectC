using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : EnemyState {

    private float alertDuration = 5f;
    private float timeAlertRemaining;
    private float attackCdRemaining;

    public ChasingState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        timeAlertRemaining = alertDuration;
        attackCdRemaining = enemy.attackCoolDown;

        enemy.speedBoostAlertMultiplicator = 1.75f;
        enemy.OnMoveDestinationReached += OnDestinationReached;

        enemy.bubbleSearch.SetActive(false);

        Debug.Log("ENTER CHASING STATE");
    }

    public override void ExitState() {
        base.ExitState();

        enemy.speedBoostAlertMultiplicator = 1.0f;
        enemy.OnMoveDestinationReached -= OnDestinationReached;
    }

    public override void FrameFixedUpdate() {
        base.FrameFixedUpdate();

        TimerAlert();
        TimerAttack();

        Vector2 lineOfSightDirection = (enemy.player.position - enemy.transform.position).normalized;
        enemy.isSearching = enemy.SearchLineOfSight(lineOfSightDirection);
        enemy.isAlerted = enemy.AlertLineOfSight(lineOfSightDirection);

        if (enemy.isSearching || enemy.isAlerted) {
            enemy.SetTargetPosition(enemy.playerSeachPosition);
            timeAlertRemaining = alertDuration;
        }

        // Juste pour le test je remplace ce isAlerted par isSearching
        if (enemy.isSearching && Vector3.Distance(enemy.transform.position, enemy.playerSeachPosition) <= enemy.monsterData.portee) {
            enemy.rb.velocity = Vector2.zero;
            if(attackCdRemaining < 0) {
                enemy.StateMachine.ChangeState(enemy.AttackState);
            }
        } else {
            enemy.Move();
        }

        if (enemy.isKnockedBack) {
            enemy.StateMachine.ChangeState(enemy.StunState);
        }
    }

    public void TimerAlert() {
        if(timeAlertRemaining > 0) {
            timeAlertRemaining -= Time.deltaTime;
        } else {
            enemy.StateMachine.ChangeState(enemy.PatrolState);
        }
    }

    public void TimerAttack() {
        if(attackCdRemaining > 0) {
            attackCdRemaining -= Time.deltaTime;
        }
    }

    private void OnDestinationReached() {
        if (!enemy.isSearching || !enemy.isAlerted) {
            enemy.StateMachine.ChangeState(enemy.AlertedLookingState);
        }
    }

}