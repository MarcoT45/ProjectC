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

    public EnemyState TransitionState { get; set; }

    private float cooldownTimer = 0f;
    private bool isInvincible = false;

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
        return;
    }

    public bool CheckPhaseTransition()
    {
        float hpRatio = CurrentHealth / MaxHealth;

        if (!isInPhaseTwo && hpRatio <=  phaseSwitchHealthThreshold)
        {
            return true;
        }

        return false;
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
