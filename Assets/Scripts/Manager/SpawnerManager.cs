using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public MapBuilder mapBuilder;
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    public GameObject playerPrefab;
    public int nbStartMonsters = 0; // Nombre de monstres à spawn au début
    public int maxMonsters = 0; // Nombre maximum de monstres à spawn
    public GameObject enemyPrefab;
    [HideInInspector] public List<GameObject> spawnedEnemies = new List<GameObject>(); // Liste des ennemis spawnés

    void Start()
    {
        if(mapBuilder != null)
        {
            spawnPoints = mapBuilder.spawnPoints;
        }

        List<SpawnPoint> playerSpawnPoints = new List<SpawnPoint>();

        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            if (spawnPoint.spawnType == SpawnPoint.SpawnType.Player)
            {
                playerSpawnPoints.Add(spawnPoint);
            }
            else if(spawnPoint.spawnType == SpawnPoint.SpawnType.OnStart)
            {
                for (int i = 0; i < nbStartMonsters; i++)
                {
                    SpawnEnemy(spawnPoint);
                }
            }
            else if (spawnPoint.spawnType == SpawnPoint.SpawnType.Looping)
            {
                spawnPoint.nextSpawnTime = Time.time + spawnPoint.spawnInterval;
            }
        }

        // Spawn du joueur à un point de spawn aléatoire
        if (playerSpawnPoints.Count > 0 && playerPrefab != null)
        {
            SpawnPoint randomPlayerSpawn = playerSpawnPoints[Random.Range(0, playerSpawnPoints.Count)];
            Instantiate(playerPrefab, randomPlayerSpawn.point, Quaternion.identity);
        }
    }

    void Update()
    {
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            if (spawnPoint.spawnType == SpawnPoint.SpawnType.Looping )
            {

                // Nettoyer la liste des ennemis spawnés en supprimant ceux qui ont été détruits
                spawnedEnemies.RemoveAll(enemy => enemy == null);

                // Vérifier si on peut respawner un ennemi
                if (spawnedEnemies.Count < maxMonsters && Time.time >= spawnPoint.nextSpawnTime)
                {
                    SpawnEnemy(spawnPoint);
                    spawnPoint.nextSpawnTime = Time.time + spawnPoint.spawnInterval;
                }
            }
        }
    }

    public void SpawnEnemy(SpawnPoint spawnPoint)
    {
        if(spawnPoint == null || enemyPrefab == null)
        {
            Debug.LogWarning("SpawnPoint or enemyPrefab is null.");
            return;
        }
        if (spawnedEnemies.Count >= maxMonsters)
        {
            Debug.Log("Maximum number of monsters reached.");
            return;
        }

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.point, Quaternion.identity);
        spawnedEnemies.Add(newEnemy);
        
    }

    public void ClearEnemies()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            Destroy(enemy);
        }
        spawnedEnemies.Clear();
    }
}
