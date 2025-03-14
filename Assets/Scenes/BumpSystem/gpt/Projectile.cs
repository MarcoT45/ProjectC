using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;       // Vitesse du projectile
    public int damage = 5;          // Dégâts infligés
    [HideInInspector]
    public Vector2 direction;       // Direction dans laquelle le projectile va se déplacer

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Appliquer une vitesse constante dans la direction souhaitée
        rb.velocity = direction.normalized * speed;
    }

    // Le projectile est en mode Trigger pour détecter les collisions sans utiliser la physique standard
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si le projectile touche un ennemi, inflige des dégâts
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        // Quel que soit l'objet touché (sauf le joueur), détruire le projectile
        if (!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
