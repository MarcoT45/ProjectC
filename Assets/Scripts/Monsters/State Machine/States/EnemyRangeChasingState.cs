using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeChasingState : EnnemyState
{
    private GameObject target;
    private Vector3 targetPos;
    private Vector2 direction;
    private Vector3Int currentGridPosition;

    private float aggroDuration = 5f;
    private float timeRemaining;
    private bool timerIsRunning = false;
    private float aggroRange = 5f;
    private float safeRange = 3f;

    private float attackCoolDown = 2f;
    private float attacKTimeRemaining;
    private bool attackKTimeIsRuning = false;

    //Tir de projectiles
    public Transform firePoint;
    public float fireRate = 3f;
    private float lastShotTime = 0f;


    private GridManager gridManager;

    public EnemyRangeChasingState(Ennemy ennemy, EnnemyStateMachine ennemyStateMachine) : base(ennemy, ennemyStateMachine)
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

        gridManager = ennemy.gridManager;
        firePoint = ennemy.transform;

    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();

        Vector3Int ennemyCellPos = ennemy.gridManager.WorldToCell(ennemy.transform.position);
        currentGridPosition = ennemyCellPos;

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
        ennemy.MoveEnnemy();

        Vector3Int playerGridPosition = gridManager.WorldToCell(targetPos);
        Vector3Int enemyGridPosition = gridManager.WorldToCell(ennemy.transform.position);
        Vector3Int directionToPlayer = playerGridPosition - enemyGridPosition;

        if (Vector3.Distance(ennemy.transform.position, gridManager.CellToWorld(ennemy.newCellTarget)) <= .05f)
        {
            //Debug.Log("Away rangz " + safeRange);

            //Player détecté et ennemi reste à une certaine distance

         /*   Debug.Log(IsPlayerAligned(playerGridPosition));
            Debug.Log(directionToPlayer.magnitude <= safeRange);
            Debug.Log(Time.time - lastShotTime >= fireRate);*/
            if (IsPlayerAligned(playerGridPosition) && directionToPlayer.magnitude <= safeRange && Time.time - lastShotTime >= fireRate)
            {
                //Debug.Log("Shoot");
                Shoot();
            }
            else if (directionToPlayer.magnitude <= safeRange)
            { 
                //Debug.Log("Move away");
                MoveAwayFromPlayer(playerGridPosition);

            }else
            {
                //Debug.Log("Move");
                MoveATowardsPlayer();
            }

/*
            direction = ennemy.FindNextCell(direction, targetPos);
            Vector3 nextPosition = ennemy.transform.position + (Vector3)direction;
            Vector3Int gridNextPosition = gridManager.WorldToCell(nextPosition);

            if (gridManager.CanMoveOnCell(gridNextPosition))
            {
                GridManager.CellData cellData = gridManager.GetCellData(gridNextPosition);

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
            }*/

        }
    }

    private void MoveATowardsPlayer()
    {
        direction = ennemy.FindNextCell(direction, targetPos);
        Vector3 nextPosition = ennemy.transform.position + (Vector3)direction;
        Vector3Int gridNextPosition = gridManager.WorldToCell(nextPosition);
        Move(gridNextPosition);
    }

    private void MoveAwayFromPlayer(Vector3Int playerGridPosition)
    {
        Vector3Int awayFromPlayer = currentGridPosition - (playerGridPosition - currentGridPosition);
        Vector3Int nextMove = GetBestMove(awayFromPlayer);
        Move(nextMove);
    }

    private Vector3Int GetBestMove(Vector3Int awayFromPlayer)
    {
        List<Vector3Int> possibleMoves = new List<Vector3Int>
        {
            currentGridPosition + Vector3Int.up,
            currentGridPosition + Vector3Int.down,
            currentGridPosition + Vector3Int.left,
            currentGridPosition + Vector3Int.right
        };

        Vector3Int bestMove = currentGridPosition;
        float shortestDistance = float.MaxValue;

        foreach (var move in possibleMoves)
        {
            if (ennemy.gridManager.CanMoveOnCell(move))
            {
                float distance = Vector3Int.Distance(move, Vector3Int.FloorToInt(awayFromPlayer));
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    bestMove = move;
                }
            }
        }

        return bestMove;
    }

    private bool IsPlayerAligned(Vector3Int playerGridPosition)
    {
        // Vérifie si le joueur est sur la même ligne ou colonne
        if (currentGridPosition.x == playerGridPosition.x || currentGridPosition.y == playerGridPosition.y)
        {
            Vector2 direction = ((Vector3)playerGridPosition - (Vector3)currentGridPosition).normalized;
            return !gridManager.IsObstacleBetween(currentGridPosition, playerGridPosition, direction);
        }
        return false;
    }

    private void Shoot()
    {
        if (Time.time - lastShotTime >= fireRate)
        {
            lastShotTime = Time.time;
            GameObject projectile = UnityEngine.Object.Instantiate(ennemy.projectilePrefab, firePoint.position, Quaternion.identity);

            Vector3 start = gridManager.WorldToCell(targetPos);
            Vector3 end = gridManager.WorldToCell(ennemy.transform.position);
            projectile.GetComponent<ProjectileEnemyRange>().Initialize(start - end);
        }
    }

    private void Move(Vector3Int gridNextPosition)
    {

        if (gridManager.CanMoveOnCell(gridNextPosition))
        {
            GridManager.CellData cellData = gridManager.GetCellData(gridNextPosition);

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

    public void TimerAggro()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
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
