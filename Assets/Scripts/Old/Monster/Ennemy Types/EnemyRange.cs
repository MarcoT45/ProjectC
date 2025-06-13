using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : Ennemy
{
    protected override void Awake()
    {
        base.Awake();

        IdleState = new EnnemyIdleState(this, StateMachine);
        ChasingState = new EnemyRangeChasingState(this, StateMachine);
        AttackingState = new OrcAttackingState(this, StateMachine);


    }

    protected override void Start()
    {

        this.gridManager = GameObject.FindWithTag("GridManager").GetComponent<GridManager>();
        Vector3Int cellPosition = gridManager.WorldToCell(transform.position);
        GridManager.CellData cellData = gridManager.GetCellData(cellPosition);
        cellData.containedInCell = this.gameObject;

        base.Start();
    }

    /* protected override void Update()
     {
         //base.Update();
     }*/
}
