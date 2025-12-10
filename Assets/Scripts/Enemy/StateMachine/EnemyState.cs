using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState {

    public enum StateID {
        Idle = 0,         //Etats P1 0+
        Attacking = 10,
        Slam = 11,
        Thrust = 12,
        Roots = 13,
        Staggered = 20,
        Transition = 30,
        Dead = 40,
        IdleP2 = 100,     //Etats Phase2 100+
        AttackingP2 = 110,
        SlamP2 = 111,
        ThrustP2 = 112,
        RootsP2 = 113,
        StaggeredP2 = 120,
        TransitionP2 = 130,
        DeadP2 = 140,
    }

    protected EnemyAI enemy;
    protected EnemyStateMachine enemyStateMachine;
    protected StateID stateID;

    public EnemyState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void FrameFixedUpdate() { }
    public virtual void AnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType) { }

}