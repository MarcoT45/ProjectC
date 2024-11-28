using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnnemyMoveable 
{
    public bool IsFacingRight { get; set; }

    void MoveEnnemy();

    void MoveEnnemyMovePoint(Vector2 direction);

    bool CanEnnemyMove(Vector2 direction);

    void CheckForLeftOrRightFacing(Vector2 direction);
}
