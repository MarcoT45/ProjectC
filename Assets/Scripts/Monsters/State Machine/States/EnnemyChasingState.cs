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
        targetPos = ennemy.gridManager.WorldToCell(targetPos);

        timeRemaining = aggroDuration;

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        Vector3Int ennemyCellPos = ennemy.gridManager.WorldToCell(ennemy.transform.position);


        // ---Mouvement du movePoint et de l'ennemi 
        ennemy.MoveEnnemy();

        if (Vector3.Distance(ennemy.transform.position, ennemy.gridManager.CellToWorld(ennemy.newCellTarget)) <= .05f)
        {
            if (Time.time > ennemy.LastUsedTimeMove + ennemy.coolDownMove)
            {
                ennemy.lastPosition = ennemy.gridManager.WorldToCell(ennemy.transform.position);
                direction = ennemy.FindNextCell(direction, targetPos);

                Vector3 nextPosition = ennemy.transform.position + (Vector3)direction;
                Vector3Int gridNextPosition = ennemy.gridManager.WorldToCell(nextPosition);

                ennemy.newCellTarget = gridNextPosition;

                //changement de place sur la grid du gridManager
                var currentCell = ennemy.gridManager.GetCellData(ennemy.lastPosition);
                currentCell.containedInCell = null;

                var targetCell = ennemy.gridManager.GetCellData(ennemy.newCellTarget);
                targetCell.containedInCell = ennemy.gameObject;

                //Debug
                ennemy.movePoint.transform.position = ennemy.gridManager.CellToWorld(ennemy.newCellTarget);
                //-----

                UnityEngine.Object.Instantiate(ennemy.stepVFXPrefab, ennemy.gridManager.CellToWorld(ennemy.lastPosition), Quaternion.identity);

                ennemy.LastUsedTimeMove = Time.time;
            }
        }
        // ----

        // ---Debug 
        Vector3 targetTmp = target.transform.position;
        targetTmp = ennemy.gridManager.WorldToCell(targetTmp);
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

            if(targetPos != ennemy.gridManager.WorldToCell(target.transform.position))
            {
                /*Debug.Log("Reset target");*/
                targetPos = target.transform.position;
                targetPos = ennemy.gridManager.WorldToCell(targetPos);
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
                ennemy.StateMachine.ChangeState(ennemy.IdleState);
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }
}
