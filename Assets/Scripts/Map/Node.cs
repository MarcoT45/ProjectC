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
    public Vector2 position;                                                 // Coordonnées du noeud
    public List<int> nodeLinkedNextFloor = new List<int>();                  // On liste les numéros des noeuds de l'étage supérieur auquel il est lié
    public SpriteRenderer spriteRenderer;                                    // Pour changer le sprite rapidement
    public List<Sprite> spriteList;                                          // Liste des icones des evenements (Tete de mort, Coffre, Boutique, etc...)
    
    public int numero;                                                       // Numero du node
    public string titre;                                                     // Titre du type de node
    public string description;                                               // Description du type de node
    
    public int taille = 0;                                                   // Taille de la map du level X*X
    public int nbEnnemis = 0;                                                // Nombre d'ennemis dans le level
    public bool sortie = true;                                               // Si le niveau à une sortie

    public void SetAllDate(Node n) {
        this.position = n.position;
        this.nodeLinkedNextFloor = n.nodeLinkedNextFloor;
        this.numero = n.numero;
        this.titre = n.titre;
        this.description = n.description;
        this.taille = n.taille;
        this.nbEnnemis = n.nbEnnemis;
        this.sortie = n.sortie;

        UpdateNodeState(n.currentState);

        switch(n.titre) {
            case "Combat normal":
                this.spriteRenderer.sprite = spriteList[0];
                break;
            case "Combat d'élite":
                this.spriteRenderer.sprite = spriteList[1];
                break;
            case "Coffre":
                this.spriteRenderer.sprite = spriteList[2];
                break;
            case "Événement":
                this.spriteRenderer.sprite = spriteList[3];
                break;
            case "Repos":
                this.spriteRenderer.sprite = spriteList[4];
                break;
            case "Echange":
                this.spriteRenderer.sprite = spriteList[5];
                break;
            case "Magasin":
                this.spriteRenderer.sprite = spriteList[6];
                break;
            case "Boss":
                this.spriteRenderer.sprite = spriteList[7];
                this.transform.localScale = new Vector2 (3, 3);
                break;
        }
    }

    public void SetNumero(int n) {
        this.numero = n;
    }

    public void UpdateNodeState(NodeState newState) {
        this.currentState = newState;

        if(this.currentState == NodeState.Accessible) {
            this.spriteRenderer.color = new Color (1, 1, 1, 1);
        } else if(this.currentState == NodeState.Visite || this.currentState == NodeState.Position) {
            this.spriteRenderer.color = new Color (0.85f, 0.85f, 0.85f, 1);
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

    public void AddNodeLinkedNextFloor (int newNodeNumber) {
        if (!this.nodeLinkedNextFloor.Contains(newNodeNumber)) {
            this.nodeLinkedNextFloor.Add(newNodeNumber);
        }
    }

    public void ChangeNodeEvent (int spriteValue) {

        switch (spriteValue) {
            case 0:
                this.titre = "Combat normal";
                this.description = "Affrontez des ennemis (Fuite possible).";
                this.taille = Random.Range(2, 4);    // Un carré de 2x2 ou 3x3
                this.nbEnnemis = Random.Range(2, 4); // Entre 2 et 3 ennemis
                this.sortie = true;
                break;
            case 1:
                this.titre = "Combat d'élite";
                this.description = "Affrontez des ennemis coriaces (Fuite impossible).";
                this.taille = Random.Range(3, 5);    // Un carré de 3x3 ou 4x4
                this.nbEnnemis = Random.Range(4, 7); // Entre 4 et 6 ennemis
                this.sortie = false;
                break;
            case 2:
                this.titre = "Coffre";
                this.description = "Obtenez un objet en ouvrant ce coffre.";
                break;
            case 3:
                this.titre = "Événement";
                this.description = "Que va t'il se passer ? Une bonne ou une mauvaise rencontre ?";
                break;
            case 4:
                this.titre = "Repos";
                this.description = "Regagner 1PV en vous reposant au feu de camp.";
                break;
            case 5:
                this.titre = "Echange";
                this.description = "Echangez un objet en votre possession contre un autre.";
                break;
            case 6:
                this.titre = "Magasin";
                this.description = "Depensez votre argent dans cette boutique.";
                break;
            case 7:
                this.titre = "Boss";
                this.description = "Affrontez le chef de la zone.";
                this.transform.localScale = new Vector2 (3, 3);
                break;
        }

        this.spriteRenderer.sprite = spriteList[spriteValue];
    }

    // Lancer action
    private void OnMouseDown() {
        if(this.currentState == NodeState.Accessible) {
            GameObject map;
            map = GameObject.Find("Map Generator");
            MapGeneratorController mapGenerator = (MapGeneratorController) map.GetComponent(typeof(MapGeneratorController));
            mapGenerator.MovePlayer(this);
        }
    }

    // Si on survole afficher petite pop-up avec infos
    private void OnMouseOver() {
        GameObject map;
        map = GameObject.Find("Map Generator");
        MapGeneratorController mapGenerator = (MapGeneratorController) map.GetComponent(typeof(MapGeneratorController));
        mapGenerator.DisplayPopUp(true, this);
    }

    // Si on arrete de survoler cacher pop-up
    private void OnMouseExit() {
        GameObject map;
        map = GameObject.Find("Map Generator");
        MapGeneratorController mapGenerator = (MapGeneratorController) map.GetComponent(typeof(MapGeneratorController));
        mapGenerator.DisplayPopUp(false, this);
    }

}