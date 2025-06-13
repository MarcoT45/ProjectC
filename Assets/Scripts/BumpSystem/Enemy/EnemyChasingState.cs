using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyChasingState : EnemyState
{
    private GameObject target;
    private Vector3 targetPosition;
    private Vector2 direction;

    private float aggroDuration = 5f;
    private float timeRemaining;
    private bool timerIsRunning = false;
    private float aggroRange = 4f;


    public EnemyChasingState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy,enemyStateMachine)
    {

    }

    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Chasing");

        direction = Vector2.zero;
        //Changer par le player du GM, autre façon de faire avec le joueur comme direction
        target = GameObject.FindWithTag("Player");
        targetPosition = target.transform.position;

        timeRemaining = aggroDuration;

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        ManageAggro(targetPosition);

        //Mouvement
        enemy.Move(targetPosition);


    }
    private void ManageAggro(Vector3 playerCellPostion)
    {
        TimerAggro();
        Vector2 lineOfSightDirection = (enemy.player.position - enemy.transform.position).normalized;
        enemy.isAggroed = enemy.HasLineOfSight(lineOfSightDirection);

        //Update de la position que si vision sur le joueur
        if(enemy.isAggroed )
        {
            targetPosition = target.transform.position;
        }

        if (enemy.isAggroed || timeRemaining > 0)
        {

            if (enemy.isAggroed && timerIsRunning)
            {
                timerIsRunning = false;
                timeRemaining = aggroDuration;
            }
            else
            {
                timerIsRunning = true;
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
                enemy.StateMachine.ChangeState(enemy.IdleState);
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }
    public override void AnnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType)
    {
        base.AnnimationTriggerEvent(triggerType);
    }
}
