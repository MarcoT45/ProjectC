using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IShopCustomer, IDamageable {

    [HideInInspector] public PlayerStats playerStats;
    [HideInInspector] public EquipmentController equipment;

    [Header("References")]
    //public Transform firePoint; // Point d'origine des projectiles ( à définir si besoin)
    public Animator animator;
    public GridManager gridManager;
    public Collider2D playerCollider;

    [Header("Settings")]

    public float dashForce = 8f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;
    public float knockbackForce = 2f;
    public float knockbackForceWeak = 0.5f;
    public float knockbackDuration = 0.2f;
    public AnimationCurve knockbackCurve;
    public float coolDownAttack = 0.5f;

    private float timerAttack = 0f;

    [Header("VFX")]
    public GameObject hitVFX;
    public GameObject projectilePrefab; // Prefab projectile

    protected Material material;
    [SerializeField] protected float tintFadeSpeed = 0.5f;
    [SerializeField] protected Color tintColor = Color.white;
    public Rigidbody2D rb;


    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public  Vector2 forwardDirection;
    [HideInInspector] public bool canDash = true;
    [HideInInspector] public bool isDashing = false;
    [HideInInspector] public bool isHurt;
    [HideInInspector] public bool isKnockedBack = false;
    [HideInInspector] public bool isAttacking = false;
    private Vector2 currentVelocity = Vector2.zero;
    private SpriteRenderer spriteRenderer;

    private float wallOffset = 0.1f;
    private LayerMask wallLayerMask;
    private ContactFilter2D contactFilter;
    private RaycastHit2D[] hits = new RaycastHit2D[4];


    // IDamageable implementation ( à voir si utile )
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float MaxShield { get; set; }
    public float CurrenShield { get; set; }

    private void Awake() {
        gridManager = GameObject.FindWithTag("GridManager").GetComponent<GridManager>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        material = spriteRenderer.material;
        forwardDirection = Vector2.right;
        playerStats = GetComponent<PlayerStats>();
        equipment = GetComponent<EquipmentController>();

        rb.simulated = false;

        wallLayerMask = LayerMask.GetMask("Mur");
        playerCollider = GetComponent<Collider2D>();    
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = wallLayerMask;
        contactFilter.useTriggers = false;

    }

    private void Start()
    {
        // Assigner la caméra principale pour suivre le joueur
        if ( Camera.main.GetComponent<CameraFollowTarget>() != null) {
            Camera.main.GetComponent<CameraFollowTarget>().target = this.gameObject;
        } else {
            Camera.main.gameObject.AddComponent<CameraFollowTarget>().target = this.gameObject;
        }

        //Stats initiales
        MaxHealth = playerStats.totalStats.pv;
        CurrentHealth = MaxHealth;
        MaxShield = playerStats.totalStats.shield;
        CurrenShield = MaxShield;

        rb.position = transform.position;
        rb.simulated = true;

    }

    private void Update() {

        // Ne rien faire si le jeu est en pause
        if (GameManager.Instance.GameIsPaused)
            return;

        // Mouvement avec le ControlsManager
        if (ControlsManager.Instance.controlsState == ControlsState.CharacterHub || ControlsManager.Instance.controlsState == ControlsState.Combat)
        {

            if (ControlsManager.Instance.DeplacerHold)
            {
                float moveX = ControlsManager.Instance.DeplacerValue.x;
                float moveY = ControlsManager.Instance.DeplacerValue.y;
                moveInput = new Vector2(moveX, moveY).normalized;
                if (moveInput != Vector2.zero)
                {
                    forwardDirection = moveInput;
                }

                Animating(moveX, moveY);
            }
            else
            {
                moveInput = Vector2.zero;
                Animating(moveInput.x, moveInput.y);
            }

            if (isHurt) moveInput = Vector2.zero;
        }

        //Mettre à jour MaxHealth et MaxShield en fonction des stats totales si elles ont changées
        if (MaxHealth != playerStats.totalStats.pv)
        {
            MaxHealth = playerStats.totalStats.pv;
            if (CurrentHealth >= MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }

        if (MaxShield != playerStats.totalStats.shield)
        {
            MaxShield = playerStats.totalStats.shield;
            if (CurrenShield >= MaxShield)
            {
                CurrenShield = MaxShield;
            }
        }
    }   

    private void FixedUpdate() {

        if(isKnockedBack) return;

        // Déplacement du RB
        if (ControlsManager.Instance.controlsState == ControlsState.CharacterHub || ControlsManager.Instance.controlsState == ControlsState.Combat)
        {
            Vector2 movement = moveInput * playerStats.totalStats.spd * Time.fixedDeltaTime;
            
            rb.MovePosition((Vector2)transform.position + movement);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                BumpSystem.HandleBump(this, enemy);
                ShowImpact(enemy.transform.position);
                isAttacking = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isAttacking = false;
        }
    }

    public void Collect(Collectible collectible) {
        switch (collectible.collectibleType) {
            case CollectibleType.Coin:
                GameManager.Instance.AddCoinsToRunPlayerCoins(collectible.amount);
                // Déclencher les passifs liés à la collecte de pièces
                EquipmentController.Instance.TriggerPassives(EquipmentTriggerType.OnPickup, collectible.gameObject, 0);
                break;
            case CollectibleType.Loot:
                GameManager.Instance.AddCoinsToRunPlayerCoins(collectible.amount);
                break;
        }
    }

    private void ShowImpact(Vector2 position) {
        GameObject impactEffect = GameObject.Instantiate(hitVFX, position, Quaternion.identity);
        GameObject.Destroy(impactEffect, 0.2f);
    }

    public void Heal(float amount) {
        CurrentHealth += (int)amount;
        //Clamp la vie actuelle à la vie max
        CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);
    }

    public void Damage(int amount) {
        if(CurrenShield > 0) {
            //Deux façons de gérer les dégâts sur le bouclier : soit on enlève 1 point de bouclier par attaque,
            //soit on enlève autant de points que de dégâts reçus
            //A voir ce qui est le plus intéressant en termes de gameplay
            int shieldDamage = Mathf.Min(1, (int)CurrenShield);
            //int shieldDamage = Mathf.Min(amount, (int)CurrenShield);

            CurrenShield -= shieldDamage;
            //clamp le bouclier actuel à 0
            CurrenShield = Mathf.Max(CurrenShield, 0);
        } else {
            CurrentHealth -= (int)amount;
            //clamp la vie actuelle à 0
            CurrentHealth = Mathf.Max(CurrentHealth, 0);

            isHurt = true;
            StartCoroutine(HitFlash());

        }

        if (CurrentHealth <= 0) {
            Die();
        }
    }

    public void ApplyKnockback(Vector2 direction, bool isKBWeak) {

        rb.velocity = Vector2.zero;
        if (!isDashing && !isKnockedBack) {
            StartCoroutine(KnockbackCoroutine(direction, isKBWeak));
        }
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction, bool isKBWeak)
    {

        isKnockedBack = true;

        Vector2 startPos = this.rb.position;
        Vector2 targetPos = startPos;

        if (isKBWeak)
        {
            targetPos = (Vector2)transform.position + direction * knockbackForceWeak;
        }
        else
        {
            targetPos = (Vector2)transform.position + direction * knockbackForce;
        }

            //Boucle pour vérifier chaque case entre la position de départ et la position cible. Pour les murs
            for (int i = 1; i <= knockbackForce; i++)
            {
                Vector2 intermediatePos = (Vector2)transform.position + direction * i;
                if (gridManager.GetNodeFromWorldPoint(intermediatePos).walkable == false)
                {
                    targetPos = (Vector2)transform.position + direction * (i - 1);
                    break;
                }
            }

        float elapsed = 0f;

        while (elapsed < knockbackDuration)
        {
            float t = elapsed / knockbackDuration;
            float curveValue = knockbackCurve.Evaluate(t);
            Vector2 newPos = Vector2.Lerp(startPos, targetPos, curveValue);
            rb.MovePosition(newPos);

            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(targetPos);
        isKnockedBack = false;

    }

    private IEnumerator HitFlash()
    {
        material.SetColor("_Tint", tintColor);
        Color tempColor;
        tempColor = tintColor;

        float time = 0f;
        while (time < tintFadeSpeed)
        {
            time += Time.fixedDeltaTime;
            tempColor.a = Mathf.Lerp(tintColor.a, 0f, (time / tintFadeSpeed));
            material.SetColor("_Tint", tempColor);
            yield return null;
        }

        isHurt = false;
    }

    public void BuyItem(ItemData itemData) {
        GameManager.Instance.SetRunPlayerCoins(GameManager.Instance.GetRunPlayerCoins() - itemData.GetPrice());
        InventoryController.Instance.AddItem(itemData);
       //uiInventory.RefreshInventoryItems();
    }

    public void Die() {
        gameObject.SetActive(false);
        // Destroy(gameObject);

        //Changer de scène ou afficher un écran de fin de jeu
        //Changer état du ControlsManager
    }

    private void Animating(float h, float v)
    {
        Vector2 movement = new Vector2(h, v);

        if(movement.magnitude > 1f)
        {
            movement = movement.normalized;
        }

        movement = transform.InverseTransformDirection(movement);
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
    }

    //DEBUG GUI POUR AFFICHER LA VIE DU PLAYER
    /* private void OnGUI()
     {
         GUIStyle gUIStyle = new GUIStyle();
         gUIStyle.fontSize = 12;
         gUIStyle.normal.textColor = Color.yellow;
         float x = 10f;
         float y = 10f;

         GUI.Label(new Rect(x,y,200,50), $"PLAYER HP: {this.CurrentHealth}", gUIStyle);
     }*/
}