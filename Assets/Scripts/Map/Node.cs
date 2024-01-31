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
    public List<GameObject> nodeLinkedNextFloor = new List<GameObject>();    // On liste les noeuds de l'étage supérieur auquel il est lié
    public SpriteRenderer spriteRenderer;                                    // Pour changer le sprite rapidement
    public List<Sprite> spriteList;                                          // Liste des icones des evenements (Tete de mort, Coffre, Boutique, etc...)
    
    public string titre;                                                     // Titre du type de node
    public string description;                                               // Description du type de node
    
    public int taille = 0;                                                   // Taille de la map du level X*X
    public int nbEnnemis = 0;                                                // Nombre d'ennemis dans le level
    public bool sortie = true;                                               // Si le niveau à une sortie

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
            mapGenerator.OpenPopUp(this);
        }
        Debug.Log("Mouse Click Detected on: "+this.name);
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