using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCoreState : EnemyState
{
    private Boss1 boss;

    public BossCoreState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine){}

    public override void EnterState() {
        base.EnterState();

        // Récupérer la référence au Boss1
        boss = enemy as Boss1;

        // Démarrer animation idle
        stateID = StateID.Idle;
        boss.animator.SetInteger("StateID", (int)stateID);
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
        base.FrameUpdate();

        // Vérifie la transition de phase du boss à chaque frame
        if (!boss.CheckPhaseTransition())
        {
            boss.StateMachine.ChangeState(boss.TransitionState);
        }

        // Vérifie si le boss peut attaquer
        if (boss.IsCooldownComplete())
        {
            boss.StateMachine.ChangeState(boss.AttackState);
        }

    }

    public override void AnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }
}
