using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStarPathfinding
{
    //GridManager2 a changer de nom 
    public static List<Node> FindPath(GridManager gridManager, Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = gridManager.GetNodeFromWorldPoint(startPos);
        //Node targetNode = gridManager.GetNodeFromWorldPoint(targetPos);
        Node targetNode = gridManager.GetNodeFromWorldPoint(targetPos);

        if (startNode == null || targetNode == null) return null;

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        // Initialiser les coûts
        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, targetNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
                return RetracePath(startNode, targetNode);

            foreach (Node neighbor in gridManager.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        return null; // Aucun chemin trouvé
    }

    // Reconstitution du chemin en remontant depuis le node cible au node de départ
    static List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            //  /!\ Debug Affiche le chemin
            Debug.DrawLine(currentNode.cellPosition, (currentNode.parent).cellPosition, Color.green, 1f);

            path.Add(currentNode);
            currentNode = currentNode.parent;

        }
        path.Reverse();
        return path;
    }

    // Utilise la distance de Manhattan puisque nous autorisons uniquement les déplacements en 4 directions
    static int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int dstY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);
        return dstX + dstY;
    }
}