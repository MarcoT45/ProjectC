using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NodeState {
    Accessible,
    Position,
    Visite,
    Bloque
};

public class Node : MonoBehaviour {

    public NodeState currentState;                                           // Etat du noeud pour le joueur
    public List<GameObject> nodeLinkedNextFloor = new List<GameObject>();    // On liste les noeuds de l'étage supérieur auquel il est lié
    public Vector2 position;                                                 // Coordonnées du noeud
    public SpriteRenderer spriteRenderer;                                    // Pour changer le sprite rapidement
    public List<Sprite> spriteList;                                          // Liste des icones des evenements (Tete de mort, Coffre, Boutique, etc...)

    public void UpdateNodeState(NodeState newState) {
        this.currentState = newState;

        if(this.currentState == NodeState.Accessible) {
            this.spriteRenderer.color = new Color (1, 1, 1, 1);
        } else if(this.currentState == NodeState.Visite) {
            this.spriteRenderer.color = new Color (0.75f, 0.75f, 0.75f, 1);
        } else if(this.currentState == NodeState.Bloque) {
            this.spriteRenderer.color = new Color (0.5f, 0.5f, 0.5f, 1);
        }
    }

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