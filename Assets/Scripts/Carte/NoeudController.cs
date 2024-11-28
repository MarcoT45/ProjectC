using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoeudController : MonoBehaviour {

    public Noeud noeud = new Noeud();
    public List<Sprite> spriteList;

    private Color blackColor = new Color (0f, 0f, 0f, 1f);
    private Color greyColor = new Color (0.6f, 0.6f, 0.6f, 6f);
    private Color whiteColor = new Color (1f, 1f, 1f, 1f);

    public void UpdateEtatNoeud(EtatNoeud nouvelleEtat) {
        this.noeud.SetEtat(nouvelleEtat);

        if(this.noeud.etat == EtatNoeud.Accessible) {
            this.GetComponent<SpriteRenderer>().color = whiteColor;
        } else if (this.noeud.etat == EtatNoeud.Visite || this.noeud.etat == EtatNoeud.Joueur) {
            this.GetComponent<SpriteRenderer>().color = blackColor;
        } else {
            this.GetComponent<SpriteRenderer>().color = greyColor;
        }
    }

    public void UpdateCouleur() {
        if(this.noeud.etat == EtatNoeud.Accessible) {
            this.GetComponent<SpriteRenderer>().color = whiteColor;
        } else if (this.noeud.etat == EtatNoeud.Visite || this.noeud.etat == EtatNoeud.Joueur) {
            this.GetComponent<SpriteRenderer>().color = blackColor;
        } else {
            this.GetComponent<SpriteRenderer>().color = greyColor;
        }
    }

    public void ChangeNodeState(int spriteValue) {
        this.noeud.eventNumber = spriteValue;
        this.GetComponent<SpriteRenderer>().sprite = spriteList[spriteValue];
    }

    // Si on clique sur le noeud
    private void OnMouseDown() {
        if(this.noeud.etat == EtatNoeud.Accessible) {
            GameObject commandCarte = GameObject.Find("CarteDeplacementController");
            CarteDeplacementController cmdC = (CarteDeplacementController) commandCarte.GetComponent(typeof(CarteDeplacementController));
            cmdC.ConfirmerDeplacement();
        }
    }

    // Si on survole sur le noeud
    private void OnMouseOver() {
        if(this.noeud.etat == EtatNoeud.Accessible) {
            GameObject commandCarte = GameObject.Find("CarteDeplacementController");
            CarteDeplacementController cmdC = (CarteDeplacementController) commandCarte.GetComponent(typeof(CarteDeplacementController));
            cmdC.OnMouseOverNode(this.noeud.numero);
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