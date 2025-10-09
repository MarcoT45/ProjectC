using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap solTilemap;            //  Tilemap de la scène
    public Tilemap murTilemap;
    public float cellSize = 1f;        // Dans la plupart des cas, 1 unité = 1 case

    [HideInInspector]
    public Vector2Int gridSize;        // Taille de la grille (en cases)
    private Node[,] grid;


    void GenerateGrid()
    {
        // On récupère les bornes de la Tilemap
        solTilemap.CompressBounds();
        BoundsInt bounds = solTilemap.cellBounds;
        gridSize = new Vector2Int(bounds.size.x, bounds.size.y);
        grid = new Node[gridSize.x, gridSize.y];

        // Pour chaque cellule de la Tilemap, on crée un node.
        // Ici, on considère qu'une tile présente signifie "marchable".
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                // Convertir l’index de la grille en position de cell dans la Tilemap
                Vector3Int cellPos = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);

                // Par exemple, une tile existe => marcher (true); sinon, c’est un mur (false)
                bool walkable = false;
                if (solTilemap.GetTile(cellPos) != null && murTilemap.GetTile(cellPos) == null)
                {
                    walkable = true;
                }

                grid[x, y] = new Node(walkable, cellPos, new Vector2Int(x, y));
            }
        }
    }

    //Retourne les Nodes walkables
    public List<Node> GetAllWalkableNodes()
    {
        List<Node> nodes = new List<Node>();    
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (grid[x, y].walkable)
                {
                    nodes.Add(grid[x, y]);
                }
            }
        }

        return nodes;
    }

    public Vector3 FindRandomWalkableInRange(Vector3 enemyPosition, float range)
    {
        List<Node> walkableNodes = this.GetAllWalkableNodes();

        if(walkableNodes.Count == 0) { return Vector3.zero; }

        Node currentNode = this.GetNodeFromWorldPoint(enemyPosition);

        int attempts = 0;
        while(attempts < 10)
        {
            attempts++;
            Node candidate = walkableNodes[Random.Range(0, walkableNodes.Count)];

            float dist = Vector3.Distance(this.solTilemap.CellToWorld(candidate.cellPosition), enemyPosition);
            
            if(dist < range)
            {
                return this.solTilemap.CellToWorld(candidate.cellPosition);   
            }
        }

        return Vector3.zero;
    }

    // Convertit une World position en Node de la grille.
    public Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        Vector3Int cellPos = solTilemap.WorldToCell(worldPosition);
        BoundsInt bounds = solTilemap.cellBounds;
        int x = cellPos.x - bounds.xMin;
        int y = cellPos.y - bounds.yMin;

        if (x >= 0 && x < gridSize.x && y >= 0 && y < gridSize.y)
        {
            return grid[x, y];
        }
        return null;
    }

    public Vector3 CellToWorld(Vector3Int cellPosition)
    {
        return solTilemap.GetCellCenterWorld(cellPosition);
    }

    // Retourne les voisins d'un node (ici les 4 directions cardinales)
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        Vector2Int[] directions = new Vector2Int[] {
            new Vector2Int( 1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int( 0, 1),
            new Vector2Int( 0, -1)
        };

        foreach (Vector2Int d in directions)
        {
            int checkX = node.gridPosition.x + d.x;
            int checkY = node.gridPosition.y + d.y;
            if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
            {
                neighbors.Add(grid[checkX, checkY]);
            }
        }
        return neighbors;
    }

    //--------------------DEBUG--------------------------
    private void Start()
    {
        //Dans le start pour s'assurer que la tilemap est bien générée avant (MapBuilder)
        GenerateGrid();
    }

    //Debug : Affiche la grille dans la scène
    private void OnDrawGizmos()
    {
        if (grid != null)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Gizmos.color = (grid[x, y].walkable) ? Color.white : Color.red;
                    Vector3 worldPos = solTilemap.CellToWorld(grid[x, y].cellPosition) + new Vector3(cellSize, cellSize) * 0.5f;
                    Gizmos.DrawCube(worldPos, Vector3.one * (cellSize * 0.9f));
                }
            }
        }
    }
}