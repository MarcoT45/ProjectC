using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{


    [Header("Settings")]
    public float speed = 5f;
    public int damage = 10;
    public int health = 100;

    public float dashForce = 8f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;
    public float knockbackForce = 5f;
    public float knockbackDurationFront = 0.05f;
    public float knockbackDurationSide = 0.3f;

    [Header("VFX")]
    public Rigidbody2D rb;
    public GameObject hitVFX;
    public GameObject projectilePrefab; // Prefab projectile

    [HideInInspector] public Vector2 movement;
    [HideInInspector] public  Vector2 forwardDirection;
    [HideInInspector] public bool canDash = true;
    [HideInInspector] public bool isDashing = false;

    private bool isSpeedBoosted = false;    
    private bool isKnockedBack = false;
    private Vector2 currentVelocity = Vector2.zero;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        forwardDirection = Vector2.right;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveX, moveY).normalized;
        if (movement != Vector2.zero)
        {
            forwardDirection = movement;
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.Space))
        {

            // BumpSystem.HandleDash(this);
        }

        // Projectile
      /*  if (Input.GetKeyDown(KeyCode.E))
        {
            FireProjectile();
        }*/
    }

    void FixedUpdate()
    {
        if (!isDashing && !isKnockedBack)
        {
            currentVelocity = Vector2.Lerp(currentVelocity, movement * speed, 0.1f);
            rb.velocity = movement * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                BumpSystem.HandleBump(this, enemy);
                ShowImpact(enemy.transform.position);
            }
        }
    }

    public void Collect(Collectible collectible)
    {
        switch (collectible.collectibleType)
        {
            case CollectibleType.Coin:
                GameManager.Instance.AddCoinsToRunPlayerCoins( collectible.amount);
                break;

            case CollectibleType.Loot:
                GameManager.Instance.AddCoinsToRunPlayerCoins(collectible.amount);
                break;
        }
    }

    private void ShowImpact(Vector2 position)
    {
        GameObject impactEffect = GameObject.Instantiate(hitVFX, position, Quaternion.identity);
        GameObject.Destroy(impactEffect, 0.2f);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        //StartCoroutine(BlinkRoutine());
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void ApplyKnockback(Vector2 direction)
    {
        if (!isDashing)
        {
            StartCoroutine(KnockbackCoroutine(direction));
        }
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction)
    {
        isKnockedBack = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        Debug.Log("direction.magnitude: " + direction.magnitude);

        if (direction.magnitude < 0.5)
        {
            yield return new WaitForSeconds(knockbackDurationSide);
        }
        else
        {
            yield return new WaitForSeconds(knockbackDurationFront);
        }

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

    private void FireProjectile()
    {
        // Le projectile apparaît à une courte distance devant le joueur
        Vector3 spawnPos = transform.position + (Vector3)(forwardDirection.normalized * 0.5f);
        // Instantiate le projectile
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        // Récupère le script Projectile pour définir sa direction
        Projectile projectileScript = proj.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.direction = forwardDirection;
        }
    }

    private void ActivateSpeedBoost(float duration)
    {
        if (!isSpeedBoosted)
        {
           // StartCoroutine();
        }
    }
}
