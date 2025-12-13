using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackShootState : EnemyState {

    private GameObject projectile;
    private float timeAnimationRemaining;
    private bool timerIsRunning = false;

    public AttackShootState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        projectile = UnityEngine.Object.Instantiate(enemy.projectilePrefab, enemy.transform.position, Quaternion.identity);
        projectile.GetComponent<ProjectileEnemyRange>().Initialize(enemy.playerSeachPosition - enemy.transform.position);
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