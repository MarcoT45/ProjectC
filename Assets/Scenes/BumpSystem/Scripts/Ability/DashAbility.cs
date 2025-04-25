using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dash", menuName = "ScriptableObjects/Abilities/Dash")]
public class DashAbility : Ability
{
    public float dashVelocity;
    public override void Activate(GameObject parent)
    {
        PlayerController player = parent.GetComponent<PlayerController>();

        if (!player.canDash)
            return;

        player.canDash = false;
        player.isDashing = true;

        Vector2 dashDirection = player.forwardDirection;
       // player.rb.velocity = dashDirection * player.dashForce;
        player.rb.velocity = dashDirection * dashVelocity;

       // player.StartCoroutine(DashCoroutine(player, dashDirection));
    }


    //A voir si toujours de l"interet
    /*
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

    public override void BeginCooldown(GameObject parent)
    {

        PlayerController player = parent.GetComponent<PlayerController>();

        player.isDashing = false;
        player.rb.velocity = Vector2.zero;
        player.canDash = true;
    }
}
