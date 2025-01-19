using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;
using DG.Tweening;

public class OrcAttackingState : EnnemyState
{
    private GameObject target;
    private Vector3 targetPos;
    private Vector2 direction;

    private float duration = 1;
    private float timeRemaining;
    private bool isCountingDown = false;

    public OrcAttackingState(Ennemy ennemy, EnnemyStateMachine ennemyStateMachine) : base(ennemy, ennemyStateMachine)
    {

    }

    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Attacking");

        direction = Vector2.zero;
        //Changer par le player du GM, autre façon de faire avec le joueur comme direction
        target = GameObject.FindWithTag("Player");
        targetPos = target.transform.position;
        targetPos = ennemy.gridManager.WorldToCell(targetPos);

        Vector3 ennemyCellPos = ennemy.gridManager.WorldToCell(ennemy.transform.position);
        direction = (targetPos - ennemyCellPos).normalized;

        if(!isCountingDown)
        {
            isCountingDown = true;
            timeRemaining = duration;
        }

        //Tween animation de la couleur
        SpriteRenderer spriteRenderer = ennemy.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.DOColor(Color.red, 1f)
             .SetEase(Ease.OutQuart);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if(!isCountingDown)
        {
            Attack();
        }
        else
        {
            timeRemaining -= Time.deltaTime;
            if(timeRemaining <= 0f)
            {
                isCountingDown = false;
            }
        }
    }

    public override void AnnimationTriggerEvent(Ennemy.AnimationTriggerType triggerType)
    {
        base.AnnimationTriggerEvent(triggerType);
    }

    private void Attack()
    {

    }

/*    private void Attack(Vector2 direction)
    {

        ennemy.transform.position = Vector3.MoveTowards(ennemy.transform.position, ennemy.movePoint.position, ennemy.monsterData.speed * 3 * Time.deltaTime);

        if (Vector3.Distance(ennemy.transform.position, ennemy.movePoint.position) <= .05f)
        {
            ennemy.lastPosition = ennemy.gridManager.WorldToCell(ennemy.transform.position);
            //direction = ennemy.FindNextCell(direction, targetPos);
            //ennemy.MoveEnnemyMovePoint(direction);

            if (!ennemy.CanEnnemyMove(direction) || ennemy.isHurt)
            {

                ennemy.isHurt = false;
                Debug.Log("fin attack");
                SpriteRenderer spriteRenderer = ennemy.GetComponentInChildren<SpriteRenderer>();
                spriteRenderer.DOColor(Color.white, 1f)
                    .SetEase(Ease.OutQuart);
                ennemy.StateMachine.ChangeState(ennemy.ChasingState);
            }
        }

    }*/

}
