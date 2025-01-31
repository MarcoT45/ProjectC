using DG.Tweening;
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

    private float attackCoolDown = 2f;
    private float attacKTimeRemaining;
    private bool attackKTimeIsRuning = false;

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

        attacKTimeRemaining = attackCoolDown;
        attackKTimeIsRuning = true;

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        Vector3Int ennemyCellPos = ennemy.gridManager.WorldToCell(ennemy.transform.position);

        ManageMovement();

        Vector3 targetTmp = target.transform.position;
        targetTmp = ennemy.gridManager.WorldToCell(targetTmp);

        ManageAggro(targetTmp, ennemyCellPos);

    }
    private void ManageAggro(Vector3 playerCellPostion, Vector3 enemyCellPosition)
    {
        TimerAggro();
        ennemy.isAggroed = ennemy.CheckAggro((playerCellPostion - enemyCellPosition).normalized, aggroRange);

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

            if (targetPos != ennemy.gridManager.WorldToCell(target.transform.position))
            {
                /*Debug.Log("Reset target");*/
                targetPos = target.transform.position;
                targetPos = ennemy.gridManager.WorldToCell(targetPos);
            }
        }
    }


    // ---Mouvement du movePoint et de l'ennemi 
    public void ManageMovement()
    {
        GridManager gridManager = ennemy.gridManager;
        ennemy.MoveEnnemy();

        if (Vector3.Distance(ennemy.transform.position, gridManager.CellToWorld(ennemy.newCellTarget)) <= .05f)
        {
            direction = ennemy.FindNextCell(direction, targetPos);
            Vector3 nextPosition = ennemy.transform.position + (Vector3)direction;
            Vector3Int gridNextPosition = gridManager.WorldToCell(nextPosition);

            if (gridManager.CanMoveOnCell(gridNextPosition))
            {
                GridManager.CellData cellData = gridManager.GetCellData(gridNextPosition);

                if (gridManager.IsObjectOnCell(gridNextPosition))
                {
                    if (cellData.containedInCell.tag == "Player")
                    {
                        //Attaque

                        //ennemy.StateMachine.ChangeState(ennemy.AttackingState);

                        if (!attackKTimeIsRuning)
                        {
                            UnityEngine.Object.Instantiate(ennemy.slashVFXPrefab, ennemy.gridManager.CellToWorld(gridNextPosition), Quaternion.identity);
                            attacKTimeRemaining = attackCoolDown;
                            attackKTimeIsRuning = true;

                            CharacterController player = cellData.containedInCell.GetComponent<CharacterController>();

                            if (player != null)
                            {
                                player.OnDamage();
                            }

                            Camera camera = Camera.main;
                            camera.DOShakePosition(0.2f, 0.1f, 5, 90, true, ShakeRandomnessMode.Harmonic);

                        }
                        else
                        {
                            attacKTimeRemaining -= Time.deltaTime;
                            if (attacKTimeRemaining <= 0f)
                            {
                                attackKTimeIsRuning = false;
                            }
                        }
                    }
                }
                else
                {
                    //Move

                    if (Time.time > ennemy.LastUsedTimeMove + ennemy.coolDownMove)
                    {

                        ennemy.lastPosition = ennemy.gridManager.WorldToCell(ennemy.transform.position);

                        ennemy.newCellTarget = gridNextPosition;

                        //changement de place sur la grid du gridManager
                        var currentCell = ennemy.gridManager.GetCellData(ennemy.lastPosition);
                        currentCell.containedInCell = null;

                        var targetCell = ennemy.gridManager.GetCellData(ennemy.newCellTarget);
                        targetCell.containedInCell = ennemy.gameObject;

                        UnityEngine.Object.Instantiate(ennemy.stepVFXPrefab, ennemy.gridManager.CellToWorld(ennemy.lastPosition), Quaternion.identity);

                        //Debug
                        //ennemy.gridManager.DebugCellWithObjects();
                        ennemy.movePoint.transform.position = ennemy.gridManager.CellToWorld(ennemy.newCellTarget);
                        //------

                        ennemy.LastUsedTimeMove = Time.time;
                    }
                }
            }

        }
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
    public override void AnnimationTriggerEvent(Ennemy.AnimationTriggerType triggerType)
    {
        base.AnnimationTriggerEvent(triggerType);
    }
}
