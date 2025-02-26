using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceWindowController : MonoBehaviour {

    [SerializeField] private int choiceValue;
    [SerializeField] private PopUpEventForetController popEvent;

    // Si on clique sur le choix
    private void OnMouseDown() {
        if(ControlsManager.Instance.controlsState == ControlsState.CarteChoiceWindow) {
            popEvent.MouseValiderChoix();
        }
    }

    // Si on survole sur le choix
    private void OnMouseOver() {
        if(ControlsManager.Instance.controlsState == ControlsState.CarteChoiceWindow) {
            popEvent.MouseDeplacerChoix(choiceValue);
        }
    }
   
}
