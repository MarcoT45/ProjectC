using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootHitbox : MonoBehaviour
{
    [HideInInspector]public int damage = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Damage(damage);
            }
        }
    }
     
}
