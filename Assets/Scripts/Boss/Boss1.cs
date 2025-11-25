using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : BossAI
{
    [Header("References")]
    public GameObject rootPrefab;           // Prefab de la racine
    public BossAttackSlamCircle slamCircle; // Référence à l'attaque de slam en cercle
    public BossAttackSlamCone slamCone;     // Référence à l'attaque de slam en cône
    public BossAttackRoots rootAttack;       // Référence à l'attaque de racines

    [Header("Distance Settings")]
    public float coneMinDistance = 3f;   // Distance minimale pour l'attaque en cône
    public float coneMaxDistance = 6f;   // Distance maximale pour l'attaque en cône
    public float circleDistance = 2f;    // Distance pour l'attaque en cercle

    [HideInInspector] public bool attackInProgress = false;

    protected override void Awake()
    {
        base.Awake();

        IdleState = new BossCoreState(this, StateMachine);
        AttackingState = new BossAttackState(this, StateMachine);

        chaseDistance = 15f;
    }

    protected override void Start()
    {
        base.Start();
    }

}
