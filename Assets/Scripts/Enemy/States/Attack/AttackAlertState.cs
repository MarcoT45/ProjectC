using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackAlertState : EnemyState {

    private Color originalColor;  
    private float timeAnimationRemaining;
    private bool timerIsRunning = false;

    public AttackAlertState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        enemy.isAttacking = true;
        enemy.attackType = EnemyAI.AttackType.Slash; // Le comportement du tir est un peu comme un slash

        // Pour simuler une animation d'attaque tant qu'on n'a pas d'animation
        originalColor = enemy.spriteRenderer.color;
        enemy.spriteRenderer.DOColor(Color.red, 1f).OnComplete(() => {
            enemy.spriteRenderer.DOColor(originalColor, 0.1f);
        });

        AlertAllEnemies();
        timeAnimationRemaining = 1f; // Il faudra mettre ici un script qui récupere le temps de l'animaion d'attaque de l'ennemi pour matcher
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
            enemy.SetTargetPosition(enemy.playerSearchPosition);
        }
    }

    public void AlertAllEnemies() {

        // Pour que l'ennemi se retourne vers le joueur si jamais il avait reculé
        // Ou alors on doit l'enlever ? Et l'ennemi peut sonner l'alerte en nous tournant le dos ?
        // Et donc si il sonne l'alerte, il s'expose aux coups ?
        Vector2 direction = enemy.player.position - enemy.transform.position;
        Vector2 sideVector = enemy.GetSideVectorFromDirection(direction);
        enemy.forwardDirection = sideVector.normalized;

        GameObject[] enemiesList = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject ene in enemiesList) {
            EnemyAI e = (EnemyAI) ene.GetComponent(typeof(EnemyAI));
            e.SetTargetPosition(enemy.playerSearchPosition);
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