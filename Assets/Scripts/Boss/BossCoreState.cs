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

        Debug.Log("Boss Core State Entered");

        // Démarrer animation de l'attaque de slam
        this.animationHash = Animator.StringToHash("Idle");
        boss.animator.SetInteger("StateID", animationHash);
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

    public override void AnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }
}
