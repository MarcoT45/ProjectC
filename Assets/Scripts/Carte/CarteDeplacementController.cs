using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarteDeplacementController : MonoBehaviour {

    private int currentNode = 0;
    private List<int> nextNodes = new List<int>();
    private int currentPos = 0;
    private int lastPos = 0;

    private Color blackColor = new Color (0f, 0f, 0f, 1f);
    private Color whiteColor = new Color (1f, 1f, 1f, 1f);
    private Color greenColor = new Color(0.7450981f, 0.7372549f, 0.4156863f, 1);

    // Sons de la pop-up
    public AudioClip parcourirCarteSFXTrack;
    public AudioClip confirmerCarteSFXTrack;

    private void Start() {
        UpdateCurrentNode(currentNode);
    }

    private void Update() {
        if(ControlsManager.Instance.controlsState == ControlsState.Carte) {
            Deplacer();
            Valider();
        }
    }

    private void Deplacer() {
        if (ControlsManager.Instance.DeplacerPressed) {
            int val = (int) ControlsManager.Instance.DeplacerValue.x;
            switch (val) {
                case -1:
                    Gauche();
                    break;
                case 1:
                    Droite();
                    break;
            }
        }
    }

    private void Valider() {
        if (ControlsManager.Instance.ValiderPressed && currentPos != 0) {
            ConfirmerDeplacement();
        }
    }

    public void ConfirmerDeplacement() {
        if(ControlsManager.Instance.controlsState == ControlsState.Carte) {
            SoundFXManager.Instance.PlaySoundFXClip(confirmerCarteSFXTrack, this.transform);

            GameObject tmpNode = GameObject.Find("Noeud "+ nextNodes[currentPos-1]);
            tmpNode.GetComponent<FlashingController>().enabled = false;

            NoeudController node = (NoeudController) tmpNode.GetComponent(typeof(NoeudController));

            GameObject carte = GameObject.Find("Carte Foret");
            CarteForetController carteController = (CarteForetController) carte.GetComponent(typeof(CarteForetController));
            carteController.MovePlayer(node.noeud);
        }
    }

    private void Droite() {
        if(currentPos == 0) {
            currentPos = currentPos + 1;
        } else {
            UnshowPreviousNodeSelected(currentPos);
            if((currentPos + 1) > nextNodes.Count) {
                currentPos = 1;
            } else {
                currentPos = currentPos + 1;
            }
        }

        ShowCurrentNodeSelected();
    }

    private void Gauche() {
        if(currentPos == 0) {
            currentPos = nextNodes.Count;
        } else {
            UnshowPreviousNodeSelected(currentPos);
            if((currentPos - 1) == 0) {
                currentPos = nextNodes.Count;
            } else {
                currentPos = currentPos - 1;
            }
        }

        ShowCurrentNodeSelected();
    }

    private void ShowCurrentNodeSelected() {
        if(lastPos != currentPos) {
            SoundFXManager.Instance.PlaySoundFXClip(parcourirCarteSFXTrack, this.transform);
            lastPos = currentPos;
        }

        GameObject tmpNode = GameObject.Find("Noeud "+ nextNodes[currentPos-1]);
        tmpNode.GetComponent<SpriteRenderer>().color = blackColor;

        GameObject path = GameObject.Find("Chemin "+ currentNode +"-"+nextNodes[currentPos-1]);
        LineRenderer p = (LineRenderer) path.GetComponent(typeof(LineRenderer));
        p.startColor = greenColor;
        p.endColor = greenColor;

        tmpNode.GetComponent<FlashingController>().enabled = true;
    }

    private void UnshowPreviousNodeSelected(int previousPos) {
        GameObject tmpNode = GameObject.Find("Noeud "+ nextNodes[previousPos-1]);
        tmpNode.GetComponent<SpriteRenderer>().color = whiteColor;

        GameObject path = GameObject.Find("Chemin "+ currentNode +"-"+nextNodes[previousPos-1]);
        LineRenderer p = (LineRenderer) path.GetComponent(typeof(LineRenderer));
        p.startColor = whiteColor;
        p.endColor = whiteColor;

        tmpNode.GetComponent<FlashingController>().enabled = false;
    }

    public void UpdateCurrentNode(int n){
        GameObject tmpNode = GameObject.Find("Noeud "+ n);
        NoeudController nds = (NoeudController) tmpNode.GetComponent(typeof(NoeudController));
        this.currentNode = n;
        this.nextNodes = nds.noeud.noeudsSuivant;
        this.nextNodes.Sort();
        this.currentPos = 0;
    }

    public void OnMouseOverNode(int nodeId) {
        if(ControlsManager.Instance.controlsState == ControlsState.Carte) {
            if(currentPos != 0) {
                UnshowPreviousNodeSelected(currentPos);
            }

            for(int i = 0; i < nextNodes.Count; i++) {
               if(nextNodes[i] == nodeId) {
                    currentPos = i+1;
               }
            }

            ShowCurrentNodeSelected();
        }
    }

}