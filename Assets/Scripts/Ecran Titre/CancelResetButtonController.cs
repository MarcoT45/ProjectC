using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelResetButtonController : MonoBehaviour {

    public GameObject popUpConfirmReset;
    private bool isClosing = false;

    private void FixedUpdate() {

        if(isClosing) {
            Vector3 targetAngle = new Vector3(90, 0, 0);

            if (Vector3.Distance(popUpConfirmReset.transform.eulerAngles, targetAngle) > 0.01f) {
                popUpConfirmReset.transform.Rotate(2.5f, 0, 0);
            } else {
                isClosing = false;
                popUpConfirmReset.transform.eulerAngles = new Vector3(-90, 0, 0);
            }
        }
    }

    public void Annuler() {
        isClosing = true;
    }

}