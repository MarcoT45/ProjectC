using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCoreState : EnemyState
{
    private Boss1 boss;

    public BossCoreState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine){}

    public override void EnterState() {
        base.EnterState();

        boss = enemy.gameObject.GetComponent<Boss1>();

        Debug.Log("Boss Core State Entered");
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
        base.FrameUpdate();

        // Vérifie la transition de phase du boss à chaque frame
        boss.CheckPhaseTransition();

        // Vérifie si le boss peut attaquer
        if (boss.IsCooldownComplete()) {
            boss.StateMachine.ChangeState(boss.AttackingState);
        }

    }

    public override void AnnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType) {
        base.AnnimationTriggerEvent(triggerType);
    }
}
