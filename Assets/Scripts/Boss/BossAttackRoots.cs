using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackRoots : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 1;
    public float radius = 3f;

    [Header("Timing Settings")]
    public float windUpTime = 0.5f;       // Temps avant l'attaque
    public float slamDuration = 0.2f;     // Durée de l'attaque
/*
    [Header("Effects")]
    public GameObject slamEffectPrefab;   // Effet visuel de l'attaque*/

    [Header("Pattern Settings")]
    public int numberOfWaves = 3;         // Nombre de vagues de racines
    public float[] waveRadius;            // Rayons pour chaque vague : ex : [2f, 4f, 6f]
    public int rootsPerWave = 6;          // Nombre de racines par vague
    public float timeBetweenWaves = 1f;   // Temps entre chaque vague
    public float rootLifetime = 0.7f;     // Duree de vie des racines

    private Transform bossTransform;
    private bool isAttacking = false;

    private void Awake()
    {
        bossTransform = transform;
    }

    public void PerformAttack()
    {
        if (isAttacking)return;
        Debug.Log("BossAttackRoots: Performing Root Attack");
        StartCoroutine(RootAttack());

    }

    public IEnumerator RootAttack()
    {
        isAttacking = true;

        // Lancer les vagues de racines
        for (int i = 0; i < numberOfWaves; i++)
        {
            float radius = waveRadius[i];

            SpawnRootsInWave(radius);

            yield return new WaitForSeconds(timeBetweenWaves);
        }
        isAttacking = false;
    }

    public void SpawnRootsInWave(float radius)
    {
        float angleStep = 360f / rootsPerWave;

        // Spawn des racines en cercle
        for (int i = 0; i < rootsPerWave; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 spawnPosition = new Vector3(
                transform.position.x + radius * Mathf.Cos(angle),
                transform.position.y + radius * Mathf.Sin(angle),
                0
            );

            // Essai de s'assurer que la racine spawn sur une case marchable
            /* Node node = gridManager.GetNodeFromWorldPoint(spawnPosition);
             GameObject root = Instantiate(rootPrefab, node.cellPosition, Quaternion.identity);*/

            Boss1 boss = bossTransform.gameObject.GetComponent<Boss1>();
            GameObject root = Instantiate(boss.rootPrefab, spawnPosition, Quaternion.identity);
            root.transform.parent = this.transform;

            Destroy(root, rootLifetime);
        }

    }
}
