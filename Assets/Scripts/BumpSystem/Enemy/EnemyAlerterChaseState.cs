using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlerterChaseState : EnemyState {

    private GameObject target;
    private Vector3 targetPosition;
    private Vector3 currentPosition;
    private float aggroDuration = 5f;
    private float timeRemaining;
    private bool timerIsRunning = false;
    private float safeRange;
    private GridManager gridManager;

    public EnemyAlerterChaseState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy,enemyStateMachine) {}

    public override void EnterState() {
        base.EnterState();

        //Changer par le player du GM, autre façon de faire avec le joueur comme direction
        target = GameObject.FindWithTag("Player");
        targetPosition = target.transform.position;
        timeRemaining = aggroDuration;
        safeRange = enemy.safeRange;
        gridManager = enemy.gridManager;
    }

    public override void FrameFixedUpdate() {
        base.FrameFixedUpdate();

        currentPosition = enemy.transform.position;
        Vector3 targetTmp = target.transform.position;
        ManageAggro(targetTmp, currentPosition);
        ManageMovement();
    }

    private void ManageAggro(Vector3 playerCellPostion, Vector3 enemyCellPosition) {
        TimerAggro();
        Vector2 lineOfSightDirection = (target.transform.position - enemy.transform.position).normalized;
        //enemy.isAlerted = enemy.HasLineOfSight(lineOfSightDirection);

        //Update de la position que si vision sur le joueur
        if (enemy.isAlerted) {
            targetPosition = target.transform.position;
            AlertEveryone(); // On attire tous les ennemis du niveau
        }

        if (enemy.isAlerted || timeRemaining > 0) {
            if (enemy.isAlerted && timerIsRunning) {
                timerIsRunning = false;
                timeRemaining = aggroDuration;
            } else {
                timerIsRunning = true;
            }
        }
    }

    private void AlertEveryone() {
        GameObject[] enemiesList = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject e in enemiesList) {
            EnemyAI en = (EnemyAI) e.GetComponent(typeof(EnemyAI));
            //en.SetIdleTargetPosition(targetPosition);
        }
    }

    // ---Mouvement du movePoint et de l'ennemi 
    public void ManageMovement() {
        Vector3 directionToPlayer = targetPosition - currentPosition;
        Vector2 lineOfSightDirection = (target.transform.position - enemy.transform.position).normalized;

        if (directionToPlayer.magnitude <= safeRange - 0.1f) {
            MoveAwayFromPlayer();
        } else if(directionToPlayer.magnitude >= safeRange + 0.1f) {
            MoveTowardsPlayer();
        } else if (directionToPlayer.magnitude >= safeRange - 0.1f && directionToPlayer.magnitude <= safeRange + 0.1f) { // Pour regler le tremblement
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
        }
    }

    private void MoveTowardsPlayer() {
        //enemy.Move(targetPosition);
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

    public void TimerAggro() {
        if (timerIsRunning) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
            } else {
                //enemy.StateMachine.ChangeState(enemy.IdleState);
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    public override void AnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }

}