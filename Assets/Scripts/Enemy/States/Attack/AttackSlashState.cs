using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSlashState : EnemyState {

    public AttackSlashState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
        base.FrameUpdate();

        GameObject impactEffect = GameObject.Instantiate(enemy.slashVFXPrefab, enemy.player.position, Quaternion.identity);
        GameObject.Destroy(impactEffect, 0.2f);

        PlayerController p = enemy.player.gameObject.GetComponent<PlayerController>();
        p.Damage(enemy.monsterData.atk);
        p.ApplyKnockback(enemy.forwardDirection);

        enemy.StateMachine.ChangeState(enemy.AlertedState);
    }

}