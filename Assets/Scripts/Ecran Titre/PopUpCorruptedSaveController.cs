using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpCorruptedSaveController : MonoBehaviour {

    public GameObject popUpCorrupted;
    private bool isClosingPopUpCorrupted = false;

    private void Update() {
        if(ControlsManager.Instance.controlsState == ControlsState.PopUpCorrupted & !isClosingPopUpCorrupted) {
            Fermer();
        }
    }

    private void FixedUpdate() {

        if(isClosingPopUpCorrupted) {
            Vector3 targetAngle = new Vector3(90, 0, 0);

            if (Vector3.Distance(popUpCorrupted.transform.eulerAngles, targetAngle) > 0.01f) {
                popUpCorrupted.transform.Rotate(2.5f, 0, 0);
            } else {
                isClosingPopUpCorrupted = false;
                popUpCorrupted.transform.eulerAngles = new Vector3(-90, 0, 0);
                ControlsManager.Instance.UpdateState(1);
            }
        }
    }

    private void Fermer() {
        if (ControlsManager.Instance.FermerPressed) {
            FermerPopUpCorrupted();
        }
    }

    public void FermerPopUpCorrupted() {
        isClosingPopUpCorrupted = true;
    }

}