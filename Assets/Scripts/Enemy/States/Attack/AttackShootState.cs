using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackShootState : EnemyState {

    private Color originalColor;  
    private GameObject projectile;
    private float timeAnimationRemaining;
    private bool timerIsRunning = false;

    public AttackShootState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        enemy.isAttacking = true;
        enemy.attackType = EnemyAI.AttackType.Slash; // Le comportement du tir est un peu comme un slash

        // Pour simuler une animation d'attaque tant qu'on n'a pas d'animation
        originalColor = enemy.spriteRenderer.color;
        enemy.spriteRenderer.DOColor(Color.red, 0.5f).OnComplete(() => {
            enemy.spriteRenderer.DOColor(originalColor, 0.1f);
        });

        projectile = UnityEngine.Object.Instantiate(enemy.projectilePrefab, enemy.transform.position, Quaternion.identity);
        projectile.GetComponent<ProjectileEnemyRange>().Initialize(enemy.playerSeachPosition - enemy.transform.position);
        timeAnimationRemaining = 0.5f; // Il faudra mettre ici un script qui récupere le temps de l'animation d'attaque de l'ennemi pour matcher
        timerIsRunning = true;
    }

    public override void ExitState() {
        base.ExitState();

        enemy.isAttacking = false;
        enemy.attackType = EnemyAI.AttackType.Bump;
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