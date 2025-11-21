using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootHitbox : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int damage = Mathf.RoundToInt(this.transform.parent.GetComponent<Boss1>().monsterData.atk);
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().Damage(damage);
        }
    }

}
