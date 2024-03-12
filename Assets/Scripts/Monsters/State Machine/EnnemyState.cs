using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyState 
{
    protected Ennemy ennemy;
    protected EnnemyStateMachine ennemyStateMachine;

    public EnnemyState(Ennemy ennemy, EnnemyStateMachine ennemyStateMachine)
    {
        this.ennemy = ennemy;
        this.ennemyStateMachine = ennemyStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void AnnimationTriggerEvent(Ennemy.AnimationTriggerType triggerType) { }
}
