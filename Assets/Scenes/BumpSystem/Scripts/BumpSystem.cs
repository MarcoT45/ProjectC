using UnityEngine;
using System.Collections;

public static class BumpSystem
{
    public static void HandleBump(PlayerController player, EnemyController enemy)
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
            player.TakeDamage(enemy.damage); // Le joueur prend des dégâts
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
        enemy.TakeDamage(finalDamage);
        enemy.ApplyKnockback(playerToEnemy);

        if (enemy.health <= 0)
        {
            GameObject.Destroy(enemy.gameObject);
        }

    }
    public static void HandleHazard(Collision2D hazard, PlayerController player = null, EnemyController enemy = null, bool bumpBack = false)
    {
        Vector2 hazardToEnemy = (enemy.transform.position - hazard.transform.position).normalized;


    }

    /*    public static void HandleBump(PlayerController player, EnemyController enemy)
        {
            Vector2 enemyToPlayer = (player.transform.position - enemy.transform.position).normalized;

            float playerAttackAngle = Vector2.Dot(player.forwardDirection, enemyToPlayer);

            float damageMultiplier;
            if (playerAttackAngle > 0.7f)
            {
                damageMultiplier = 1f;
                player.TakeDamage(enemy.damage);
                player.ApplyKnockback(-enemyToPlayer);
            }
            else if (playerAttackAngle < -0.7f)
            {
                damageMultiplier = 2f;
            }
            else
            {
                damageMultiplier = 1.5f;
            }

            int finalDamage = (int)(player.damage * damageMultiplier);
            enemy.TakeDamage(finalDamage);
            ShowImpact(enemy.transform.position);

            Debug.Log("Knockback appliqué !");
            enemy.ApplyKnockback(enemyToPlayer);
            player.ApplyKnockback(-enemyToPlayer * 0.75f);
        }
    */


    /*  Déplacé dans DashAbility
    
    public static void HandleDash(PlayerController player)
    {
        if (!player.canDash)
            return;

        player.canDash = false;
        player.isDashing = true;

        Vector2 dashDirection = player.forwardDirection;
        player.rb.velocity = dashDirection * player.dashForce;

        player.StartCoroutine(DashCoroutine(player, dashDirection));
    }

    private static IEnumerator DashCoroutine(PlayerController player, Vector2 dashDirection)
    {
        float elapsedTime = 0f;
        while (elapsedTime < player.dashDuration)
        {
            elapsedTime += Time.deltaTime;
            // Vérifier si une collision avec un ennemi survient pendant le dash
            Collider2D hit = Physics2D.OverlapCircle(player.transform.position, 0.5f, LayerMask.GetMask("Enemy"));
            if (hit != null)
            {
                EnemyController enemy = hit.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage((int)(player.damage * 2)); // Le dash fait plus de dégâts
                    enemy.ApplyKnockback(dashDirection * 1.5f);
                    player.ApplyKnockback(-dashDirection * 0.75f);
                    break;
                }
            }
            yield return null;
        }
        player.isDashing = false;
        player.rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(player.dashCooldown);
        player.canDash = true;
    }
    */

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
