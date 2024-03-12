using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour, IShopCustomer, IDamageable
{

    private UI_Inventory uiInventory;
    private Inventory inventory;

    [SerializeField]
    private Tilemap solTileMap;

    [SerializeField]
    private Tilemap murTileMap;

    [SerializeField]
    private Tilemap sortieTileMap;

    private GameObject character;
    private Rigidbody2D physics;
    private Vector2 direction;
    private Vector2 directionAttack;
    private bool isAttacking;
    private PlayerMovement controls;
    private GameManager gameManager;

    public float moveSpeed;
    public Transform movePoint;
    private Vector3Int lastPosition;
    private Vector2 lastDirection;
    private bool isHurt;
    private SpriteRenderer spriteRenderer;
    private Material material;
    [SerializeField] private float pushBackDistance = 1f;
    [SerializeField] private float pushBackSpeed;
    [SerializeField] private float tintFadeSpeed;
    [SerializeField] private Color tintColor;

    public LayerMask stopMovement;

    public CharacterData data;
    public CharacterStats stats;

    public float maxHealth { get; set; }
    public float currentHealth { get; set; }

    //Si on utilise les barres de vie au dessus du perso
    /*public UnityEvent<float> OnChangeHealth;
    public UnityEvent OnHit;*/

    private void Awake()
    {
        stats = data.baseStats;
        maxHealth = stats.pv;
        currentHealth = maxHealth;

        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        controls = new PlayerMovement();

        //Enregistrer les types d'inputs et leurs affecter les fonctions
        //started ~= GetKeyDown / performed ~= GetKey / canceled ~= GetKeyReleased
        controls.Main.Movement.started += ctx => Move(ctx.ReadValue<Vector2>());
        /*controls.Main.Movement.canceled += ctx => this.direction = Vector2.zero;*/

    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        inventory = Inventory.Instance;
        character = this.gameObject;
        physics = character.GetComponent<Rigidbody2D>();
        movePoint.parent = null;
        /*uiInventory = GameObject.FindWithTag("UI_Inventory").GetComponent<UI_Inventory>();
        uiInventory.SetInventory(inventory);*/
        lastPosition = solTileMap.WorldToCell(transform.position);
        lastDirection = Vector2.zero;
        isHurt = false;

    }

    void Update()
    {
        if (isHurt)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, pushBackSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        }

        if(this.direction + lastDirection == Vector2.zero && lastDirection != Vector2.zero)
        {
            movePoint.position = solTileMap.GetCellCenterWorld(lastPosition); 

            lastDirection =  this.direction;
        }

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            lastPosition = solTileMap.WorldToCell(transform.position);

            if(isHurt)
            {
                isHurt = false;
                controls.Enable();
            }

            if (CanMove(this.direction))
            {
                movePoint.position += (Vector3)this.direction;
                lastDirection = this.direction;
            }
            else if(CanMove(lastDirection))
            {
                movePoint.position += (Vector3)lastDirection;
            }
        }
    }

    private void Move(Vector2 newDirection)
    {
        if (this.direction != newDirection && CanMove(newDirection))
        {
            lastDirection = this.direction;
        }
        this.direction = (Vector3)newDirection;

        Debug.Log("lastDir " + lastDirection);
        Debug.Log("dir " + this.direction);

    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = solTileMap.WorldToCell(transform.position + (Vector3)direction);
        if (!solTileMap.HasTile(gridPosition) || murTileMap.HasTile(gridPosition))
        {
            return false;
        }

        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 otherCell = Vector3.zero;
        Vector3Int thisCell = Vector3Int.zero;
        Vector3 directionPushback = Vector3.zero;

        if (other.gameObject.CompareTag("Ennemi"))
        {
            Ennemy ennemy = other.gameObject.GetComponent<Ennemy>();

            if(ennemy != null)
            {
                ennemy.Damage(stats.atk);
                StartCoroutine(HitFlash());

                otherCell = solTileMap.WorldToCell(other.transform.position);
                thisCell = solTileMap.WorldToCell(transform.position);
                directionPushback = (otherCell - thisCell).normalized * -1;

                if (directionPushback != Vector3.up && directionPushback != Vector3.down && directionPushback != Vector3.left && directionPushback != Vector3.right)
                {
                    otherCell = solTileMap.WorldToCell(movePoint.transform.position);
                    thisCell = solTileMap.WorldToCell(transform.position);
                    directionPushback = (otherCell - thisCell).normalized * -1;
                    transform.position = solTileMap.GetCellCenterWorld(lastPosition);
                }
                else
                {
                    transform.position = solTileMap.GetCellCenterWorld(thisCell);
                }

                for (int i = 0; i < pushBackDistance; i++)
                {
                    if (CanMove(directionPushback))
                    {
                        transform.position += directionPushback;
                    }
                }

                isHurt = true;
                this.direction = Vector2.zero;
                movePoint.position = transform.position;
                controls.Disable();
            }
        }
        if ( other.gameObject.CompareTag("Coin") )
        {
            Destroy(other.gameObject);
            gameManager.AddCoins(1) ;
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

    public void BuyItem(ItemData itemData)
    {
        gameManager.SetPlayerCoins(gameManager.GetPlayerCoins() - itemData.GetPrice());
        this.inventory.AddItem(itemData);
        uiInventory.RefreshInventoryItems();
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
