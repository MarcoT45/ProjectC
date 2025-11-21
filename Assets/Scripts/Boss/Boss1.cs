using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss1 : BossAI
{
    [Header("References")]
    public GameObject rootPrefab;           // Prefab de la racine

    [Header("Pattern Settings")]    
    public int numberOfWaves = 3;           // Nombre de vagues de racines
    public float[] waveRadius;              // Rayons pour chaque vague : ex : [2f, 4f, 6f]
    public int rootsPerWave = 6;            // Nombre de racines par vague
    public float timeBetweenWaves = 1f;     // Temps entre chaque vague
    public float rootLifetime = 0.7f;         // Duree de vie des racines

    [HideInInspector] public bool attackInProgress = false;

    protected override void Awake()
    {
        base.Awake();

        IdleState = new BossCoreState(this, StateMachine);
        AttackingState = new BossAttackState(this, StateMachine);
    }

    protected override void Start()
    {
        base.Start();
    }

    public IEnumerator RootAttack()
    {
        attackInProgress = true;

        // Lancer les vagues de racines
        for (int i = 0;  i < numberOfWaves;  i++)
        {
            float radius = waveRadius[i];

            SpawnRootsInWave(radius);

            yield return new WaitForSeconds(timeBetweenWaves);
        }

        attackInProgress = false;
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

            GameObject root = Instantiate(rootPrefab, spawnPosition, Quaternion.identity);
            root.transform.parent = this.transform;

            Destroy(root, rootLifetime);
        }

    }
}
