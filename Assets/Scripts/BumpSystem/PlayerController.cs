using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IShopCustomer, IDamageable
{
    [HideInInspector] public PlayerStats playerStats;
    [HideInInspector] public EquipmentController equipment;

    [Header("Settings")]

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


    // IDamageable implementation ( à voir si utile )
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float MaxShield { get; set; }
    public float CurrenShield { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        forwardDirection = Vector2.right;
        playerStats = GetComponent<PlayerStats>();
        equipment = GetComponent<EquipmentController>();
    }

    private void Start()
    {
        // Assigner la caméra principale pour suivre le joueur
        if ( Camera.main.GetComponent<CameraFollow>() != null)
        {
            Camera.main.GetComponent<CameraFollow>().target = this.transform;
        }
        else
        {
            Camera.main.gameObject.AddComponent<CameraFollow>().target = this.transform;
        }

        //Stats initiales
        MaxHealth = playerStats.totalStats.pv;
        CurrentHealth = MaxHealth;
        MaxShield = playerStats.totalStats.shield;
        CurrenShield = MaxShield;
    }

    private void Update()
    {
        // Ne rien faire si le jeu est en pause
        if (GameManager.Instance.GameIsPaused)
            return;

        // Mouvement avec le ControlsManager
        if (ControlsManager.Instance.controlsState == ControlsState.CharacterHub || ControlsManager.Instance.controlsState == ControlsState.Combat)
        {
            if( ControlsManager.Instance.DeplacerHold)
            {
                float moveX = ControlsManager.Instance.DeplacerValue.x;
                float moveY = ControlsManager.Instance.DeplacerValue.y;
                movement = new Vector2(moveX, moveY).normalized;
                if (movement != Vector2.zero)
                {
                    forwardDirection = movement;
                }
            }
            else
            {
                movement = Vector2.zero;
            }
        }

    }

    void FixedUpdate()
    {
        if (!isDashing && !isKnockedBack)
        {
            currentVelocity = Vector2.Lerp(currentVelocity, movement * playerStats.totalStats.spd , 0.1f);
            rb.velocity = movement * playerStats.totalStats.spd;
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
                // Déclencher les passifs liés à la collecte de pièces
                EquipmentController.Instance.TriggerPassives(EquipmentTriggerType.OnPickup, collectible.gameObject, 0);
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
    public void Heal(float amount)
    {
        CurrentHealth += (int)amount;
        //Clamp la vie actuelle à la vie max
        CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);
    }

    public void Damage(int amount)
    {
        if( CurrenShield > 0)
        {
            //Deux façons de gérer les dégâts sur le bouclier : soit on enlève 1 point de bouclier par attaque,
            //soit on enlève autant de points que de dégâts reçus
            //A voir ce qui est le plus intéressant en termes de gameplay
            int shieldDamage = Mathf.Min(1, (int)CurrenShield);
            //int shieldDamage = Mathf.Min(amount, (int)CurrenShield);

            CurrenShield -= shieldDamage;
        }else
        {
            CurrentHealth -= (int)amount;
        }

        //StartCoroutine(BlinkRoutine());
        if (CurrentHealth <= 0)
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

    public void BuyItem(ItemData itemData)
    {
        GameManager.Instance.SetRunPlayerCoins(GameManager.Instance.GetRunPlayerCoins() - itemData.GetPrice());
        InventoryController.Instance.AddItem(itemData);
       //uiInventory.RefreshInventoryItems();
    }

    public void Die()
    {
        Destroy(gameObject);

        //Changer de scène ou afficher un écran de fin de jeu
        //Changer état du ControlsManager
    }
}
