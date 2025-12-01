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
        Debug.Log("Boss Thrust State Entered");
        base.EnterState();
        // Récupérer la référence au Boss1
        boss = enemy as Boss1;

        if (boss == null)
        {
            Debug.LogError("BossThrustState: Boss1 component not found on the enemy GameObject.");
            enemyStateMachine.ChangeState(enemy.IdleState);
        }

        timer = 0f;

        // Démarrer animation de l'attaque de thrust
        this.animationHash = Animator.StringToHash("AttackThrust");
        boss.animator.SetInteger("StateID", animationHash);
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        timer += Time.deltaTime;
        if (timer >= boss.slamCone.windUpTime && boss.attackInProgress)
        {
            boss.slamCone.DoSlam();
            boss.attackInProgress = false;
        }

        //Vérifie la fin de l'animation
        var info = boss.animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("AttackThrust") && info.normalizedTime >= 1.0f)
        {
            enemyStateMachine.ChangeState(boss.IdleState);
        }

    }
    public override void ExitState()
    {
        base.ExitState();
        boss.attackInProgress = false;
        boss.ResetCooldown();
    }
}