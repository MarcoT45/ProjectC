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

        boss.attackInProgress = true;
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
            enemyStateMachine.ChangeState(boss.SlamState);
        }

        //Priorité 2 : Attaque en avant
        else if (distanceToPlayer > boss.coneMinDistance && distanceToPlayer <= boss.coneMaxDistance)
        {
            Debug.Log("Performing Cone Slam Attack " + distanceToPlayer);
            enemyStateMachine.ChangeState(boss.ThrustState);
        }
        else
        {
            //Priotrité 3 : Attaque de racines en 
            Debug.Log("Root Attack ");
            enemyStateMachine.ChangeState(boss.RootsState);
        }
    }

    public override void AnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
