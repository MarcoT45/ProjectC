using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New fire", menuName = "ScriptableObjects/Abilities/Fire")]
public class FireAbility : Ability
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    private GameObject projectileInstance;

    public override void Activate(GameObject parent)
    {
        PlayerController player = parent.GetComponent<PlayerController>();
        Vector2 spawnPosition = player.transform.position + (Vector3)(player.forwardDirection * 1f); // 1f est la distance devant le joueur
        //Spawn prefab
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        //Direction du projectile
        Projectile proj = projectile.GetComponent<Projectile>();
        proj.direction = player.forwardDirection;
        proj.speed = projectileSpeed;   

        projectileInstance = projectile;

    }

    public override void BeginCooldown(GameObject parent)
    {
        // Optionnel : Détruire le projectile après activetime si nécessaire

        if (projectileInstance != null)
        {
            projectileInstance.GetComponent<Projectile>().OnDestroy();
        }
    }
}
