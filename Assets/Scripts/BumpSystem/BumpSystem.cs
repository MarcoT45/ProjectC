using UnityEngine;
using System.Collections;

public static class BumpSystem
{
    public static void HandleBump(PlayerController player, EnemyAI enemy)
    {
        //Calcul de l'angle de l'attaque
        Vector2 playerToEnemy = (enemy.transform.position - player.transform.position ).normalized;
        playerToEnemy = GetCardinalDirection(playerToEnemy);
        float dotProduct = Vector2.Dot(enemy.forwardDirection, playerToEnemy);


       /* Debug.Log($"EnemytoPlayer: {playerToEnemy}");
        Debug.Log($"EnemyForward: {enemy.forwardDirection}");
        Debug.Log($"Dot Product: {dotProduct}");*/

        // Calcul des dégâts selon l'angle
        float damageMultiplier = 1f;
        if (dotProduct > 0.7f)
        {
            /*Debug.Log(" Attaque de face !");*/
            player.TakeDamage((int)enemy.monsterData.atk); // Le joueur prend des dégâts
            player.ApplyKnockback(-playerToEnemy * 0.75f);
        }
        else if (dotProduct < -0.7f)
        {
           /* Debug.Log(" Attaque dans le dos !");*/
            damageMultiplier = 2f;
            player.ApplyKnockback(-playerToEnemy * 0.1f);
        }
        else
        {
           /* Debug.Log(" Attaque latérale !");*/
            damageMultiplier = 1.5f;
            player.ApplyKnockback(-playerToEnemy * 0.1f);
        }

        int finalDamage = (int)(player.damage * damageMultiplier);
        enemy.Damage(finalDamage);
        enemy.ApplyKnockback(playerToEnemy);

        if (enemy.CurrentHealth <= 0)
        {
            GameObject.Destroy(enemy.gameObject);
        }

    }
    public static void HandleHazard(Collision2D hazard, PlayerController player = null, EnemyController enemy = null, bool bumpBack = false)
    {
        Vector2 hazardToEnemy = (enemy.transform.position - hazard.transform.position).normalized;


    }

    private static Vector2 GetCardinalDirection(Vector2 v)
    {
        // Si la valeur absolue de x est supérieure à celle de y, la direction est horizontale.
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
        {
            return new Vector2(Mathf.Sign(v.x), 0f); // Droite si x positif, gauche si négatif.
        }
        else
        {
            return new Vector2(0f, Mathf.Sign(v.y)); // Haut si y positif, bas si négatif.
        }
    }
}
