using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System;

public abstract class EnemyAI : MonoBehaviour, IDamageable, IEnnemyMoveable {

    [Header("Data")]
    public MonsterData monsterData;
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public int Coins { get; set; }
    public Vector2 forwardDirection;
    public float movementRange = 5f;
    public bool isSearching = false;
    public bool isAlerted = false;
    public float aggroDuration;
    public float attackCoolDown;
    public float fireRate;
    private SpriteRenderer spriteRenderer;

    [Header("Hit variables")]
    public bool isKnockedBack = false;
    public float knockbackDuration = 0.5f;
    public float knockbackForce = 3f;

    [Header("AI Settings")]
    public float alertDistance;             // Distance à partir de laquelle on passe en mode Chase / = aggroRange
    public float safeRange;                 // Distance à laquelle l'ia se considere safe
    public float pathUpdateInterval = 0.5f; // Fréquence de mise à jour du pathfinding en Chase

    [Header("Pathfinding")]
    public GridManager gridManager;
    private Vector3 targetPosition;
    private List<Node> path;
    private int currentPathIndex = 0;
    public float pathRefreshInterval = 0.5f;
    private float pathRefreshTimer = 1f;

    [Header("VFX")]
    public Animator animator;
    public GameObject slashVFXPrefab;
    public GameObject stepVFXPrefab;
    public GameObject projectilePrefab;
    private ParticleSystem hitParticles;
    protected Material material;
    [SerializeField] protected Color tintColor = Color.red;
    [SerializeField] protected float tintFadeSpeed = 0.5f;

    #region State Machine variables

    [Header("Bubble")]
    public GameObject bubbleIdle;
    public GameObject bubbleSearch;

    public EnemyStateMachine StateMachine { get; set; }

    //Mis dans les héritiers

    public EnemyState PatrolState { get; set; }
    public EnemyState PatrolIdleState { get; set; }
    public EnemyState PatrolWalkState { get; set; }
    public EnemyState SearchState { get; set; }
    public EnemyState SearchLookUpState { get; set; }
    public EnemyState SearchLookDownState { get; set; }
    public EnemyState SearchLookLeftState { get; set; }
    public EnemyState SearchLookRightState { get; set; }
    public EnemyState SearchWalkState { get; set; }
    public EnemyState AlertedState { get; set; }
    public EnemyState AlertedLookingState { get; set; }
    public EnemyState AttackState { get; set; }

    #endregion

    [HideInInspector] public Vector3 playerSeachPosition;               // Position ou le joueur a été vu dans le Search ou le Alert
    [HideInInspector] public int searchLookCounter = 4;                 // Compteur du nombre de position ou l'ennemi peut regarder en SearchState
    [HideInInspector] public float speedBoostAlertMultiplicator = 1.0f; // Multiplicateur de vitesse de déplacement de l'ennemi

    [HideInInspector] public Transform player;  // Référence au joueur (assignée dans Start)
    [HideInInspector] public Transform loot;    // Référence au collectible visible
    public LayerMask wallLayerMask;
    [HideInInspector] public Rigidbody2D rb;

    public Action OnMoveDestinationReached;
    public static int AliveEnemyCount = 0;
    private PoolEnemy poolEnemy;

    #region Awake/Start/Update
    private void OnEnable()
    {
        AliveEnemyCount++;
    }
    private void OnDisable()
    {
        AliveEnemyCount--;
    }

    protected virtual void Awake() {
        gridManager = GameObject.FindWithTag("GridManager").GetComponent<GridManager>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        forwardDirection = Vector2.right;
        hitParticles = GetComponentInChildren<ParticleSystem>(); // Récupère le système de particules

        //Ennemy
        StateMachine = new EnemyStateMachine();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    protected virtual void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        MaxHealth = monsterData.pv;
        CurrentHealth = MaxHealth;
        forwardDirection = Vector2.right;

        StateMachine.Initialize(PatrolState);
    }

    protected virtual void Update() {
        //Juste pour voir où il regarde
        Debug.DrawRay(transform.position, forwardDirection, Color.blue);

        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    protected virtual void FixedUpdate() {
        StateMachine.CurrentEnemyState.FrameFixedUpdate();
    }

    #endregion

    #region Move

    public void SetTargetPosition(Vector3 newTarget) {
        this.targetPosition = newTarget;
    }

    // Déplace l'ennemi le long du chemin calculé
    public void Move() {

        if (isKnockedBack) return;

        pathRefreshTimer += Time.deltaTime;
        if (pathRefreshTimer >= pathRefreshInterval) {
            path = AStarPathfinding.FindPath(gridManager, transform.position, this.targetPosition);
            currentPathIndex = 0;
            pathRefreshTimer = 0f;
        }

        if (path != null && path.Count > 0 && currentPathIndex < path.Count) {
            Vector3 targetTmpPosition = gridManager.CellToWorld(path[currentPathIndex].cellPosition);

            rb.velocity = (targetTmpPosition - transform.position).normalized * monsterData.speed * speedBoostAlertMultiplicator;

            // Pour changer la direction ou l'ennemi regarde
            float upValue, downValue, rightValue, leftValue = 0f;

            upValue = Vector2.Dot(Vector2.up, (targetTmpPosition - transform.position).normalized);
            downValue = Vector2.Dot(Vector2.down, (targetTmpPosition - transform.position).normalized);
            rightValue = Vector2.Dot(Vector2.right, (targetTmpPosition - transform.position).normalized);
            leftValue = Vector2.Dot(Vector2.left, (targetTmpPosition - transform.position).normalized);

            float maxValue = Mathf.Max(upValue, downValue, rightValue, leftValue);

            if (maxValue == upValue) {
                forwardDirection = Vector2.up;
            } else if (maxValue == downValue) {
                forwardDirection = Vector2.down;
            } else if (maxValue == rightValue) {
                forwardDirection = Vector2.right;
            } else if (maxValue == leftValue) {
                forwardDirection = Vector2.left;
            } else {
                forwardDirection = Vector2.zero;
            }

            // Si suffisamment proche du prochain point du chemin, on avance au suivant
            if (Vector3.Distance(transform.position, targetTmpPosition) < 0.1f) {
                currentPathIndex++;

                // Arrivé à destination ?
                if (currentPathIndex >= path.Count) {
                    path = null;
                    currentPathIndex = 0;
                    rb.velocity = Vector2.zero;
                    OnMoveDestinationReached.Invoke();
                }
            }
        } else if (path != null && currentPathIndex == path.Count) {
            // Dans le cas ou l'ennemi se coince dans un mur
            path = null;
            currentPathIndex = 0;
            rb.velocity = Vector2.zero;
            OnMoveDestinationReached.Invoke();
        }
    }

    #endregion

    #region Collision functions

    private void OnCollisionEnter2D(Collision2D collision) {

        // Pour bloquer les collisions entre les ennemies
        if (collision.gameObject.CompareTag("Enemy")) {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }

        // Si on rentre en contact avec le joueur
        if (collision.gameObject.CompareTag("Player")) {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null) {
                PlayHitEffect(collision.GetContact(0).point);
            }
        }

        if (isKnockedBack) {
            //-------------- Collision avec un mur ( rétiré pour le moment ) 
            /*if (collision.gameObject.CompareTag("Mur")) {
                Debug.Log("Collision Mur");
                //Dégat à voir si en fonction du joueur ou du mur
                Damage(5);

                //Code pour le rebond
                //Vector2 direction = ((Vector2)this.transform.position - collision.GetContact(0).point).normalized;
                //ApplyKnockback(direction);
            }*/
            //---------------

            //---------------- Collision avec autre ennemi ( rétiré pour le moment ) 
            /*if (collision.gameObject.CompareTag("Ennemi")) {
                Debug.Log("Collision Ennemi");
                EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
                if (enemy != null) {
                    Damage((int)enemy.monsterData.atk);
                    enemy.Damage((int)this.monsterData.atk);
                }
            }*/
            //---------------
        }
    }

    #endregion

    #region Health / Die functions

    public void Damage(int damage) {
        CurrentHealth -= damage;

        //StartCoroutine(BlinkRoutine());
        StartCoroutine(HitFlash());

        if (CurrentHealth <= 0f) {
            Die();
        }
    }

    public void Die() {
        if(poolEnemy != null) {
            CurrentHealth = MaxHealth;
            poolEnemy.HideEnemy(this.gameObject);
        }else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region VFX functions

    public virtual void ApplyKnockback(Vector2 direction)
    {
        Debug.Log("Applying Knockback to Enemy");
        rb.velocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        //Debug.Log("Knockback Applied: " + rb.velocity);

        // Debug.Log("Knockback Direction: " + direction); // Vérification
        StartCoroutine(KnockbackCoroutine(direction));

    }

    private IEnumerator KnockbackCoroutine(Vector2 direction) {
        isKnockedBack = true;

        yield return new WaitForSeconds(knockbackDuration);
        rb.velocity = Vector2.zero;

        isKnockedBack = false;
    }

    private IEnumerator BlinkRoutine() {
        for (int i = 0; i < 3; i++) {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }


    private IEnumerator HitFlash()
    {
        material.SetColor("_Tint", tintColor);
        Color tempColor;
        tempColor = tintColor;

        float time = 0f;
        while (time < tintFadeSpeed)
        {
            time += Time.deltaTime;
            tempColor.a = Mathf.Lerp(tintColor.a, 0f, (time / tintFadeSpeed));
            material.SetColor("_Tint", tempColor);
            yield return null;
        }

    }

    private void PlayHitEffect(Vector2 hitPosition) {
        if (hitParticles != null) {
            hitParticles.transform.position = hitPosition; // Positionne les particules au point de contact
            hitParticles.Play(); // Lance l'effet
        }
    }

    #endregion

    #region Aggro functions

    // Renvoie vrai si l'ennemi apercoit le joueur dans la zone (rouge-bleu)
    public bool SearchLineOfSight(Vector2 lineOfSightDirection) {
        float viewAngle = 30f;
        int raycount = 3;
        float startAngle = -viewAngle / 2f;
        float angleIncrement = viewAngle / (raycount - 1);

        for (int i = 0; i < raycount; i++) {
            float angle = startAngle + angleIncrement * i;
            Vector2 rayDirection = RotateVector(lineOfSightDirection, angle);
            RaycastHit2D hitSearch = Physics2D.Raycast(transform.position, rayDirection, (alertDistance + 2f), LayerMask.GetMask("Mur", "Player"));

            Debug.DrawRay(transform.position, rayDirection * (alertDistance + 2f), Color.blue);
            Debug.DrawRay(transform.position, rayDirection * alertDistance, Color.red);

            if (hitSearch.collider != null) {
                if (hitSearch.collider.CompareTag("Player") || hitSearch.collider.transform.parent.CompareTag("Player")) {
                    Debug.DrawRay(transform.position, rayDirection * (alertDistance + 2f), Color.green);
                    playerSeachPosition = hitSearch.collider.transform.position;
                    return true;
                }
            }
        }
        return false;
    }

    // Renvoie vrai si l'ennemi apercoit le joueur dans la zone (rouge seulement)
    public bool AlertLineOfSight(Vector2 lineOfSightDirection) {
        float viewAngle = 30f;
        int raycount = 3;
        float startAngle = -viewAngle / 2f;
        float angleIncrement = viewAngle / (raycount - 1);

        for (int i = 0; i < raycount; i++) {
            float angle = startAngle + angleIncrement * i;
            Vector2 rayDirection = RotateVector(lineOfSightDirection, angle);
            RaycastHit2D hitAlert = Physics2D.Raycast(transform.position, rayDirection, alertDistance, LayerMask.GetMask("Mur", "Player"));

            Debug.DrawRay(transform.position, rayDirection * (alertDistance + 2f), Color.blue);
            Debug.DrawRay(transform.position, rayDirection * alertDistance, Color.red);

            if (hitAlert.collider != null) {
                if (hitAlert.collider.CompareTag("Player") || hitAlert.collider.transform.parent.CompareTag("Player")) {
                    Debug.DrawRay(transform.position, rayDirection * alertDistance, Color.green);
                    playerSeachPosition = hitAlert.collider.transform.position;
                    return true;
                }
            }
        }
        return false;
    }

    public Vector2 RotateVector(Vector2 v, float angleDegrees) {
        float rad = angleDegrees * Mathf.Deg2Rad;
        float cos  = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }

    #endregion

    #region Collect

    public void Collect(Collectible collectible) {
        if(collectible.collectibleType == CollectibleType.Coin) {
            this.Coins = this.Coins + collectible.amount;
        }
    }

    #endregion

    #region Annimation Triggers

    protected virtual void AnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType) {
        //StateMachine.CurrentEnnemyState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType {
        Idle,
        EnnemyDamaged,
        PlayFootStepSound
    }

    #endregion

}