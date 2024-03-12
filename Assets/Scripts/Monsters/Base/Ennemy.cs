using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class Ennemy : MonoBehaviour, IDamageable, IEnnemyMoveable
{
    public MonsterData monsterData;

    public Transform movePoint;

    public float maxHealth { get; set; }

    public float currentHealth { get; set; }

    public bool isFacingRight { get; set; }

    public Tilemap murTileMap;
    public Tilemap solTileMap;

    #region State Machine variables

    public EnnemyStateMachine stateMachine { get; set; }

    public EnnemyIdleState idleState { get; set; }

    public EnnemyChasingState chasingState { get; set; }

    private bool isBlocked;

    #endregion

    #region Movement variables

    public float movementRange = 5f;
    public bool isAggroed = false;
    public Vector3Int lastPosition;

    #endregion

    #region Hit variables

    private bool isHurt;
    private SpriteRenderer spriteRenderer;
    private Material material;
    [SerializeField] private float pushBackDistance = 1f;
    [SerializeField] private float pushBackSpeed = 3f;
    [SerializeField] private float tintFadeSpeed = 0.25f;
    [SerializeField] private Color tintColor;

    #endregion

    //Si on utilise les barres de vie au dessus du perso
   /* public UnityEvent<float> OnChangeHealth;
    public UnityEvent OnHit;*/


    private void Awake()
    {
        stateMachine = new EnnemyStateMachine();

        idleState = new EnnemyIdleState(this, stateMachine);
        chasingState = new EnnemyChasingState(this, stateMachine);

        movePoint.parent = null;

        spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        material = spriteRenderer.material;
        isHurt = false;

    }

    private void Start()
    {
        maxHealth = monsterData.pv;
        currentHealth = maxHealth;

        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currentEnnemyState.FrameUpdate();
    }


    #region Health / Die functions
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
    #endregion

    #region Move Ennemy functions

    public void MoveEnnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, monsterData.speed * Time.deltaTime);
    }

    public void MoveEnnemyMovePoint(Vector2 direction)
    {
        movePoint.position += (Vector3)direction;
    }

    public bool CanEnnemyMove(Vector2 direction)
    {
        Vector3Int gridPosition = solTileMap.WorldToCell(transform.position + (Vector3)direction);

        if (!solTileMap.HasTile(gridPosition) || murTileMap.HasTile(gridPosition))
        {
            return false;
        }

        return true;
    }

    public Vector2 FindNextCell(Vector2 direction, Vector3 targetPos)
    {
        float min = Mathf.Infinity;
        Vector2 directionTemp = Vector2.zero;
        Vector2 directionInverse = Vector2.zero;
        Vector2 directionMin = Vector2.zero;

        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    directionTemp = Vector2.up;
                    directionInverse = Vector2.down;
                    break;

                case 1:
                    directionTemp = Vector2.left;
                    directionInverse = Vector2.right;
                    break;

                case 2:
                    directionTemp = Vector2.down;
                    directionInverse = Vector2.up;
                    break;

                case 3:
                    directionTemp = Vector2.right;
                    directionInverse = Vector2.left;
                    break;
            }

            if (direction != directionInverse)
            {
                if (CanEnnemyMove(directionTemp))
                {

                    if (Vector2.Distance(transform.position + (Vector3)directionTemp, targetPos) < min)
                    {
                        min = Vector2.Distance(transform.position + (Vector3)directionTemp, targetPos);
                        directionMin = directionTemp;
                    }
                }
            }
        }
        return direction = directionMin;

    }

    public void CheckForLeftOrRightFacing(Vector2 direction)
    {
        if (isFacingRight && direction.x < 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;

        }
        else if (!isFacingRight && direction.x > 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
    }

    #endregion

    #region Aggro / Chase functions
    
    public bool CheckAggro(Vector2 direction, float aggroRange)
    {
        Vector2 position = transform.position;


        RaycastHit2D hit = Physics2D.Raycast(
                origin: position,
                direction: direction,
                distance: aggroRange);

        //Ici hit ne detecte pas le collider d'où il sort car j'ai décoché l'option dans les settings
        // Project Settings > Physics 2D > Query Start in collider
        if ( hit.collider != null)
        {
            Debug.DrawRay(
                    start: transform.position,
                    dir: direction * aggroRange,
                    color: Color.green);

            if (hit.collider.gameObject.tag == "Player")
            {
                return true;
            }

        }
        else
        {

            Debug.DrawRay(
                    start: transform.position,
                    dir: direction * aggroRange,
                    color: Color.red);
        }

        return false;
    }

    #endregion

    #region Collider / Hit flash

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 otherCell = Vector3.zero;
        Vector3Int thisCell = Vector3Int.zero;
        Vector3 directionPushback = Vector3.zero;

        if (other.gameObject.CompareTag("Player"))
        {

            //A changer peut etre si le CharacterController est amené à être modifié dans sa structure
            CharacterController characterController = other.gameObject.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characterController.Damage(monsterData.atk);
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
                    if (CanEnnemyMove(directionPushback))
                    {
                        transform.position += directionPushback;
                    }
                }

                isHurt = true;
                movePoint.position = transform.position;
            }

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
    #endregion

    #region Annimation Triggers

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        stateMachine.currentEnnemyState.AnnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        Idle,
        EnnemyDamaged,
        PlayFootStepSound
    }

    #endregion
}
