using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : EnemyState
{
    private float playerAttackTimer;
    public StunState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("ENTER STUN STATE");
        playerAttackTimer = 0f;

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameFixedUpdate()
    {
        base.FrameFixedUpdate();

        playerAttackTimer += Time.fixedDeltaTime;

        Collider2D[] hits = Physics2D.OverlapCircleAll(enemy.rb.position, 0.3f);

        if(hits.Length > 0 ) {
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    PlayerController player = hit.GetComponent<PlayerController>();

                    if(playerAttackTimer >= player.coolDownAttack )
                    {
                        playerAttackTimer = 0f;

                        Debug.Log("STUN STATE BUMP PLAYER");
                        BumpSystem.HandleBump(player, enemy);

                    }
                }
            }
        }

        if (!enemy.isKnockedBack)
        {
            enemy.StateMachine.ChangeState(enemy.PatrolState);
        }
    }

}
