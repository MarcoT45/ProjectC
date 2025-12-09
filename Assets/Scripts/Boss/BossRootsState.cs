using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRootsState : EnemyState
{
    private Boss1 boss;
    private float timer;

    public BossRootsState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void EnterState()
    {
        base.EnterState();
        // Récupérer la référence au Boss1
        boss = enemy as Boss1;

        if (boss == null)
        {
            Debug.LogError("BossRootsState: Boss1 component not found on the enemy GameObject.");
            enemyStateMachine.ChangeState(enemy.PatrolIdleState);
        }

        timer = 0f;

        // Démarrer animation de l'attaque
        stateID = StateID.Roots;
        boss.animator.SetInteger("StateID", (int)stateID);
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        timer += Time.deltaTime;
        if (timer >= boss.slamCone.windUpTime && boss.attackInProgress)
        {
            boss.rootAttack.PerformAttack();
            boss.attackInProgress = false;
        }

        //Vérifie la fin de l'animation
        var info = boss.animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("AttackRoots") && info.normalizedTime >= 1.0f)
        {
            enemyStateMachine.ChangeState(boss.PatrolIdleState);
        }

    }
    public override void ExitState()
    {
        base.ExitState();
        boss.attackInProgress = false;
        boss.ResetCooldown();
    }
}