using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public abstract class Ennemy : MonoBehaviour, IDamageable, IEnnemyMoveable
{
    public MonsterData monsterData;

    public Transform movePoint;

    public float MaxHealth { get; set; }

    public float CurrentHealth { get; set; }

    public bool IsFacingRight { get; set; }

    public Tilemap murTileMap;
    public Tilemap solTileMap;

    #region State Machine variables

    public EnnemyStateMachine StateMachine { get; set; }

    //Mis dans les héritiers

    public EnnemyState IdleState { get; set; }

    public EnnemyState ChasingState { get; set; }


    public bool isBlocked;

    #endregion

    #region Movement variables

    public float movementRange = 5f;
    public bool isAggroed = false;
    public Vector3Int lastPosition;

    #endregion

    #region Hit variables

    public bool isHurt;
    protected SpriteRenderer spriteRenderer;
    protected Material material;
    [SerializeField] protected float pushBackDistance = 1f;
    [SerializeField] protected float pushBackSpeed = 3f;
    [SerializeField] protected float tintFadeSpeed = 0.25f;
    [SerializeField] protected Color tintColor;

    #endregion

    //Si on utilise les barres de vie au dessus du perso
   /* public UnityEvent<float> OnChangeHealth;
    public UnityEvent OnHit;*/


    protected virtual void Awake()
    {
        StateMachine = new EnnemyStateMachine();

        /* Mis dans les héritiers
        IdleState = new EnnemyIdleState(this, StateMachine);
        ChasingState = new EnnemyChasingState(this, StateMachine);*/

        movePoint.parent = null;

        spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        material = spriteRenderer.material;
        isHurt = false;

    }

    protected virtual void Start()
    {
        MaxHealth = monsterData.pv;
        CurrentHealth = MaxHealth;

        StateMachine.Initialize(IdleState);
    }

    protected virtual void Update()
    {
        StateMachine.CurrentEnnemyState.FrameUpdate();
    }


    #region Health / Die functions
    public void Damage(float damage)
    {
        CurrentHealth -= damage;

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
        if (IsFacingRight && direction.x < 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

        }
        else if (!IsFacingRight && direction.x > 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
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

    public void OnTriggerEnter2D(Collider2D other)
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

    protected virtual void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnnemyState.AnnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        Idle,
        EnnemyDamaged,
        PlayFootStepSound
    }

    #endregion
}
