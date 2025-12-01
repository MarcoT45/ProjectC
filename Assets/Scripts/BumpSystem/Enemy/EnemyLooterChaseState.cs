using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLooterChaseState : EnemyState {

    private GameObject target;
    private Vector3 targetPosition;
    private Vector3 currentPosition;

    private float aggroDuration = 5f;
    private float timeRemaining;
    private bool timerIsRunning = false;

    private GridManager gridManager;

    public EnemyLooterChaseState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy,enemyStateMachine) {}

    public override void EnterState() {
        base.EnterState();

        //Changer par le player du GM, autre façon de faire avec le joueur comme direction
        target = GameObject.FindWithTag("Player");
        targetPosition = target.transform.position;
        timeRemaining = aggroDuration;
        gridManager = enemy.gridManager;
    }

    public override void FrameUpdate() {
        base.FrameUpdate();

        currentPosition = enemy.transform.position;
        ManageAggro(targetPosition);
        enemy.Move(targetPosition);
    }

    // Ici on va faire 2 cas, N°1 => Il voit une piece, N°2 => Il voit le joueur
    private void ManageAggro(Vector3 playerCellPostion) {
        TimerAggro();
        Vector2 lineOfSightDirection = (enemy.player.position - enemy.transform.position).normalized;

        //Update de la position si vision sur un collectible
        /*
        if(enemy.HasLootInLineOfSight(lineOfSightDirection)) {
            enemy.isAggroed = true;
            targetPosition = enemy.loot.transform.position;  // targetPosition <= Ca devient le collectible/la piece
        }
        */

        //Update de la position si vision sur le joueur
        if(enemy.HasLineOfSight(lineOfSightDirection)) {          // FAUT TROUVER UN CHEMIN QUI NE PASSE PAS PAR LE JOUEUR
            enemy.isAggroed = true;
            targetPosition = GetFarthestMove();  // Ca devient le point le plus éloigné du joueur
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

    private Vector3 GetFarthestMove() {
        Vector3 bestMove = currentPosition;
        float farthestDistance = float.MinValue;

        foreach (Node n in gridManager.GetAllWalkableNodes()) {            
            float distance = Vector3Int.Distance(n.cellPosition, Vector3Int.FloorToInt(target.transform.position));
            if (distance > farthestDistance) {
                farthestDistance = distance;
                bestMove = n.cellPosition;
            }
        } 
        return bestMove;
    }

    public void TimerAggro() {
        if(timerIsRunning) {
            if(timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
            } else {
                enemy.StateMachine.ChangeState(enemy.LookingState);
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    public override void AnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }

}