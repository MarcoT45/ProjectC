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

        base.EnterState();

        // Récupérer la référence au Boss1
        boss = enemy as Boss1;
         

        if (boss == null)
        {
            Debug.LogError("BossAttackState: Boss1 component not found on the enemy GameObject.");
            enemyStateMachine.ChangeState(enemy.PatrolState);
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
            enemyStateMachine.ChangeState(boss.SlamState);
        }

        //Priorité 2 : Attaque en avant
        else if (distanceToPlayer > boss.coneMinDistance && distanceToPlayer <= boss.coneMaxDistance)
        {
            enemyStateMachine.ChangeState(boss.ThrustState);
        }
        else
        {
            //Priotrité 3 : Attaque de racines en 
            enemyStateMachine.ChangeState(boss.RootsState);
        }
    }

    public override void AnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
