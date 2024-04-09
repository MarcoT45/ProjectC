using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoeudController : MonoBehaviour {

    public Noeud noeud = new Noeud();
    public List<Sprite> spriteList;

    public void UpdateEtatNoeud(EtatNoeud nouvelleEtat) {
        this.noeud.SetEtat(nouvelleEtat);

        if(this.noeud.etat == EtatNoeud.Accessible || this.noeud.etat == EtatNoeud.Visite || this.noeud.etat == EtatNoeud.Joueur) {
            this.GetComponent<SpriteRenderer>().color = new Color (0.3333333f, 0.2196078f, 0.2509804f, 1);
        } else {
            this.GetComponent<SpriteRenderer>().color = new Color (0.6078432f, 0.4078431f, 0.3490196f, 1);
        }
    }

    public void UpdateCouleur() {
        if(this.noeud.etat == EtatNoeud.Accessible || this.noeud.etat == EtatNoeud.Visite || this.noeud.etat == EtatNoeud.Joueur) {
            this.GetComponent<SpriteRenderer>().color = new Color (0.3333333f, 0.2196078f, 0.2509804f, 1);
        } else {
            this.GetComponent<SpriteRenderer>().color = new Color (0.6078432f, 0.4078431f, 0.3490196f, 1);
        }
    }

    public void ChangeNodeState(int spriteValue) {
        this.noeud.eventNumber = spriteValue;
        this.GetComponent<SpriteRenderer>().sprite = spriteList[spriteValue];
    }

    // Si on clique sur le noeud
    private void OnMouseDown() {
        if(this.noeud.etat == EtatNoeud.Accessible) {
            GameObject carte = GameObject.Find("Carte Foret");
            CarteForetController carteController = (CarteForetController) carte.GetComponent(typeof(CarteForetController));
            carteController.MovePlayer(this.noeud);
        }
    }

}

public enum EtatNoeud {
    Accessible,
    Inatteignable,
    Visite,
    Joueur
};

[System.Serializable]
public class Noeud {

    public int numero;
    public List<int> noeudsPrecedent = new List<int>();
    public List<int> noeudsSuivant = new List<int>();
    public EtatNoeud etat = EtatNoeud.Inatteignable;
    public int eventNumber = 0;

    public Noeud() {
        this.noeudsPrecedent = new List<int>();
        this.noeudsSuivant = new List<int>();
        this.etat = EtatNoeud.Inatteignable;
        this.eventNumber = 0;
    }

    public Noeud(Noeud n) {
        this.numero = n.numero;
        this.noeudsPrecedent = n.noeudsPrecedent;
        this.noeudsSuivant = n.noeudsSuivant;
        this.etat = n.etat;
        this.eventNumber = n.eventNumber;
    }

    public void AddNoeudSuivant(int numeroNoeud) {
        noeudsSuivant.Add(numeroNoeud);
    }

    public void AddNoeudPrecedent(int numeroNoeud) {
        noeudsPrecedent.Add(numeroNoeud);
    }

    public void SetEtat(EtatNoeud nouvelleEtat) {
        this.etat = nouvelleEtat;
    }
}