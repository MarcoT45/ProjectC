using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackBumpState : EnemyState {
    
    private Color originalColor;
    private bool isAttackComplete = false;

    public AttackBumpState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void EnterState() {
        base.EnterState();

        isAttackComplete = false;
        enemy.isInvincible = true;
        enemy.isAttacking = true;

        originalColor = enemy.spriteRenderer.color;
        enemy.spriteRenderer.DOColor(Color.red, 0.5f).OnComplete(() => {
            enemy.spriteRenderer.DOColor(originalColor, 0.1f);
        });

        AttackBump();
    }

    public override void ExitState() {
        base.ExitState();

        enemy.isInvincible = false;
        enemy.isAttacking = false;
    }

    public override void FrameFixedUpdate() {
        base.FrameFixedUpdate();

        if (enemy.isKnockedBack && !enemy.isAttacking) {
            enemy.transform.DOKill();
            enemy.StateMachine.ChangeState(enemy.StunState);
        }

        if (isAttackComplete) {
            enemy.StateMachine.ChangeState(enemy.AlertedState);
        }
    }

    public async void AttackBump() {
        Vector2 direction = enemy.player.position - enemy.transform.position;

        Vector2 originPosition = enemy.transform.position;
        Vector2 sideVector = enemy.GetSideVectorFromDirection(direction);
        Vector2 targetPosition = (Vector2)originPosition + sideVector.normalized * enemy.monsterData.portee;
        enemy.forwardDirection = sideVector.normalized; 

        await enemy.rb.DOMove(targetPosition, 0.4f).SetEase(Ease.InOutQuint).AsyncWaitForCompletion();
        enemy.isInvincible = false;
        enemy.isAttacking = false;
        await enemy.rb.DOMove(originPosition, 0.5f).SetEase(Ease.Linear).AsyncWaitForCompletion();

        isAttackComplete = true;
    }

}