using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState {

    protected EnemyAI enemy;
    protected EnemyStateMachine enemyStateMachine;

    public EnemyState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void FrameFixedUpdate() { }
    public virtual void AnnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType) { }

}