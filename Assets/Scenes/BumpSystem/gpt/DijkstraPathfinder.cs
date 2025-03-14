using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class DijkstraPathfinder
{
    public static bool[,] Mapping(Tilemap sol, Tilemap mur)
    {
        Vector3Int mapSize = sol.size;
        bool[,] map = new bool[mapSize.x , mapSize.y];

        for (int j = 0; j < mapSize.y; j++)
        {

            for (int i = 0; i < mapSize.x; i++)
            {
                Vector3Int gridPosition = new Vector3Int(i, j, 0);

                if (!sol.HasTile(gridPosition) || mur.HasTile(gridPosition))
                {
                    map[i, j] = false;
                }
                else
                {
                    map[i, j] = true;
                }
            }
        }

        return map;
    }

    // Calcule le chemin (liste de positions mondiales) entre start et goal sur une grille
    public static List<Vector3> ComputePath(Vector3 start, Vector3 goal, bool[,] grid, Vector2 gridOrigin, float cellSize)
    {
        Vector2Int startCell = WorldToCell(start, gridOrigin, cellSize);
        Vector2Int goalCell = WorldToCell(goal, gridOrigin, cellSize);
        List<Vector2Int> pathCells = ComputePath(startCell, goalCell, grid);
        if (pathCells == null)
            return null;
        List<Vector3> path = new List<Vector3>();
        foreach (Vector2Int cell in pathCells)
        {
            path.Add(CellToWorld(cell, gridOrigin, cellSize));
        }
        return path;
    }

    // Implémente l'algorithme de Dijkstra sur la grille (utilise 4 directions : haut, bas, gauche, droite)
    public static List<Vector2Int> ComputePath(Vector2Int start, Vector2Int goal, bool[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        Dictionary<Vector2Int, float> dist = new Dictionary<Vector2Int, float>();
        Dictionary<Vector2Int, Vector2Int> prev = new Dictionary<Vector2Int, Vector2Int>();
        List<Vector2Int> unvisited = new List<Vector2Int>();

        // Initialisation
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                dist[pos] = float.MaxValue;
                unvisited.Add(pos);
            }
        }
        dist[start] = 0;

        while (unvisited.Count > 0)
        {
            // Sélectionner le nœud non visité ayant la plus petite distance
            Vector2Int current = unvisited[0];
            foreach (var pos in unvisited)
            {
                if (dist[pos] < dist[current])
                    current = pos;
            }

            if (current == goal)
                break;

            unvisited.Remove(current);

            foreach (Vector2Int neighbor in GetNeighbors(current, width, height))
            {
                if (!grid[neighbor.x, neighbor.y])
                    continue; // Obstacle
                if (!unvisited.Contains(neighbor))
                    continue;
                float alt = dist[current] + 1f; // Coût uniforme entre cases
                if (alt < dist[neighbor])
                {
                    dist[neighbor] = alt;
                    prev[neighbor] = current;
                }
            }
        }

        if (!prev.ContainsKey(goal))
            return null; // Aucun chemin trouvé

        // Reconstitution du chemin
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int node = goal;
        while (node != start)
        {
            path.Add(node);
            node = prev[node];
        }
        path.Add(start);
        path.Reverse();
        return path;
    }

    static List<Vector2Int> GetNeighbors(Vector2Int cell, int width, int height)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        Vector2Int[] directions = new Vector2Int[] {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1)
        };
        foreach (var d in directions)
        {
            Vector2Int neighbor = cell + d;
            if (neighbor.x >= 0 && neighbor.x < width && neighbor.y >= 0 && neighbor.y < height)
                neighbors.Add(neighbor);
        }
        return neighbors;
    }

    static Vector2Int WorldToCell(Vector3 worldPos, Vector2 gridOrigin, float cellSize)
    {
        int x = Mathf.FloorToInt((worldPos.x - gridOrigin.x) / cellSize);
        int y = Mathf.FloorToInt((worldPos.y - gridOrigin.y) / cellSize);
        return new Vector2Int(x, y);
    }

    static Vector3 CellToWorld(Vector2Int cell, Vector2 gridOrigin, float cellSize)
    {
        float x = gridOrigin.x + cell.x * cellSize + cellSize / 2f;
        float y = gridOrigin.y + cell.y * cellSize + cellSize / 2f;
        return new Vector3(x, y, 0);
    }
}
