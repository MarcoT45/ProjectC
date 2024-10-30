using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsTestScript : MonoBehaviour {

    private void Update() {
        if(ControlsManager.Instance.controlsState == ControlsState.CharacterHub) {
            Deplacer();
            Trinket1();
            Trinket2();
            Pause();
            Valider();
            Fermer();
            Inventaire();
            ActionSpeciale();
        }
    }

    private void Deplacer() {
        if (ControlsManager.Instance.DeplacerPressed) {
            Debug.Log("On se déplace : "+ControlsManager.Instance.DeplacerValue);
        }
    }

    private void Trinket1() {
        if (ControlsManager.Instance.Trinket1Pressed) {
            Debug.Log("On utilise le trinket 1");
        }
    }

    private void Trinket2() {
        if (ControlsManager.Instance.Trinket2Pressed) {
            Debug.Log("On utilise le trinket 2");
        }
    }

    private void Pause() {
        if (ControlsManager.Instance.PausePressed) {
            Debug.Log("On appuie sur pause");
        }
    }

    private void Valider() {
        if (ControlsManager.Instance.ValiderPressed) {
            Debug.Log("On valide");
        }
    }

    private void Fermer() {
        if (ControlsManager.Instance.FermerPressed) {
            Debug.Log("On ferme");
        }
    }

    private void Inventaire() {
        if (ControlsManager.Instance.InventairePressed) {
            Debug.Log("On ouvre l'inventaire");
        }
    }

    private void ActionSpeciale() {
        if (ControlsManager.Instance.ActionSpecialePressed) {
            Debug.Log("On utilise l'action spéciale");
        }
    }

}