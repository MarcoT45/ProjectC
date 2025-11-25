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
    [Header("Global Cooldowns")]
    public float globalCooldown = 5f;      // Cooldown global entre les attaques
    private float cooldownTimer = 0f;


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

        cooldownTimer -= Time.deltaTime;
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

    public bool IsCooldownComplete()
    {
        return cooldownTimer <= 0f;
    }

    public void ResetCooldown()
    {
        cooldownTimer = globalCooldown;
    }

    public float GetPlayerDistance()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            return distanceToPlayer;
        }

        return Mathf.Infinity;
    }

}
