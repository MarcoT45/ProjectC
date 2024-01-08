using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState {
    Accessible,
    Visite,
    Bloque
};

public class Node : MonoBehaviour {

    //public NodeState currentState;                                         // Etat du noeud pour le joueur
    public List<GameObject> nodeLinkedNextFloor = new List<GameObject>();    // On liste les noeuds de l'étage supérieur auquel il est lié
    public Vector2 position;                                                 // Coordonnées du noeud
    public SpriteRenderer spriteRenderer;                                    // SpriteRenderer du GameObject Node
    public List<Sprite> spriteList;                                          // Liste des icones des evenements (Tete de mort, Coffre, Boutique, etc...)

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

    public void ChangeNodeEvent(int spriteValue) {
        spriteRenderer.sprite = spriteList[spriteValue];

        if (spriteValue == 7) { // Si c'est le boss on agrandit l'icone
            this.transform.localScale = new Vector2 (3, 3);
        }
    }

}