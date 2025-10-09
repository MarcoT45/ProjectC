//  OLD FILE





/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Ennemy
{
    protected override void Awake()
    {
        base.Awake();

        IdleState = new EnnemyIdleState(this, StateMachine);
        //ChasingState = new EnnemyChasingState(this, StateMachine);
        ChasingState = new OrcChasingState(this, StateMachine);
        AttackingState = new OrcAttackingState(this, StateMachine);

   
    }

    protected override void Start()
    {

        this.gridManager = GameObject.FindWithTag("GridManager").GetComponent<GridManagerNew>();
        Vector3Int cellPosition = gridManager.WorldToCell(transform.position);
        GridManagerNew.CellData cellData = gridManager.GetCellData(cellPosition);
        cellData.containedInCell = this.gameObject;

        base.Start();
    }

   *//* protected override void Update()
    {
        //base.Update();
    }*//*
}
*/