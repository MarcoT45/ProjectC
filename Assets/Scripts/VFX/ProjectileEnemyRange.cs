using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyRange : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject); // Le projectile s’arrête sur un obstacle
        }
    }
}
