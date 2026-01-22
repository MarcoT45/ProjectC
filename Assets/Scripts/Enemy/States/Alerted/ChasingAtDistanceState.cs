using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingAtDistanceState : EnemyState {

    private float alertDuration = 5f;
    private float timeAlertRemaining;
    private float attackCdRemaining;
    private Vector3 lastSeen;

    public ChasingAtDistanceState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        timeAlertRemaining = alertDuration;
        attackCdRemaining = enemy.attackCoolDown;

        enemy.speedBoostAlertMultiplicator = 1.75f;
        enemy.OnMoveDestinationReached += OnDestinationReached;

        enemy.bubbleSearch.SetActive(false);
    }

    public override void ExitState() {
        base.ExitState();

        enemy.speedBoostAlertMultiplicator = 1.0f;
        enemy.OnMoveDestinationReached -= OnDestinationReached;
    }

    public override void FrameUpdate() {
        base.FrameUpdate();

        TimerAlert();
        TimerAttack();

        Vector2 lineOfSightDirection = (enemy.player.position - enemy.transform.position).normalized;
        enemy.isSearching = enemy.SearchLineOfSight(lineOfSightDirection);
        enemy.isAlerted = enemy.AlertLineOfSight(lineOfSightDirection);

        if (enemy.isSearching || enemy.isAlerted) {
            enemy.SetTargetPosition(enemy.playerSeachPosition);
            lastSeen = enemy.playerSeachPosition;
            timeAlertRemaining = alertDuration;
        }

        
        Vector3 directionToPlayer = enemy.player.position - enemy.transform.position;

        if (enemy.isSearching && directionToPlayer.magnitude <= enemy.safeRange + 0.1f && attackCdRemaining < 0) {
            enemy.rb.velocity = Vector2.zero;
            enemy.StateMachine.ChangeState(enemy.AttackState);
        } else if (enemy.isSearching && directionToPlayer.magnitude <= enemy.safeRange - 0.1f) {
            enemy.SetTargetPosition(GetBestMove(enemy.transform.position - (enemy.playerSeachPosition - enemy.transform.position)));
            enemy.Move();
        } else if(enemy.isSearching && directionToPlayer.magnitude >= enemy.safeRange + 0.1f) {
            enemy.Move();
        } else if (enemy.isSearching && directionToPlayer.magnitude >= enemy.safeRange - 0.1f && directionToPlayer.magnitude <= enemy.safeRange + 0.1f) {
            enemy.rb.velocity = Vector2.zero;
        } else {
            enemy.SetTargetPosition(lastSeen);
            enemy.Move();
        }

        if (enemy.isKnockedBack)
        {
            enemy.StateMachine.ChangeState(enemy.StunState);
        }
    }

    public override void FrameFixedUpdate() {
        base.FrameFixedUpdate();

    }

    private Vector3 GetBestMove(Vector3 awayFromPlayer) {
        Vector3 bestMove = enemy.transform.position;
        float shortestDistance = float.MaxValue;

        Node currentNode = enemy.gridManager.GetNodeFromWorldPoint(bestMove);

        foreach (Node neighbor in enemy.gridManager.GetNeighbors(currentNode)) {
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

    public void TimerAlert() {
        if(timeAlertRemaining > 0) {
            timeAlertRemaining -= Time.fixedDeltaTime;
        } else {
            enemy.StateMachine.ChangeState(enemy.PatrolState);
        }
    }

    public void TimerAttack() {
        if(attackCdRemaining > 0) {
            attackCdRemaining -= Time.fixedDeltaTime;
        }
    }

    private void OnDestinationReached() {
        if ((!enemy.isSearching || !enemy.isAlerted) && Vector3.Distance(enemy.transform.position, lastSeen) < 0.5f) {
            enemy.StateMachine.ChangeState(enemy.AlertedLookingState);
        } else {
            enemy.SetTargetPosition(lastSeen);
            enemy.Move();
        }
    }

}