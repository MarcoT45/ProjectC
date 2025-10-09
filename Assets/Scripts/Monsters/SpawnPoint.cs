using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public enum SpawnType
    {
        Player,
        OnStart,
        Looping
    }

    public Vector2 point; // Point de spawn
    public SpawnType spawnType;
    public float spawnInterval; // Intervalle de spawn pour le type Looping

    [HideInInspector] public float nextSpawnTime; // Temps du prochain spawn

    void Start()
    {
        point = this.transform.position;
    }

}
