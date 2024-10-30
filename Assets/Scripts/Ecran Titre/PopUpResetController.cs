using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpResetController : MonoBehaviour {

    public GameObject popUpReset;
    private bool isClosingPopUpReset = false;

    private void Update() {
        if(ControlsManager.Instance.controlsState == ControlsState.PopUpOuiNon & !isClosingPopUpReset) {
            Valider();
            Fermer();
        }
    }

    private void FixedUpdate() {
        if(isClosingPopUpReset) {
            Vector3 targetAngle = new Vector3(90, 0, 0);

            if (Vector3.Distance(popUpReset.transform.eulerAngles, targetAngle) > 0.01f) {
                popUpReset.transform.Rotate(2.5f, 0, 0);
            } else {
                popUpReset.transform.eulerAngles = new Vector3(-90, 0, 0);
                isClosingPopUpReset = false;
                ControlsManager.Instance.UpdateState(2);
            }
        }
    }

    private void Valider() {
        if (ControlsManager.Instance.ValiderPressed) {
            OnClickButtonValider();
        }
    }

    private void Fermer() {
        if (ControlsManager.Instance.FermerPressed) {
            OnClickButtonFermer();
        }
    }

    public void OnClickButtonValider() {
        SaveManager.Instance.ResetData();
        isClosingPopUpReset = true;
    }

    public void OnClickButtonFermer() {
        isClosingPopUpReset = true;
    }

}