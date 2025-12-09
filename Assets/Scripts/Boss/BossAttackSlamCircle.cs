using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackSlamCircle : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 1;
    public float range = 3f;

    [Header("Timing Settings")]
    public float windUpTime = 0.2f; // Temps avant l'attaque
    public float slamDuration = 0.2f; // Durée de l'attaque

    // A voir pour les effets visuels si comme ça
    /* [Header("Effects")]
     public GameObject slamEffectPrefab; // Effet visuel de l'attaque*/

    private Transform bossTransform;

    private void Awake()
    {
        bossTransform = transform;
    }

    public void DoSlam()
    {

        // Cf au dessus pour les effets visuels
        // Instantiate slam effect
        /*if (slamEffectPrefab != null)
        {
            Instantiate(slamEffectPrefab, bossTransform.position, Quaternion.identity);
        }*/

        // Slam phase
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(bossTransform.position, range);
        foreach (var hitCollider in hitColliders)
        {
            PlayerController player = hitCollider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Damage(damage);
                player.ApplyKnockback((player.transform.position - bossTransform.position).normalized);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
