using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlamState : EnemyState
{
    private Boss1 boss;
    private float timer;

    public BossSlamState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void EnterState()
    {
        base.EnterState();
        // Récupérer la référence au Boss1
        boss = enemy as Boss1;
         
        if (boss == null)
        {
            Debug.LogError("BossSlamState: Boss1 component not found on the enemy GameObject.");
            enemyStateMachine.ChangeState(enemy.PatrolState);
        }

        timer = 0f;

        // Démarrer animation de l'attaque de slam
        stateID = StateID.Slam;
        boss.animator.SetInteger("StateID", (int)stateID);
        boss.attackInProgress = true;
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        timer += Time.deltaTime;
        if (timer >= boss.slamCircle.windUpTime && boss.attackInProgress)
        {
            boss.slamCircle.DoSlam();
        }

        //Vérifie la fin de l'animation
        var info = boss.animator.GetCurrentAnimatorStateInfo(0);
        if(info.IsName("AttackSlam") && info.normalizedTime >= 1.0f)
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
