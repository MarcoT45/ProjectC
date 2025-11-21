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

        // Récupérer la référence au Boss1&
       // boss = enemy.gameObject.GetComponent<Boss1>(); 
       boss = enemy as Boss1;
        Debug.Log("boss component retrieved: " + boss.name);

        if (boss == null)
        {
            Debug.LogError("BossAttackState: Boss1 component not found on the enemy GameObject.");
            enemyStateMachine.ChangeState(enemy.IdleState);
        }

        // Vérifier si une attaque est déjà en cours
        if (boss.attackInProgress)
        {
            Debug.Log("Attack already in progress, returning to Idle State.");
            enemyStateMachine.ChangeState(enemy.IdleState);
        }

        boss.StartCoroutine(boss.RootAttack());

    }


    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void AnnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType)
    {
        base.AnnimationTriggerEvent(triggerType);
    }
}
