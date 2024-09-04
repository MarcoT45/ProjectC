using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenResetPopUpController : MonoBehaviour {

    public GameObject popUpConfirmReset;
    private bool isOpening = false;

    private void FixedUpdate() {

        if(isOpening) {
            Vector3 targetAngle = new Vector3(0, 0, 0);

            if (Vector3.Distance(popUpConfirmReset.transform.eulerAngles, targetAngle) > 0.01f) {
                popUpConfirmReset.transform.Rotate(2.5f, 0, 0);
            } else {
                popUpConfirmReset.transform.eulerAngles = targetAngle;
                isOpening = false;
            }
        }
    }

    public void OuvrirPopUpConfirmation() {
        isOpening = true;
    }
}