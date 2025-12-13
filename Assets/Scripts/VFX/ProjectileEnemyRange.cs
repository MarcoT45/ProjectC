using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyRange : MonoBehaviour {

    public float speed = 5f;
    public float timeRemaining = 5f;
    private Vector2 direction;

    public void Initialize(Vector2 dir) {
        direction = dir.normalized;
    }

    private void FixedUpdate() {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
        TimerBeforeDestroy();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.Damage(1);
            Destroy(gameObject);
        } else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            Destroy(gameObject); // Le projectile s’arrête sur un obstacle
        }
    }

    // Si jamais on esquive, il faut détruire l'objet après un lapse de temps (5s) pour pas saturer la mémoire
    private void TimerBeforeDestroy() {
        if (timeRemaining > 0) {
            timeRemaining -= Time.deltaTime;
        } else {
            Destroy(gameObject);
        } 
    }

}