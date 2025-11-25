using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState: EnemyState
{
    private Boss1 boss;

    public BossAttackState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }


    public override void EnterState()
    {
        Debug.Log("Boss Attack State Entered");

        base.EnterState();

        // Récupérer la référence au Boss1
        boss = enemy as Boss1;

        if (boss == null)
        {
            Debug.LogError("BossAttackState: Boss1 component not found on the enemy GameObject.");
            enemyStateMachine.ChangeState(enemy.IdleState);
        }

    }


    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (!boss.IsCooldownComplete())return;

        float distanceToPlayer = boss.GetPlayerDistance();

        //Priorité 1 : Attaque de proximité en cercle
        if (distanceToPlayer <= boss.circleDistance)
        {
            Debug.Log("Performing Circle Slam Attack " + distanceToPlayer);
            boss.slamCircle.PerformSlam(() =>
            {
                // Callback une fois l'attaque terminée
                boss.ResetCooldown();
                enemyStateMachine.ChangeState(enemy.IdleState);
            });
        }

        //Priorité 2 : Attaque en cône
        if (distanceToPlayer > boss.coneMinDistance && distanceToPlayer <= boss.coneMaxDistance)
        {
            Debug.Log("Performing Cone Slam Attack " + distanceToPlayer);
            boss.slamCone.PerformSlam(() =>
            {
                // Callback une fois l'attaque terminée
                boss.ResetCooldown();
                enemyStateMachine.ChangeState(enemy.IdleState);
            });
        }

        //Priotrité 3 : Attaque de racines en 
        boss.rootAttack.PerformAttack(() =>
        {
            // Callback une fois l'attaque terminée
            Debug.Log("Root Attack Finished");
            boss.ResetCooldown();
            enemyStateMachine.ChangeState(enemy.IdleState);
        });
    }

    public override void AnnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType)
    {
        base.AnnimationTriggerEvent(triggerType);
    }
}
