using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBumpState : EnemyState
{
    private bool isAttackComplete = false;

    public AttackBumpState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void EnterState()
    {
        base.EnterState();

        isAttackComplete = false;
        enemy.isInvincible = true;
        enemy.isAttacking = true;

        AttackBump();

    }

    public override void ExitState()
    {
        base.ExitState();

        enemy.isInvincible = false;
        enemy.isAttacking = false;
    }

    public override void FrameFixedUpdate()
    {
        base.FrameFixedUpdate();

        if (enemy.isSearching || enemy.isAlerted)
        {
            enemy.SetTargetPosition(enemy.playerSeachPosition);
        }

        if (isAttackComplete)
        {
            enemy.StateMachine.ChangeState(enemy.AlertedState);
        }
    }

    public async void AttackBump()
    {
        Vector2 direction = enemy.player.position - enemy.transform.position;

        Vector2 originPosition = enemy.transform.position;
        Vector2 sideVector = enemy.GetSideVectorFromDirection(direction);
        Vector2 targetPosition = (Vector2)originPosition + sideVector.normalized * enemy.monsterData.portee;
        
        await enemy.transform.DOMove(targetPosition, 0.5f).SetEase(Ease.InOutQuint).AsyncWaitForCompletion();
        enemy.isInvincible = false;
        await enemy.transform.DOMove(originPosition, 0.2f).SetEase(Ease.Linear).AsyncWaitForCompletion();

        isAttackComplete = true;
    
    }

}
