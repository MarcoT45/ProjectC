using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeingState : EnemyState {

    public FleeingState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
        base.FrameUpdate();
    }

    public override void AnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }

}