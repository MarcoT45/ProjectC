using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnnemyIdleState : EnnemyState
{
    private Vector3 targetPos;
    private Vector2 direction;
    private float aggroRange = 4f;

    public EnnemyIdleState(Ennemy ennemy, EnnemyStateMachine ennemyStateMachine) : base(ennemy, ennemyStateMachine)
    {

    }

    public override void EnterState()
    { 
        base.EnterState();

        Debug.Log("Idle"); 

        direction = Vector2.zero;
        targetPos = GetRandomPointInCircle();
        // targetPos = ennemy.solTileMap.WorldToCell(targetPos);
        targetPos = ennemy.gridManager.WorldToCell(targetPos);
        ennemy.newCellTarget = ennemy.gridManager.WorldToCell(ennemy.transform.position);

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



        if (Vector3.Distance(ennemy.transform.position, ennemy.gridManager.CellToWorld(ennemy.newCellTarget)) <= .05f)
        {

            if (Time.time > ennemy.LastUsedTimeMove + ennemy.coolDownMove)
            {

                ennemy.lastPosition = ennemy.gridManager.WorldToCell(ennemy.transform.position);
                direction = ennemy.FindNextCell(direction, targetPos);

                Vector3 nextPosition = ennemy.transform.position + (Vector3)direction;
                Vector3Int gridNextPosition = ennemy.gridManager.WorldToCell(nextPosition);

                ennemy.newCellTarget = gridNextPosition;

                if (ennemy.isAggroed)
                {
                    //Tween animation du saut
                    ennemy.transform.DOLocalJump(ennemy.transform.position, 1f, 1, 0.5f)
                         .SetEase(Ease.InOutQuint);
                    ennemy.StateMachine.ChangeState(ennemy.ChasingState);
                }

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

        ennemyCellPos = ennemy.gridManager.WorldToCell(ennemy.transform.position);
        if (Vector3.Distance(ennemyCellPos, targetPos) <= .05f)
        {
            targetPos = GetRandomPointInCircle();
            targetPos = ennemy.gridManager.WorldToCell(targetPos);
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
