using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Flash", menuName = "ScriptableObjects/Abilities/Flash")]
public class FlashAbility : Ability
{
    public float flashDistance = 2f;
    public LayerMask obstacleLayer;

    public override void Activate(GameObject parent)
    {
        PlayerController player = parent.GetComponent<PlayerController>();
        Vector2 flashDirection = player.forwardDirection;
        Vector2 start = player.transform.position;
        Vector2 target = start + flashDirection.normalized * flashDistance;

        Collider2D wall;
        Vector2 currentPosition = target;

        while((wall = Physics2D.OverlapCircle(currentPosition,0.1F, obstacleLayer)) != null)
        {
            currentPosition -= flashDirection.normalized * 0.1f;

            if(Vector2.Distance(currentPosition, start) < 0.1F)
            {
                currentPosition = start;
                break;
            }
        }

        player.transform.position = currentPosition;
    }

    public override void BeginCooldown(GameObject parent)
    {
        PlayerController player = parent.GetComponent<PlayerController>();
        player.rb.velocity = Vector2.zero;
    }

}
