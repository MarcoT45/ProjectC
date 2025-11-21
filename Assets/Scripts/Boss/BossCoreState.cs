using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCoreState : EnemyState
{
    private float timer = 0f;
    private float delay = 2f;
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

        timer += Time.deltaTime;

        if (timer >= delay) {
            timer = 0f;
            enemyStateMachine.ChangeState(boss.AttackingState);
        }

        // Vérifie la transition de phase du boss à chaque frame
        boss.CheckPhaseTransition();

    }

    public override void AnnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType) {
        base.AnnimationTriggerEvent(triggerType);
    }
}
