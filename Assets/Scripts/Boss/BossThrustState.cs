using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThrustState : EnemyState
{
    private Boss1 boss;
    private float timer;

    public BossThrustState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void EnterState()
    {
        base.EnterState();
        // Récupérer la référence au Boss1
        boss = enemy as Boss1;

        if (boss == null)
        {
            Debug.LogError("BossThrustState: Boss1 component not found on the enemy GameObject.");
            enemyStateMachine.ChangeState(enemy.PatrolState);
        }

        timer = 0f;

        // Démarrer animation de l'attaque de thrust
        stateID = StateID.Thrust;
        boss.animator.SetInteger("StateID", (int)stateID);
        boss.attackInProgress = true;
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        timer += Time.deltaTime;
        if (timer >= boss.slamCone.windUpTime && boss.attackInProgress)
        {
            boss.slamCone.DoSlam();
        }

        //Vérifie la fin de l'animation
        var info = boss.animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("AttackThrust") && info.normalizedTime >= 1.0f)
        {
            enemyStateMachine.ChangeState(boss.PatrolState);
        }

    }
    public override void ExitState()
    {
        base.ExitState();
        boss.attackInProgress = false;
        boss.ResetCooldown();
    }
}
public class BossLaserState : EnemyState
{
    private Boss1 boss;
    private float timer;

    public BossLaserState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void EnterState()
    {
        base.EnterState();
        // Récupérer la référence au Boss1
        boss = enemy as Boss1;

        if (boss == null)
        {
            Debug.LogError("BossLaserState: Boss1 component not found on the enemy GameObject.");
            enemyStateMachine.ChangeState(enemy.PatrolState);
        }

        timer = 0f;
        Debug.Log("LASER STATE ENTERED");
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        timer += Time.deltaTime;

        if (boss.attackInProgress && !boss.laserAttack.IsRunning)
        {
            bool left = boss.laserAttack.SidePlayer(boss.player);
            boss.laserAttack.ActivateLaser(left);
        }

        if( boss.laserAttack.IsSweepComplete())
        {
            Debug.Log("LASER SWEEP COMPLETE");
            boss.laserAttack.StopLaser();
            enemyStateMachine.ChangeState(boss.IdleP2State);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        boss.attackInProgress = false;
        boss.ResetCooldown();
    }
}