using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyIdleState : EnnemyState
{
    private Vector3 targetPos;
    private Vector2 direction;
    private float aggroRange = 3f;

    public EnnemyIdleState(Ennemy ennemy, EnnemyStateMachine ennemyStateMachine) : base(ennemy, ennemyStateMachine)
    {

    }

    public override void EnterState()
    { 
        base.EnterState();

        Debug.Log("Idle"); 

        direction = Vector2.zero;
        targetPos = GetRandomPointInCircle();
        targetPos = ennemy.solTileMap.WorldToCell(targetPos);

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        Vector3 ennemyCellPos;

        ennemy.MoveEnnemy();
        //Changer l'aggroRange et mettre dans Monster SO
        ennemy.isAggroed = ennemy.CheckAggro(direction, aggroRange);

        if (ennemy.isAggroed)
        {
            ennemy.stateMachine.ChangeState(ennemy.chasingState);
        }

        if (Vector3.Distance(ennemy.transform.position, ennemy.movePoint.position) <= .05f)
        {
            ennemy.lastPosition = ennemy.solTileMap.WorldToCell(ennemy.transform.position); 
            direction = ennemy.FindNextCell(direction, targetPos);
            ennemy.MoveEnnemyMovePoint(direction);
        }

        ennemyCellPos = ennemy.solTileMap.WorldToCell(ennemy.transform.position);
        if (Vector3.Distance(ennemyCellPos, targetPos) <= .05f)
        {
            targetPos = GetRandomPointInCircle();
            targetPos = ennemy.solTileMap.WorldToCell(targetPos);
        }
    }

    public override void AnnimationTriggerEvent(Ennemy.AnimationTriggerType triggerType)
    {
        base.AnnimationTriggerEvent(triggerType);
    }

    private Vector3 GetRandomPointInCircle()
    {
        return ennemy.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * ennemy.movementRange;
    }
}
