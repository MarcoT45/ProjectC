using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : EnemyAI
{
    // -------- Général boss --------
    [Header("Boss Settings")]
    public float phaseSwitchHealthThreshold = 0.5f; // Seuil de santé pour changer de phase (50% par défaut)
    public bool isInPhaseTwo = false; // Indique si le boss est en phase 2

    // -------- Cooldowns & Patterns --------
    [Header("Attack Patterns")]
    public float attackCooldown = 3f; // Temps entre les attaques
    private float attackTimer = 0f;

    // -------- Attaques Spéciales --------
/*    [Header("Special Attacks")]
    public BossAttack[] phaseOneAttacks; // Attaques de la phase 1
    public BossAttack[] phaseTwoAttacks; // Attaques de la phase 2*/

    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        base.Update();
    }


    public override void ApplyKnockback(Vector2 duration)
    {
        // Le boss n'est pas affecté par le knockback
        Debug.Log("Boss ignores knockback.");
        return;
    }

    public void CheckPhaseTransition()
    {
        float hpRatio = CurrentHealth / MaxHealth;

        if (!isInPhaseTwo && hpRatio <=  phaseSwitchHealthThreshold)
        {
            isInPhaseTwo = true;
            // Logique pour changer les attaques et comportements du boss
            Debug.Log("Le boss est passé en phase 2 !");
        }
    }

}
