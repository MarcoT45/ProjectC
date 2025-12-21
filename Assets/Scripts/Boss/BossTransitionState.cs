using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTransitionState : EnemyState
{
    private Boss1 boss;

    public BossTransitionState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void EnterState()
    {
        base.EnterState();
        // Récupérer la référence au Boss1
        boss = enemy as Boss1;

        if (boss == null)
        {
            Debug.LogError("BossSlamState: Boss1 component not found on the enemy GameObject.");
            enemyStateMachine.ChangeState(enemy.PatrolIdleState);
        }


        // Démarrer animation de transition
        stateID = StateID.Transition;
        boss.animator.SetInteger("StateID", (int)stateID);
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();

        //Vérifie la fin de l'animation
        var info = boss.animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("PhaseTransition") && info.normalizedTime >= 1.0f)
        {
            enemyStateMachine.ChangeState(boss.IdleP2State);

            boss.isInPhaseTwo = true;
            boss.isInvincible = true;
        }

    }
    public override void ExitState()
    {
        base.ExitState();
        boss.attackInProgress = false;
        boss.ResetCooldown();
    }
}
