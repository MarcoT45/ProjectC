using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class EnemyAI : MonoBehaviour, IDamageable, IEnnemyMoveable {

    [Header("Data")]
    public MonsterData monsterData;
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public int Coins { get; set; }
    [HideInInspector] public Vector2 forwardDirection;
    public float movementRange = 5f;
    [HideInInspector] public bool isSearching = false;
    [HideInInspector] public bool isAlerted = false;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public bool isInvincible = false;

    public float aggroDuration;
    public float attackCoolDown;
    public float fireRate;
    private SpriteRenderer spriteRenderer;

    [Header("Hit variables")]
    [HideInInspector] public bool isKnockedBack = false;
    public float knockbackDuration = 0.5f;
    public float knockbackForce = 3f;
    public AnimationCurve knockbackCurve;

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
    public EnemyState StunState { get; set; }
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

    //Events
    public event Action<int,int> OnHealthChanged;
    public event Action OnDeath;

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
            isMoving = true;
            Vector3 targetTmpPosition = gridManager.CellToWorld(path[currentPathIndex].cellPosition);

            Vector2 direction = (targetTmpPosition - transform.position).normalized;
            rb.MovePosition((Vector2) transform.position + (direction  * monsterData.speed * speedBoostAlertMultiplicator * Time.fixedDeltaTime));
            //rb.velocity = (targetTmpPosition - transform.position).normalized * monsterData.speed * speedBoostAlertMultiplicator;
            //transform.position = Vector2.MoveTowards(transform.position, targetTmpPosition, monsterData.speed * speedBoostAlertMultiplicator * Time.deltaTime);

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
                    isMoving = false; 
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

    public Vector2 GetSideVectorFromDirection(Vector2 direction)
    {

        //Dot Product où est le joueur par rapport à l'ennemi
        float upWeight = Vector2.Dot(direction.normalized, this.transform.up);
        float rightWeight = Vector2.Dot(direction.normalized, this.transform.right);

        float upMag = Mathf.Abs(upWeight);
        float rightMag = Mathf.Abs(rightWeight);

        if (upMag >= rightMag)
        {
            //Le joueur est au dessus ou en dessous de l'ennemi
            if (upWeight >= 0)
            {
                //Le joueur est au dessus
                return Vector2.up;
            }
            else
            {
                //Le joueur est en dessous
                return Vector2.down;
            }
        }
        else
        {
            //Le joueur est à gauche ou à droite de l'ennemi
            if (rightWeight >= 0)
            {
                //Le joueur est à droite
                return Vector2.right;
            }
            else
            {
                //Le joueur est à gauche
                return Vector2.left;
            }
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

        //Invoke l'action de changement de vie ( Ui barre de vie )  
        OnHealthChanged?.Invoke((int)CurrentHealth, (int)MaxHealth);

        StartCoroutine(HitFlash());

        if (CurrentHealth <= 0f) {

            //Invoke l'action de la mort ( UI barre de vie )
            OnDeath?.Invoke();
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
        rb.velocity = Vector2.zero;
        if (!isKnockedBack)
        {
            StartCoroutine(KnockbackCoroutine(direction));
        }
    }


    private IEnumerator KnockbackCoroutine(Vector2 direction)
    {
        isKnockedBack = true;

        Vector2 startPos = this.rb.position;
        Vector2 targetPos = (Vector2)transform.position + direction * knockbackForce;

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

        while(elapsed < knockbackDuration)
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