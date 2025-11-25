using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackSlamCircle : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 1;
    public float radius = 3f;

    [Header("Timing Settings")]
    public float windUpTime = 0.5f; // Temps avant l'attaque
    public float slamDuration = 0.2f; // Durée de l'attaque

    [Header("Effects")]
    public GameObject slamEffectPrefab; // Effet visuel de l'attaque

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

    public IEnumerator SlamCoroutine(System.Action onFinished)
    {
        isAttacking = true;

        // Wind-up phase
        yield return new WaitForSeconds(windUpTime);

        // Instantiate slam effect
        //TODO: GameObject et Sprites
        if (slamEffectPrefab != null)
        {
            Instantiate(slamEffectPrefab, bossTransform.position, Quaternion.identity);
        }

        // Slam phase
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(bossTransform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            PlayerController player = hitCollider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Damage(damage);
            }
        }

        yield return new WaitForSeconds(slamDuration);

        isAttacking = false;
        onFinished?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
