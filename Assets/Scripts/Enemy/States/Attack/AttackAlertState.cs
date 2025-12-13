using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAlertState : EnemyState {

    private float timeAnimationRemaining;
    private bool timerIsRunning = false;

    public AttackAlertState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        AlertAllEnemies();
        timeAnimationRemaining = 1f; // Il faudra mettre ici un script qui récupere le temps de l'animaion d'attaque de l'ennemi pour matcher
        timerIsRunning = true;
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
        base.FrameUpdate();

        TimerAnimationAttack();

        Vector2 lineOfSightDirection = (enemy.player.position - enemy.transform.position).normalized;
        enemy.isSearching = enemy.SearchLineOfSight(lineOfSightDirection);
        enemy.isAlerted = enemy.AlertLineOfSight(lineOfSightDirection);

        if (enemy.isSearching || enemy.isAlerted) {
            enemy.SetTargetPosition(enemy.playerSeachPosition);
        }
    }

    public void AlertAllEnemies() {
        GameObject[] enemiesList = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject ene in enemiesList) {
            EnemyAI e = (EnemyAI) ene.GetComponent(typeof(EnemyAI));
            e.SetTargetPosition(enemy.playerSeachPosition);
        }
    }

    public void TimerAnimationAttack() {
        if (timerIsRunning) {
            if (timeAnimationRemaining > 0) {
                timeAnimationRemaining -= Time.deltaTime;
            } else {
                enemy.StateMachine.ChangeState(enemy.AlertedState);
            }
        }
    }

}