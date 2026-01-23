using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackSlashState : EnemyState {

    private Color originalColor;
    private float timerAttackDuration = 0f;
    private bool attackIsDone = false;  
    private Vector2 targetPosition;

    public AttackSlashState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        enemy.isAttacking = true;
        enemy.attackType = EnemyAI.AttackType.Slash;
        timerAttackDuration = 0f;
        attackIsDone = false;

        originalColor = enemy.spriteRenderer.color;
        enemy.spriteRenderer.DOColor(Color.red, 0.5f).OnComplete(() => {
            enemy.spriteRenderer.DOColor(originalColor, 0.1f);
        });

        Vector2 direction = enemy.player.position - enemy.transform.position;

        Vector2 originPosition = enemy.transform.position;
        Vector2 sideVector = enemy.GetSideVectorFromDirection(direction);
        targetPosition = (Vector2)originPosition + sideVector.normalized * enemy.monsterData.portee;
        enemy.forwardDirection = sideVector.normalized;
    }

    public override void ExitState() {
        base.ExitState();

        enemy.isAttacking = false;
        enemy.attackType = EnemyAI.AttackType.Bump;
    }

    public override void FrameFixedUpdate() {
        base.FrameFixedUpdate();

        timerAttackDuration += Time.fixedDeltaTime;

        //Timer pour simuler l'animation d'attaque
        if (timerAttackDuration > 0.5f) {
            if (!attackIsDone) {
                AttackSlash();
                attackIsDone = true;
            }
        }

        //Timer de l'attaque / A CHANGER POUR UNE ANIMATION
        if (timerAttackDuration >= 1f) {
            timerAttackDuration = 0f;
            enemy.StateMachine.ChangeState(enemy.AlertedState);
        }
    }

    public void AttackSlash() {
        //Vérification de la collision entre l'ennemi et le joueur
        Collider2D[] hits = Physics2D.OverlapCircleAll(targetPosition, 0.5f);

        if (hits.Length > 0) {
            foreach (Collider2D hit in hits) {
                if (hit.CompareTag("Player")) {
                    PlayerController player = hit.GetComponent<PlayerController>();
                    BumpSystem.AttackEnemy(player, enemy);   
                }
            }
        }

        GameObject.Instantiate(enemy.slashVFXPrefab, targetPosition, Quaternion.identity);
    }

}