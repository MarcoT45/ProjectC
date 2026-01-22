using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StunState : EnemyState
{
    private float playerAttackTimer;
    private Vector2 stunDirection;
    private float stunTimer;
    private Vector2 oldPosition;
    public StunState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void EnterState()
    {
        base.EnterState();
        playerAttackTimer = 0f;
        stunTimer = 0f;
        stunDirection = Vector2.zero;
        oldPosition = enemy.rb.position;    
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameFixedUpdate()
    {
        base.FrameFixedUpdate();

        playerAttackTimer += Time.fixedDeltaTime;
        stunTimer += Time.fixedDeltaTime;

        //Vérification de la collision entre l'ennemi et le joueur
        Collider2D[] hits = Physics2D.OverlapCircleAll(enemy.rb.position, 0.3f);

        if(hits.Length > 0 ) {
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    PlayerController player = hit.GetComponent<PlayerController>();

                    //Gestion du cooldown entre chaque attaque du joueur
                    if (playerAttackTimer >= player.coolDownAttack)
                    {
                        playerAttackTimer = 0f;

                        stunTimer = 0f; 
                        BumpSystem.HandleBump(player, enemy);

                        //Cas Particulier ou l'ennemi reste bloque
                        if ((oldPosition.x - enemy.rb.position.x < 0.05f && oldPosition.x - enemy.rb.position.x > -0.05f) &&
                           (oldPosition.y - enemy.rb.position.y < 0.05f && oldPosition.y - enemy.rb.position.y > -0.05f))
                        {
                            stunDirection = (enemy.transform.position - player.transform.position).normalized;
                            stunDirection = enemy.GetSideVectorFromDirection(stunDirection);
                            enemy.forwardDirection = -stunDirection;
                            enemy.isKnockedBack = false;

                            BumpSystem.HandleBump(player, enemy);

                            enemy.isCollidingWall = false;
                        }
                        else
                        {
                            oldPosition = enemy.rb.position;
                        }
                    }

                    //Lorsque l'ennemi est bloqué contre un mur
                    if (enemy.isCollidingWall)
                    {
                        Debug.Log("Enemy on Wall - Bump Again");
                        stunDirection = (enemy.transform.position - player.transform.position).normalized;
                        stunDirection = enemy.GetSideVectorFromDirection(stunDirection);
                        enemy.forwardDirection = -stunDirection;
                        enemy.isKnockedBack = false;

                        BumpSystem.HandleBump(player, enemy);

                        enemy.isCollidingWall = false;
                    }
                }
            }
        }

        // Si le timer de stun dépasse la durée de knockback, on enlève l'état de knockback
        if (stunTimer >= enemy.knockbackDuration)
        {
            enemy.isKnockedBack = false;
        }

        // Si l'ennemi n'est plus en knockback, on change d'état
        if (!enemy.isKnockedBack)
        {
            enemy.StateMachine.ChangeState(enemy.AlertedState);
        }

    }

}
