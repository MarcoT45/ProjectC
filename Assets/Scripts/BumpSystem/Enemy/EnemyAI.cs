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
    public bool isAggroed = false;
    public float aggroDuration;
    public float attackCoolDown;
    public float fireRate;
    private SpriteRenderer spriteRenderer;

    [Header("Hit variables")]
    private bool isKnockedBack = false;
    public float knockbackDuration = 0.5f;
    public float knockbackForce = 3f;
    private ParticleSystem hitParticles;
    protected Material material;
    [SerializeField] protected Color tintColor;
    [SerializeField] protected float tintFadeSpeed = 0.25f;

    [Header("AI Settings")]
    public float chaseDistance = 5f; // Distance à partir de laquelle on passe en mode Chase / = aggroRange
    public float safeRange; // Distance à laquelle l'ia se considere safe
    public float pathUpdateInterval = 0.5f; // Fréquence de mise à jour du pathfinding en Chase

    [Header("Pathfinding")]
    public GridManager gridManager;
    public Vector3 idleTargetPosition;
    private List<Node> path;
    private int currentPathIndex = 0;
    public float pathRefreshInterval = 0.5f;
    private float pathRefreshTimer = 1f;

    [Header("VFX")]
    public Animator animator;
    public GameObject slashVFXPrefab;
    public GameObject stepVFXPrefab;
    public GameObject projectilePrefab;

    #region State Machine variables

    public EnemyStateMachine StateMachine { get; set; }

    //Mis dans les héritiers

    public EnemyState IdleState { get; set; }
    public EnemyState ChasingState { get; set; }
    public EnemyState LookingState { get; set; }
    public EnemyState AttackingState { get; set; }

    #endregion

    [HideInInspector]
    public Transform player;                     // Référence au joueur (assignée dans Start)
    public Transform loot;                       // Référence au collectible visible
    public LayerMask wallLayerMask;
    private Rigidbody2D rb;

    public Action OnIdleDestinationReached;

    #region Awake/Start/Update

    protected virtual void Awake() {
        gridManager = GameObject.FindWithTag("GridManager").GetComponent<GridManager>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        forwardDirection = Vector2.right;
        hitParticles = GetComponentInChildren<ParticleSystem>(); // Récupère le système de particules

        //Ennemy
        StateMachine = new EnemyStateMachine();
        spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    protected virtual void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        MaxHealth = monsterData.pv;
        CurrentHealth = MaxHealth;

        StateMachine.Initialize(IdleState);
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

    public void SetIdleTargetPosition(Vector3 newTarget) {
        this.idleTargetPosition = newTarget;
    }

    // Déplace l'ennemi le long du chemin calculé
    public void Move(Vector3 idleTargetPosition) {

        if (isKnockedBack) return;

        pathRefreshTimer += Time.deltaTime;
        if (pathRefreshTimer >= pathRefreshInterval) {
            path = AStarPathfinding.FindPath(gridManager, transform.position, idleTargetPosition);
            currentPathIndex = 0;
            pathRefreshTimer = 0f;
        }

        if (path != null && path.Count > 0 && currentPathIndex < path.Count) {
            Vector3 targetTmpPosition = gridManager.CellToWorld(path[currentPathIndex].cellPosition);

            // Appliquer un déplacement vers idleTargetPosition, (x1.5 si ennemy en mode aggro)
            if (isAggroed) {
                rb.velocity = (targetTmpPosition - transform.position).normalized * monsterData.speed * 1.5f;
            } else {
                rb.velocity = (targetTmpPosition - transform.position).normalized * monsterData.speed;
            }

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

                    // L'ennemi peut soit continuer à se balader soit regarder autour de lui (80/20)
                    int lookingIdlingRandom = UnityEngine.Random.Range(1, 101);
                    if(lookingIdlingRandom > 80) {
                        rb.velocity = Vector2.zero;
                        this.StateMachine.ChangeState(LookingState);
                    } else {
                        // Notifier le comportement idle
                        OnIdleDestinationReached?.Invoke();
                    }
                }
            }
        } else if (path != null && currentPathIndex == path.Count) {
            // DANS LE CAS OU L'ENNEMI SE COINCE DANS UN MUR SANS QU'ON AIT PU ATTEINDRE LA DISTANCE REQUISE
            path = null;
            currentPathIndex = 0;

            // L'ennemi peut soit continuer à se balader soit regarder autour de lui (80/20)
            int lookingIdlingRandom = UnityEngine.Random.Range(1, 101);
            if(lookingIdlingRandom > 80) {
                rb.velocity = Vector2.zero;
                this.StateMachine.ChangeState(LookingState);
            } else {
                // Notifier le comportement idle
                OnIdleDestinationReached?.Invoke();
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
        StartCoroutine(BlinkRoutine());

        if (CurrentHealth <= 0f) {
            Die();
        }
    }

    public void Die() {
        Destroy(gameObject);
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
        this.StateMachine.ChangeState(ChasingState);

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

    private void PlayHitEffect(Vector2 hitPosition) {
        if (hitParticles != null) {
            hitParticles.transform.position = hitPosition; // Positionne les particules au point de contact
            hitParticles.Play(); // Lance l'effet
        }
    }

    #endregion

    #region Aggro functions

    // Renvoie vrai si aucune obstruction n'empêche la vue entre l'ennemi et le joueur
    public bool HasLineOfSight(Vector2 lineOfSightDirection) {
        float viewAngle = 30f;
        int raycount = 3;
        //Vector2 lineOfSightDirection = (player.position - transform.position).normalized;

        float startAngle = -viewAngle / 2f;
        float angleIncrement = viewAngle / (raycount - 1);

        for (int i = 0; i < raycount; i++) {
            float angle = startAngle + angleIncrement * i;
            Vector2 rayDirection = RotateVector(lineOfSightDirection, angle);

            //RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, chaseDistance, LayerMask.GetMask("Player"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, chaseDistance, LayerMask.GetMask("Mur", "Player"));


            // Si le raycast n'a rien touché ou touche directement le joueur, la ligne de vue est bonne
            //if (hit.collider == null || hit.collider.CompareTag("Player"))
            Debug.DrawRay(transform.position, rayDirection * chaseDistance, Color.red);

            if (hit.collider != null) {
                if (hit.collider.CompareTag("Player") || hit.collider.transform.parent.CompareTag("Player")) {
                    Debug.DrawRay(transform.position, rayDirection * chaseDistance, Color.green);
                    return true;
                }
            }
        }
        return false;
    }

    // Renvoie vrai si aucune obstruction n'empêche la vue entre l'ennemi et la Piece/Collectible
    public bool HasLootInLineOfSight(Vector2 lineOfSightDirection) {
        float viewAngle = 30f;
        int raycount = 3;
        float startAngle = -viewAngle / 2f;
        float angleIncrement = viewAngle / (raycount - 1);

        for (int i = 0; i < raycount; i++) {
            float angle = startAngle + angleIncrement * i;
            Vector2 rayDirection = RotateVector(lineOfSightDirection, angle);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, chaseDistance, LayerMask.GetMask("Mur"));

            // Si le raycast n'a rien touché ou touche directement le collectible, la ligne de vue est bonne
            Debug.DrawRay(transform.position, rayDirection * chaseDistance, Color.red);

            if (hit.collider != null) {
                if (hit.collider.CompareTag("Collectible")) {
                    Debug.Log("Je vois un collectible");
                    loot = hit.collider.transform;
                    Debug.DrawRay(transform.position, rayDirection * chaseDistance, Color.yellow);
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