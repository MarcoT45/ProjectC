using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Node
{
    public bool walkable;
    public Vector3Int cellPosition;   // Position de la tile dans le Tilemap
    public Vector2Int gridPosition;   // Position dans la grille générée
    public int gCost;    // Coût du départ jusqu'à ce nœud
    public int hCost;    // Coût heuristique (estimation) du nœud jusqu'au but
    public int fCost { get { return gCost + hCost; } }
    public Node parent;

    public Node(bool _walkable, Vector3Int _cellPosition, Vector2Int _gridPosition)
    {
        walkable = _walkable;
        cellPosition = _cellPosition;
        gridPosition = _gridPosition;
        gCost = 0;
        hCost = 0;
        parent = null;
    }
}