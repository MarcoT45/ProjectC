using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AttackSlashState : EnemyState {

    private bool firstFrame = true; // UNIQUEMENT pour l'exemple
    private GameObject impactEffect; // UNIQUEMENT pour l'exemple ou instancie un Slash
    private float timer;
    private bool timerIsRunning = false;

    public AttackSlashState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        //impactEffect = GameObject.Instantiate(enemy.slashVFXPrefab, enemy.player.position, Quaternion.identity); // A ENLEVER PLUS TARD
        timer = 0f; // Il faudra mettre ici un script qui récupere le temps de l'animaion d'attaque de l'ennemi pour matcher
        timerIsRunning = true;
        enemy.isAttacking = true;

        enemy.gameObject.GetComponent<SpriteRenderer>().color = Color.red; // A ENLEVER PLUS TARD
        firstFrame = true;// A ENLEVER PLUS TARD
    }

    public override void ExitState() {
        base.ExitState();
        enemy.isAttacking = false;
        enemy.isInvincible = false;

        enemy.gameObject.GetComponent<SpriteRenderer>().color = Color.white; // A ENLEVER PLUS TARD
        firstFrame = false;// A ENLEVER PLUS TARD

    }

    public override void FrameFixedUpdate() {
        base.FrameFixedUpdate();

        AttackBump();

        Vector2 lineOfSightDirection = (enemy.player.position - enemy.transform.position).normalized;
        enemy.isSearching = enemy.SearchLineOfSight(lineOfSightDirection);
        enemy.isAlerted = enemy.AlertLineOfSight(lineOfSightDirection);

        if (enemy.isSearching || enemy.isAlerted) {
            enemy.SetTargetPosition(enemy.playerSeachPosition);
        }
    }

    //SLASH TEST
    public void AttackSlash() {
        if (timerIsRunning) {
            timer += Time.deltaTime;
            if(timer >= 0.5f && enemy.isAttacking) {
                Vector2 direction = (enemy.player.position - enemy.transform.position).normalized;
                Vector2 targetPosition = (Vector2)enemy.transform.position + direction * 1.5f; // Distance d'approche avant l'attaque
                
                if(firstFrame)
                {
                    impactEffect = GameObject.Instantiate(enemy.slashVFXPrefab, targetPosition, Quaternion.identity); // A ENLEVER PLUS TARD
                    firstFrame = false;

                    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(targetPosition, 1f);
                    foreach (var hitCollider in hitColliders)
                    {
                        PlayerController player = hitCollider.GetComponent<PlayerController>();
                        if (player != null)
                        {
                            player.Damage(enemy.monsterData.atk);
                            player.ApplyKnockback((player.transform.position - enemy.transform.position).normalized);
                        }
                    }
                }

            }

            // Fin de l'animation d'attaque / A REVOIR POUR MATCHER L'ANIMATION
            if (timer >= 1.5f && enemy.isAttacking)
            {
                enemy.StateMachine.ChangeState(enemy.AlertedState);
            }
        }
    }

    //BUMP TEST
    public void AttackBump() {
        Vector2 direction = enemy.player.position - enemy.transform.position;

        Vector2 originPosition = enemy.transform.position;
        Vector2 sideVector = enemy.GetSideVectorFromDirection(direction);
        Vector2 targetPosition = (Vector2)originPosition + sideVector.normalized * enemy.monsterData.portee;

        enemy.isInvincible = true;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(enemy.transform.DOMove(targetPosition, 0.5f).SetEase(Ease.InOutQuint))
                    .Append(enemy.transform.DOMove(originPosition, 0.2f).SetEase(Ease.Linear))
                    .OnComplete(() => enemy.StateMachine.ChangeState(enemy.AlertedState));
    }
}