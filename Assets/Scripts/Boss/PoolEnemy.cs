using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolEnemy : MonoBehaviour
{
    public int size = 6;
    [SerializeField] private GameObject enemyPrefab;
    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        for (int i = 0; i < size; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            //enemy.transform.parent = this.transform;
            enemy.SetActive(false);
            pool.Enqueue(enemy);
        }
    }

    public GameObject GetEnemy()
    {
        if (pool.Count == 0)
        {
            return null;
        }

        GameObject enemy = pool.Dequeue();
        enemy.SetActive(true);
        return enemy;
    }

    public void HideEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        pool.Enqueue(enemy);
    }
}
