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

    public override void BeginCooldown(GameObject parent)
    {

        PlayerController player = parent.GetComponent<PlayerController>();

        player.isDashing = false;
        player.rb.velocity = Vector2.zero;
        player.canDash = true;
    }
}
