using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;
    public int damage = 5;
    public int health = 50;
    public float knockbackDuration = 0.3f;
    public float knockbackForce = 3f;
    public Vector2 forwardDirection {  get; private set; }

    private Rigidbody2D rb;
    private Transform player;
    private bool isKnockedBack = false;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem hitParticles;

    //[Header("References")]
    public EnemyAI enemyAI; // Composant à ajouter sur le GO, le nom sera changé
   
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (enemyAI == null)
            enemyAI = GetComponent<EnemyAI>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        forwardDirection = Vector2.right;
        hitParticles = GetComponentInChildren<ParticleSystem>(); // Récupère le système de particules
    }
   
    void Update()
    {
    }

    void FixedUpdate()
    {
        /*
        if (!isKnockedBack)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            forwardDirection = direction;
            rb.velocity = direction * speed;
        }
        */

        if (isKnockedBack)
            return;

        Vector2 targetPos;
        if (enemyAI != null)
        {
            if (enemyAI.currentState == EnemyState.Chase && enemyAI.currentPath != null && enemyAI.currentPath.Count > 0)
            {
                // Suivre le chemin calculé
                if (enemyAI.currentPathIndex < enemyAI.currentPath.Count)
                {
                    targetPos = enemyAI.currentPath[enemyAI.currentPathIndex];
                    if (Vector2.Distance(transform.position, targetPos) < 0.3f)
                        enemyAI.currentPathIndex++;
                }
                else
                {
                    targetPos = enemyAI.player.position;
                }
            }
            else // Mode Patrol
            {
                targetPos = enemyAI.patrolTarget;
            }
        }
        else
        {
            targetPos = transform.position;
        }
        Vector2 direction = ((Vector2)targetPos - rb.position).normalized;
        //rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                PlayHitEffect(collision.GetContact(0).point);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        StartCoroutine(BlinkRoutine());
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void ApplyKnockback(Vector2 direction)
    {
           // Debug.Log("Knockback Direction: " + direction); // Vérification
            StartCoroutine(KnockbackCoroutine(direction));
 
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction)
    {
        isKnockedBack = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        //Debug.Log("Knockback Applied: " + rb.velocity);

        yield return new WaitForSeconds(knockbackDuration);

        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }

    private IEnumerator BlinkRoutine()
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void PlayHitEffect(Vector2 hitPosition)
    {
        if (hitParticles != null)
        {
            hitParticles.transform.position = hitPosition; // Positionne les particules au point de contact
            hitParticles.Play(); // Lance l'effet
        }
    }
}
