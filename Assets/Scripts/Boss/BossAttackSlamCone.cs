using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackSlamCone : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 1;
    public float range = 3f;

    [Header("Timing Settings")]
    public float windUpTime = 0.5f; // Temps avant l'attaque
    public float slamDuration = 0.2f; // Durée de l'attaque

    /*    [Header("Effects")]
        public GameObject slamEffectPrefab; // Effet visuel de l'attaque

        [Header("Cone Settings")]
        public float coneAngle = 60f; // Angle du cône
        public float coneRange = 5f; // Portée du cône
    */

    private Transform bossTransform;

    private void Awake()
    {
        bossTransform = transform;
    }

    public void DoSlam()
    {
        // Supposons que le boss regarde vers le bas
        Vector2 direction = Vector2.down;
        RaycastHit2D[] hits = Physics2D.RaycastAll(bossTransform.position, direction, range);

        foreach (var hit in hits)
        {
            PlayerController player = hit.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                Debug.Log("BossAttackSlamCone hit player");
                player.Damage(damage);
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, range * Vector2.down);
    }
}
