using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyIdleState : EnemyState {
    
    private Vector3 targetPosition;

    public EnemyIdleState(EnemyAI enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {}

    public override void EnterState() { 
        base.EnterState();

        GetRandomPointInCircle();
        enemy.OnIdleDestinationReached +=  GetRandomPointInCircle;
    }

    public override void ExitState() {
        base.ExitState();
        enemy.OnIdleDestinationReached -= GetRandomPointInCircle;
    }

    public override void FrameUpdate() {
        base.FrameUpdate();
        
        //Le monstre ne voit que devant lui en idle
        Vector2 lineOfSightDirection = enemy.forwardDirection;
        enemy.isAggroed = enemy.HasLineOfSight(lineOfSightDirection);

        if (enemy.isAggroed) {
            //Tween animation du saut
            enemy.transform.DOLocalJump(enemy.transform.position, 1f, 1, 0.5f).SetEase(Ease.InOutQuint);
            enemy.StateMachine.ChangeState(enemy.ChasingState);
        }

        //Mouvement
        enemy.Move(enemy.idleTargetPosition);
    }

    public override void AnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }

    private void GetRandomPointInCircle() {
        enemy.idleTargetPosition = enemy.gridManager.FindRandomWalkableInRange(enemy.transform.position, enemy.chaseDistance);
    }

}