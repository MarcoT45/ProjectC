using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterController : MonoBehaviour
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
    private PlayerMovement controls;

    public float moveSpeed;
    public Transform movePoint;

    public LayerMask stopMovement;


    private void Awake()
    {
        controls = new PlayerMovement();

        //Enregistrer les types d'inputs et leurs affecter les fonctions
        //started ~= GetKeyDown / performed ~= GetKey / canceled ~= GetKeyReleased
        controls.Main.Movement.started += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Main.Movement.canceled += ctx => this.direction = Vector2.zero;

        inventory = Inventory.Instance;
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
        character = this.gameObject;
        physics = character.GetComponent<Rigidbody2D>();
        movePoint.parent = null;
        uiInventory = GameObject.FindWithTag("UI_Inventory").GetComponent<UI_Inventory>();
        uiInventory.SetInventory(inventory);

    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (direction == Vector2.zero)
        {
            return;
        }


            if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
            {
                if (CanMove(direction))
                {
                    movePoint.position += (Vector3)direction;
                }
            }

        }

    private void Move(Vector2 direction)
    {
        this.direction = (Vector3)direction;
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
        if( other.gameObject.CompareTag("Coin") )
        {
            Destroy(other.gameObject);
            GameManager.Instance.AddCoins(1) ;
        }
    }
}
