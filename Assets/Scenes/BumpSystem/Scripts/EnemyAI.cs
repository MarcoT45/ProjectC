using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public enum EnemyState { Patrol, Chase }

public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public EnemyState currentState = EnemyState.Patrol;
    public float chaseDistance = 8f;              // Distance à partir de laquelle on passe en mode Chase
    public float pathUpdateInterval = 0.5f;         // Fréquence de mise à jour du pathfinding en Chase
    private float pathUpdateTimer = 0f;

    [Header("Pathfinding")]
    private GridManager2 gridManager;
    private List<Node> path;
    private int currentPathIndex = 0;
    public float pathRefreshInterval = 0.5f;
    private float pathRefreshTimer = 0f;

    [Header("Patrol")]
    public Vector3 patrolTarget;                  // Cible de patrouille aléatoire

    [Header("Grid Settings (pour Dijkstra)")]
    public Tilemap solTilemap;
    public Tilemap murTilemap;
    public bool[,] grid;              // Tableau 2D indiquant les cases marchables (true) et obstacles (false)
    public Vector2 gridOrigin = Vector2.zero; // Origine (coin inférieur gauche) de la grille en coordonnées mondiales
    public float cellSize = 1f;       // Taille d'une case

    [HideInInspector]
    public Transform player;        // Référence au joueur (assignée dans Start)



    void Start()
    {
        gridManager = FindObjectOfType<GridManager2>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        SetRandomPatrolTarget();

    }

    void Update()
    {

        pathRefreshTimer += Time.deltaTime;
        if (pathRefreshTimer >= pathRefreshInterval)
        {
            path = AStarPathfinding.FindPath(gridManager, transform.position, player.position);
            currentPathIndex = 0;
            pathRefreshTimer = 0f;
        }

        // Vous pouvez ensuite déplacer l’ennemi le long du chemin calculé...
        if (path != null && path.Count > 0 && currentPathIndex < path.Count)
        {
            Vector3 targetPosition = gridManager.solTilemap.CellToWorld(path[currentPathIndex].cellPosition);

            // Appliquer un déplacement vers targetPosition
        }

        /*
        // Vérifier la distance et la ligne de vue pour passer en mode Chase
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= chaseDistance && HasLineOfSight())
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }

        if (currentState == EnemyState.Chase)
        {
        }
        else // Patrol
        {
            if (Vector3.Distance(transform.position, patrolTarget) < 0.5f)
            {
                SetRandomPatrolTarget();
            }
        }*/


    }

    // Renvoie vrai si aucune obstruction n'empêche la vue entre l'ennemi et le joueur
    bool HasLineOfSight()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, chaseDistance, LayerMask.GetMask("Environment"));
        // Si le raycast n'a rien touché ou touche directement le joueur, la ligne de vue est bonne
        if (hit.collider == null || hit.collider.CompareTag("Player"))
            return true;
        return false;
    }

    void SetRandomPatrolTarget()
    {
        // Par exemple, dans des bornes prédéfinies (ajustez selon votre niveau)
        float randomX = Random.Range(0, solTilemap.size.x);
        float randomY = Random.Range(0, solTilemap.size.y);
        patrolTarget = new Vector3(randomX, randomY, 0);
        Debug.Log("Patrol target " + patrolTarget);
    }
}