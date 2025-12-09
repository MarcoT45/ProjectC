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

        // Calcul des dégâts selon l'angle
        float damageMultiplier = 1f;
        if (dotProduct > 0.7f)
        {
            //Attaque de face 
            player.Damage((int)enemy.monsterData.atk); // Le joueur prend des dégâts

            // Trigger des passifs de l'équipement du joueur ( OnHit )
            if (player.equipment != null)
            {
                player.equipment.TriggerPassives(EquipmentTriggerType.OnHitTaken, enemy.gameObject, (int)enemy.monsterData.atk);
            }

            player.ApplyKnockback(-playerToEnemy * 0.75f);
        }
        else if (dotProduct < -0.7f)
        {
            //Attaque dans le dos 
            damageMultiplier = 2f;
            player.ApplyKnockback(-playerToEnemy * 0.1f);
        }
        else
        {
            //Attaque de côté
            damageMultiplier = 1.5f;
            player.ApplyKnockback(-playerToEnemy * 0.1f);
        }

        // Application des dégâts à l'ennemi
        int finalDamage = (int)(player.playerStats.totalStats.atk * damageMultiplier);
        enemy.Damage(finalDamage);

        // Trigger des passifs de l'équipement du joueur ( OnHit )
        if (player.equipment != null)
        {
            player.equipment.TriggerPassives(EquipmentTriggerType.OnHit, enemy.gameObject, finalDamage);
        }

        // Application du knockback à l'ennemi
        enemy.ApplyKnockback(playerToEnemy);

        // Entrée en combat
        GameManager.Instance.EnterCombat();

        // Vérification de la mort de l'ennemi
        if (enemy.CurrentHealth <= 0)
        {
            enemy.Die();
        }

    }
    public static void HandleHazard(Collision2D hazard, PlayerController player = null, EnemyController enemy = null, bool bumpBack = false)
    {
        Vector2 hazardToEnemy = (enemy.transform.position - hazard.transform.position).normalized;


    }

    public static void HandleAttackBoss(PlayerController player, BossAI boss)
    {

        //Calcul de l'angle de l'attaque
        Vector2 playerToEnemy = (boss.transform.position - player.transform.position).normalized;
        playerToEnemy = GetCardinalDirection(playerToEnemy);
        float dotProduct = Vector2.Dot(boss.forwardDirection, playerToEnemy);

        // Calcul des dégâts selon l'angle
        float damageMultiplier = 1f;

        //Pas de dégats au joueur lors d'une attaque de boss et pas de knockback
        if (dotProduct < -0.7f)
        {
            //Attaque dans le dos 
            damageMultiplier = 2f;
        }
        else
        {
            //Attaque de côté
            damageMultiplier = 1.5f;
        }

        // Application des dégâts à l'ennemi
        int finalDamage = (int)(player.playerStats.totalStats.atk * damageMultiplier);
        boss.Damage(finalDamage);

        // Trigger des passifs de l'équipement du joueur ( OnHit )
        if (player.equipment != null)
        {
            player.equipment.TriggerPassives(EquipmentTriggerType.OnHit, boss.gameObject, finalDamage);
        }

        // Entrée en combat
        GameManager.Instance.EnterCombat();

        // Vérification de la mort de l'ennemi
        if (boss.CurrentHealth <= 0)
        {
            boss.Die();
        }
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
