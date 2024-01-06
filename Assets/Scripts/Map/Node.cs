using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState {
    Accessible,
    Visite,
    Bloque
};

public class Node : MonoBehaviour {

    //public NodeState currentState;                                          // Etat du noeud pour le joueur
    public List<GameObject> nodeLinkedNextFloor = new List<GameObject>();    // On liste les noeuds de l'étage supérieur auquel il est lié
    public Vector2 position;                                                  // Coordonnée du noeud
    //public Sprite eventSprite;                                              // Sprite de l'icone (Tete de mort, Coffre, Boutique, etc...)

    public Vector2 GetPosition () {
        return this.position;
    }

    public void SetPosition (Vector2 newPosition) {
        this.position = newPosition;
    }

    public void AddNodeLinkedNextFloor (GameObject newNode) {
        if (!this.nodeLinkedNextFloor.Contains(newNode)) {
            this.nodeLinkedNextFloor.Add(newNode);
        }
    }

}