using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnnemyMoveable 
{
    bool isFacingRight { get; set; }

    void MoveEnnemy();

    void MoveEnnemyMovePoint(Vector2 direction);

    bool CanEnnemyMove(Vector2 direction);

    void CheckForLeftOrRightFacing(Vector2 direction);
}
