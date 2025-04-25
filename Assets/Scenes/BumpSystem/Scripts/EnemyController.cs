using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;

public class EnemyController : MonoBehaviour
{
/*
    [Header("AI Settings")]
    public EnemyState currentState = EnemyState.Patrol;
    public float chaseDistance = 8f;              // Distance à partir de laquelle on passe en mode Chase
    public float pathUpdateInterval = 0.5f;         // Fréquence de mise à jour du pathfinding en Chase
    private float pathUpdateTimer = 0f;*/

    [Header("Pathfinding")]
    private GridManager2 gridManager;
    private List<Node> path;
    private int currentPathIndex = 0;
    public float pathRefreshInterval = 0.5f;
    private float pathRefreshTimer = 1f;

   /* [Header("Patrol")]
    public Vector3 patrolTarget;                  // Cible de patrouille aléatoire*/

    public float speed = 3f;
    public int damage = 5;
    public int health = 50;
    public float knockbackDuration = 0.3f;
    public float knockbackForce = 3f;
    public Vector2 forwardDirection {  get; private set; }
    public LayerMask wallLayerMask;

    private Rigidbody2D rb;
    private Transform player;
    private bool isKnockedBack = false;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem hitParticles;

    //[Header("References")]
    public EnemyAI enemyAI; // Composant à ajouter sur le GO, le nom sera changé
   
    void Awake()
    {
        gridManager = FindObjectOfType<GridManager2>();

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

        if (isKnockedBack) return;

        pathRefreshTimer += Time.deltaTime;
        if (pathRefreshTimer >= pathRefreshInterval)
        {
            path = AStarPathfinding.FindPath(gridManager, transform.position, player.position);
            currentPathIndex = 0;
            pathRefreshTimer = 0f;

            Debug.Log("Path count : "+path.Count);
        }

        // Vous pouvez ensuite déplacer l’ennemi le long du chemin calculé...
        if (path != null && path.Count > 0 && currentPathIndex < path.Count)
        {
            Vector3 targetPosition = gridManager.CellToWorld(path[currentPathIndex].cellPosition);

            Debug.Log("targetPosition : "+ targetPosition);
            // Appliquer un déplacement vers targetPosition
             rb.velocity = (targetPosition - transform.position).normalized * speed;
            // rb.AddForce((targetPosition - transform.position).normalized * speed, ForceMode2D.Force);

            // Si suffisamment proche du prochain point du chemin, on avance au suivant
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                currentPathIndex++;
            }
        }

        /*
        if (!isKnockedBack)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            forwardDirection = direction;
            rb.velocity = direction * speed;
        }
        */
        /*
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
                Vector2 direction = ((Vector2)targetPos - rb.position).normalized;*/
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

        if (isKnockedBack)
        {
            if (collision.gameObject.CompareTag("Mur"))
            {
                Debug.Log("Rebond !");
                Vector2 direction = ((Vector2)this.transform.position - collision.GetContact(0).point).normalized;
                ApplyKnockback(direction);
                Debug.Log(direction);
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
        rb.velocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        //Debug.Log("Knockback Applied: " + rb.velocity);

        // Debug.Log("Knockback Direction: " + direction); // Vérification
        StartCoroutine(KnockbackCoroutine(direction));
 
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction)
    {
        isKnockedBack = true;

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
