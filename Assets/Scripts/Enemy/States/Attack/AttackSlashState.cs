using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSlashState : EnemyState {

    private GameObject impactEffect; // UNIQUEMENT pour l'exemple ou instancie un Slash
    private float timeAnimationRemaining;
    private bool timerIsRunning = false;

    public AttackSlashState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        impactEffect = GameObject.Instantiate(enemy.slashVFXPrefab, enemy.player.position, Quaternion.identity); // A ENLEVER PLUS TARD
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
                GameObject.Destroy(impactEffect); // On détruit le gameobject Slash MAIS il faudra enlever/remplacer en fonction de l'animation
                enemy.StateMachine.ChangeState(enemy.AlertedState);
            }
        }
    }

}