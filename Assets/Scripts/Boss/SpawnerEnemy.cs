using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{

    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int numberOfEnemies = 0;  
    [SerializeField] private int cooldown = 0;  
    private int currentNumber = 0;
    private float timer = 0;
    private bool initSpawn = true;


    void Awake()
    {
        boss = GameObject.FindWithTag("Enemy");
        transform.position = boss.transform.position;    
    }

    private void Start()
    {
    }

    void Update()
    {
        timer += Time.time;
        BossAI bossAI = boss.GetComponent<BossAI>();

        if (bossAI != null)
        {
            if (bossAI.isInPhaseTwo)
            {
                if (initSpawn)
                {
                    SpawnEnemy(numberOfEnemies);
                    initSpawn = false;
                }

                if (timer >= cooldown)
                {
                    if (currentNumber < numberOfEnemies)
                    {
                        SpawnEnemy(1);
                    }
                    timer = 0;
                }
            }
        }

    }

    void SpawnEnemy(int number)
    {
        GameObject enemy = null;
        EnemyAI enemyAI = null;

        int n = 0;
        Vector2 position = this.transform.position;


        if (currentNumber < number)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                switch (n)
                {
                    case 0:
                        position = position + Vector2.up;
                        n = 1;
                        break;

                    case 1:
                        position = position + Vector2.right;
                        n = 2;
                        break;

                    case 2:
                        position = position + Vector2.left;
                        n = 0;
                        break;
                }

                enemy = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity );
                enemy.transform.parent = this.transform;

                enemyAI = enemy.GetComponent<EnemyAI>();

                if (enemyAI != null)
                {

                }

                currentNumber++;
            }
        }
    }
}
