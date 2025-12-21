using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBoss : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PoolEnemy pool;
    [SerializeField] private GameObject boss;

    [Header("Settings")]
    [SerializeField] private int initialSpawnCount = 3;
    [SerializeField] private float cooldown = 1.5f;
    [SerializeField] private float spawnRadius = 2.5f;

    private int currentNumber = 0;
    private float timer = 0;
    private bool initSpawn = true;
    private BossAI bossAI;

    void Awake()
    {
        boss = transform.parent.gameObject;
        bossAI = boss.GetComponent<BossAI>();

    }

    void Update()
    {

        if (bossAI != null)
        {
            if (bossAI.isInPhaseTwo)
            {
                timer += Time.deltaTime;

                if (initSpawn)
                {
                    timer = 0f;
                    /*for(int i = 0; i < initialSpawnCount; i++)
                    {
                        SpawnEnemy();
                    }*/
                    SpawnWaves(initialSpawnCount);
                    initSpawn = false;
                }
                else if (timer >= cooldown)
                {
                    if (currentNumber < pool.size)
                    {
                        int toSpawn = pool.size - currentNumber;

                        /*for(int i = 0; i < toSpawn; i++)
                        {
                            SpawnEnemy();
                        }*/
                        SpawnWaves(toSpawn);
                    }
                    timer = 0f;
                }
            }
        }

    }

    private void SpawnEnemy()
    {
        GameObject enemy = pool.GetEnemy();
        if(enemy == null)
        {
            return;
        }

        enemy.transform.position = this.transform.position;

    }

    private void SpawnWaves(int count)
    {
        if(count <= 0) return;

        Vector2 forward;

        if (bossAI != null)
        {
            forward = bossAI.forwardDirection;
        }
        else
        {
            forward = -boss.transform.up;
        }

        //Calcul des angles
        float angleStep = count > 1 ? 180f / (count - 1) : 0f;
        float startAngle = -90f;

        for(int i = 0; i < count; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * forward;
            Vector2 spawnPosition = (Vector2)transform.position + direction.normalized * spawnRadius;

            SpawnEnemy();
        }
    }
}
