using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System;

public abstract class EnemyAI : MonoBehaviour, IDamageable, IEnnemyMoveable
{
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
    public float knockbackDuration = 0.3f;
    public float knockbackForce = 3f;
    private ParticleSystem hitParticles;
    protected Material material;
    [SerializeField] protected Color tintColor;
    [SerializeField] protected float tintFadeSpeed = 0.25f;


    [Header("AI Settings")]
    public float chaseDistance = 5f; // Distance à partir de laquelle on passe en mode Chase / = aggroRange
    public float safeRange; // Distance à laquelle l'ia se considere safe
    public float pathUpdateInterval = 0.5f; // Fréquence de mise à jour du pathfinding en Chase
    private float pathUpdateTimer = 0f;

    [Header("Pathfinding")]
    public GridManager gridManager;
    //public Vector3 targetPosition;
    private List<Node> path;
    private int currentPathIndex = 0;
    public float pathRefreshInterval = 0.5f;
    private float pathRefreshTimer = 1f;

    [Header("VFX")]
    public GameObject slashVFXPrefab;
    public GameObject stepVFXPrefab;
    public GameObject projectilePrefab;

    #region State Machine variables

    public EnemyStateMachine StateMachine { get; set; }

    //Mis dans les héritiers

    public EnemyState IdleState { get; set; }

    public EnemyState ChasingState { get; set; }
    public EnemyState AttackingState { get; set; }

    #endregion

    [HideInInspector]
    public Transform player;        // Référence au joueur (assignée dans Start)
    public LayerMask wallLayerMask;
    private Rigidbody2D rb;

    public Action OnIdleDestinationReached;


    #region Awake/Start/Update
    protected virtual void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        forwardDirection = Vector2.right;
        hitParticles = GetComponentInChildren<ParticleSystem>(); // Récupère le système de particules

        //Ennemy
        StateMachine = new EnemyStateMachine();
        spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        MaxHealth = monsterData.pv;
        CurrentHealth = MaxHealth;

        StateMachine.Initialize(IdleState);
    }

    protected virtual void Update()
    {
        //Juste pour voir où il regarde
        Debug.DrawRay(transform.position, forwardDirection, Color.blue);

        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    protected virtual void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.FrameFixedUpdate();
    }
    #endregion

    #region Move
    // Déplace l'ennemi le long du chemin calculé
    public void Move(Vector3 targetPosition)
    {
        if (isKnockedBack) return;

        pathRefreshTimer += Time.deltaTime;
        if (pathRefreshTimer >= pathRefreshInterval)
        {
            path = AStarPathfinding.FindPath(gridManager, transform.position, targetPosition);
            currentPathIndex = 0;
            pathRefreshTimer = 0f;

            //Debug.Log("Path count : " + path.Count);
        }

        if (path != null && path.Count > 0 && currentPathIndex < path.Count)
        {
            Vector3 targetTmpPosition = gridManager.CellToWorld(path[currentPathIndex].cellPosition);

            // Appliquer un déplacement vers targetPosition
            rb.velocity = (targetTmpPosition - transform.position).normalized * monsterData.speed;
            // rb.AddForce((targetTmpPosition - transform.position).normalized * speed, ForceMode2D.Force);

            Vector2 face = Vector2.zero;
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        face = Vector2.up;
                        break;

                    case 1:
                        face = Vector2.down;
                        break;

                    case 2:
                        face = Vector2.right;
                        break;

                    case 3:
                        face = Vector2.left;
                        break;
                }

                if (Vector2.Dot(face, (targetTmpPosition - transform.position).normalized) > 0)
                {
                    forwardDirection = face;
                }
            }

            // Si suffisamment proche du prochain point du chemin, on avance au suivant
            if (Vector3.Distance(transform.position, targetTmpPosition) < 0.1f)
            {
                currentPathIndex++;

                // Arrivé à destination ?
                if (currentPathIndex >= path.Count)
                {
                    path = null;
                    currentPathIndex = 0;

                    // Notifier le comportement idle
                    OnIdleDestinationReached?.Invoke();
                }
            }
        }
    }
    #endregion

    #region Collision functions

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
            //-------------- Collision avec un mur ( rétiré pour le moment ) 
            /*if (collision.gameObject.CompareTag("Mur"))
            {
                Debug.Log("Collision Mur");
                //Dégat à voir si en fonction du joueur ou du mur
                Damage(5);

                //Code pour le rebond
                //Vector2 direction = ((Vector2)this.transform.position - collision.GetContact(0).point).normalized;
                //ApplyKnockback(direction);
            }*/
            //---------------

            //---------------- Collision avec autre ennemi ( rétiré pour le moment ) 
            /*if (collision.gameObject.CompareTag("Ennemi"))
            {
                Debug.Log("Collision Ennemi");
                EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    Damage((int)enemy.monsterData.atk);
                    enemy.Damage((int)this.monsterData.atk);
                }
            }*/
            //---------------
        }
    }
    #endregion

    #region Health / Die functions
    public void Damage(int damage)
    {
        CurrentHealth -= damage;
        StartCoroutine(BlinkRoutine());

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    #endregion


    #region VFX functions 
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
    #endregion


    #region Aggro functions 
    // Renvoie vrai si aucune obstruction n'empêche la vue entre l'ennemi et le joueur
    public bool HasLineOfSight(Vector2 lineOfSightDirection)
    {
        float viewAngle = 30f;
        int raycount = 3;
        //Vector2 lineOfSightDirection = (player.position - transform.position).normalized;

        float startAngle = -viewAngle / 2f;
        float angleIncrement = viewAngle / (raycount - 1);

        for (int i = 0; i < raycount; i++)
        {
            float angle = startAngle + angleIncrement * i;
            Vector2 rayDirection = RotateVector(lineOfSightDirection, angle);

            //RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, chaseDistance, LayerMask.GetMask("Player"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, chaseDistance, LayerMask.GetMask("Mur", "Player"));


            // Si le raycast n'a rien touché ou touche directement le joueur, la ligne de vue est bonne
            //if (hit.collider == null || hit.collider.CompareTag("Player"))
            Debug.DrawRay(transform.position, rayDirection * chaseDistance, Color.red);

            if (hit.collider != null)
            { 
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.DrawRay(transform.position, rayDirection * chaseDistance, Color.green);
                    return true;
                }
            }


        }

        return false;
    }

    public Vector2 RotateVector(Vector2 v, float angleDegrees)
    {
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
    public void Collect(Collectible collectible)
    {
        if(collectible.collectibleType == CollectibleType.Coin)
        {
            this.Coins = this.Coins + collectible.amount;
        }
    }
    #endregion


    #region Annimation Triggers

    protected virtual void AnimationTriggerEvent(EnemyAI.AnimationTriggerType triggerType)
    {
        //StateMachine.CurrentEnnemyState.AnnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        Idle,
        EnnemyDamaged,
        PlayFootStepSound
    }

    #endregion
}
