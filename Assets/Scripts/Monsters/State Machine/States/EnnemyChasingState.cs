using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyChasingState : EnnemyState
{
    private GameObject target;
    private Vector3 targetPos;
    private Vector2 direction;

    private float aggroDuration = 5f;
    private float timeRemaining;
    private bool timerIsRunning = false;
    private float aggroRange = 4f;

    public EnnemyChasingState(Ennemy ennemy, EnnemyStateMachine ennemyStateMachine) : base(ennemy,ennemyStateMachine)
    {

    }

    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Chasing");

        direction = Vector2.zero;
        //Changer par le player du GM, autre façon de faire avec le joueur comme direction
        target = GameObject.FindWithTag("Player");
        targetPos = target.transform.position;
        targetPos = ennemy.solTileMap.WorldToCell(targetPos);

        timeRemaining = aggroDuration;

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        Vector3 ennemyCellPos = ennemy.solTileMap.WorldToCell(ennemy.transform.position);


        // ---Mouvement du movePoint et de l'ennemi 
        ennemy.MoveEnnemy();

        if (Vector3.Distance(ennemy.transform.position, ennemy.movePoint.position) <= .05f)
        {
            ennemy.lastPosition = ennemy.solTileMap.WorldToCell(ennemy.transform.position);
            direction = ennemy.FindNextCell(direction, targetPos);
            ennemy.MoveEnnemyMovePoint(direction);
        }
        // ----

        // ---Debug 
        Vector3 targetTmp = target.transform.position;
        targetTmp = ennemy.solTileMap.WorldToCell(targetTmp);
        Debug.DrawRay(
                  start: ennemy.transform.position,
                  dir: targetPos - ennemyCellPos,
                  color: Color.white);
        // ----

        TimerAggro();
        ennemy.isAggroed = ennemy.CheckAggro((targetTmp - ennemyCellPos).normalized, aggroRange);

        if (ennemy.isAggroed || timeRemaining > 0)
        {
            if (ennemy.isAggroed && timerIsRunning)
            {
                timerIsRunning = false;
                timeRemaining = aggroDuration;
            }
            else
            {
                timerIsRunning = true;
            }

            if(targetPos != ennemy.solTileMap.WorldToCell(target.transform.position))
            {
                /*Debug.Log("Reset target");*/
                targetPos = target.transform.position;
                targetPos = ennemy.solTileMap.WorldToCell(targetPos);
            }
        }

        /* if (Vector3.Distance(ennemyCellPos, targetPos) <= .05f)
         {
             Debug.Log("Direction " + (targetTmp - ennemyCellPos).normalized); 
             ennemy.isAggroed = ennemy.CheckAggro((targetTmp - ennemyCellPos).normalized);
             if (ennemy.isAggroed)
             {
                 Debug.Log("Entre");
                 targetPos = target.transform.position;
                 targetPos = ennemy.solTileMap.WorldToCell(targetPos);
             }
             else
             {
                 ennemy.stateMachine.ChangeState(ennemy.idleState);
             }
         }*/

    }


    public override void AnnimationTriggerEvent(Ennemy.AnimationTriggerType triggerType)
    {
        base.AnnimationTriggerEvent(triggerType);
    }

    public void TimerAggro()
    {
        if(timerIsRunning)
        {
            if(timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Fin");
                ennemy.stateMachine.ChangeState(ennemy.idleState);
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }
}
