using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyRangeChaseState : EnemyState {

    private GameObject target;
    private Vector3 targetPosition;
    private Vector3 currentPosition;

    private float aggroDuration;
    private float timeRemaining;
    private bool timerIsRunning = false;
    private float aggroRange;
    private float safeRange;

    private float attackCoolDown;
    private float attackTimeRemaining;
    private bool attackTimeIsRuning = false;

    //Tir de projectiles
    public Transform firePoint;
    private float fireRate;
    private float lastShotTime = 0f;

    private GridManager gridManager;

    public EnemyRangeChaseState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() {
        base.EnterState();

        Debug.Log("Chasing");

        //Changer par le player du GM, autre façon de faire avec le joueur comme direction
        target = GameObject.FindWithTag("Player");
        targetPosition = target.transform.position;

        timeRemaining = aggroDuration;

        attackTimeRemaining = attackCoolDown;
        attackTimeIsRuning = true;

        firePoint = enemy.transform;
        aggroDuration = enemy.aggroDuration;
        aggroRange = enemy.chaseDistance;
        safeRange = enemy.safeRange;
        attackCoolDown = enemy.attackCoolDown;
        fireRate = enemy.fireRate;
        gridManager = enemy.gridManager;
    }

    public override void FrameUpdate() {
        base.FrameUpdate();

        currentPosition = enemy.transform.position;
        Vector3 targetTmp = target.transform.position;
        ManageAggro(targetTmp, currentPosition);
        ManageMovement();
    }

    private void ManageAggro(Vector3 playerCellPostion, Vector3 enemyCellPosition) {
        TimerAggro();
        Vector2 lineOfSightDirection = (target.transform.position - enemy.transform.position).normalized;
        enemy.isAggroed = enemy.HasLineOfSight(lineOfSightDirection);


        //Update de la position que si vision sur le joueur
        if (enemy.isAggroed) {
            targetPosition = target.transform.position;
        }

        if (enemy.isAggroed || timeRemaining > 0) {
            if (enemy.isAggroed && timerIsRunning) {
                timerIsRunning = false;
                timeRemaining = aggroDuration;
            } else {
                timerIsRunning = true;
            }
        }
    }

    // ---Mouvement du movePoint et de l'ennemi 
    public void ManageMovement() {
        Vector3 directionToPlayer = targetPosition - currentPosition;
        Vector2 lineOfSightDirection = (target.transform.position - enemy.transform.position).normalized;

        if (enemy.HasLineOfSight(lineOfSightDirection) && directionToPlayer.magnitude <= safeRange + 0.1f && Time.time - lastShotTime >= fireRate) {
            Debug.Log("Shoot");
            Shoot();
        } else if (directionToPlayer.magnitude <= safeRange - 0.1f) {
            Debug.Log("Move away");
            MoveAwayFromPlayer();
        } else if(directionToPlayer.magnitude >= safeRange + 0.1f) {
            Debug.Log("Move");
            MoveTowardsPlayer();
        } else if (directionToPlayer.magnitude >= safeRange - 0.1f && directionToPlayer.magnitude <= safeRange + 0.1f) { // Pour regler le tremblement
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
        }
    }

    private void MoveTowardsPlayer() {
        enemy.Move(targetPosition);
    }

    private void MoveAwayFromPlayer() {
        Vector3 awayFromPlayer = currentPosition - (targetPosition - currentPosition);
        Vector3 nextMove = GetBestMove(awayFromPlayer);
        MoveAway(nextMove);
    }

    private void MoveAway(Vector3 nextMove) {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        rb.velocity = nextMove.normalized * enemy.monsterData.speed * 1.5f; // En mode combat donc toujours en boost
    }

    private Vector3 GetBestMove(Vector3 awayFromPlayer) {
        Vector3 bestMove = currentPosition;
        float shortestDistance = float.MaxValue;

        Node currentNode =  gridManager.GetNodeFromWorldPoint(bestMove);

        foreach (Node neighbor in gridManager.GetNeighbors(currentNode)) {
            if (!neighbor.walkable)
                continue;
            
            float distance = Vector3Int.Distance(neighbor.cellPosition, Vector3Int.FloorToInt(awayFromPlayer));
            if (distance < shortestDistance) {
                shortestDistance = distance;
                bestMove = neighbor.cellPosition;
            }
        } 
        return bestMove;
    }

    private void Shoot() {
        if (Time.time - lastShotTime >= fireRate) {
            lastShotTime = Time.time;
            GameObject projectile = UnityEngine.Object.Instantiate(enemy.projectilePrefab, firePoint.position, Quaternion.identity);

            projectile.GetComponent<ProjectileEnemyRange>().Initialize(targetPosition - currentPosition);
        }
    }

    public void TimerAggro() {
        if (timerIsRunning) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
            } else {
                enemy.StateMachine.ChangeState(enemy.IdleState);
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    public override void AnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }

}