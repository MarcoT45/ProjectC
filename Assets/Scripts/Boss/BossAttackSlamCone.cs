using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackSlamCone : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 1;

    [Header("Timing Settings")]
    public float windUpTime = 0.5f; // Temps avant l'attaque
    public float slamDuration = 0.2f; // Durée de l'attaque

    [Header("Effects")]
    public GameObject slamEffectPrefab; // Effet visuel de l'attaque

    [Header("Cone Settings")]
    public float coneAngle = 60f; // Angle du cône
    public float coneRange = 5f; // Portée du cône

    private Transform bossTransform;
    private bool isAttacking = false;

    private void Awake()
    {
        bossTransform = transform;
    }

    public void PerformSlam(System.Action onFinished)
    {
        if (!isAttacking)
        {
            StartCoroutine(SlamCoroutine(onFinished));
        }
    }

    private IEnumerator SlamCoroutine(System.Action onFinished)
    {
        isAttacking = true;

        // Wind-up phase
        yield return new WaitForSeconds(windUpTime);

        // Instantiate slam effect
        BossAI bossAI = bossTransform.gameObject.GetComponent<BossAI>();
        if (slamEffectPrefab != null)
        {
            if (bossAI != null)
            {
                float rotationZ = Mathf.Atan2(bossAI.forwardDirection.y, bossAI.forwardDirection.x) * Mathf.Rad2Deg;
                Instantiate(slamEffectPrefab, bossTransform.position, Quaternion.Euler(0, 0, rotationZ));
            }
        }

        // Slam phase
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(bossTransform.position, coneRange);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Vector2 directionToPlayer = (hitCollider.transform.position - bossTransform.position).normalized;

                float dot = Vector2.Dot(bossAI.forwardDirection.normalized, directionToPlayer);
                float angleToPlayer = Mathf.Acos(dot) * Mathf.Rad2Deg;

                if (angleToPlayer <= coneAngle / 2)
                {
                    PlayerController player = hitCollider.GetComponent<PlayerController>();
                    if (player != null)
                    {
                        player.Damage(damage);
                    }
                }
            }
        }

        yield return new WaitForSeconds(slamDuration);

        isAttacking = false;
        onFinished?.Invoke();
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, coneRange);
    }
}
