using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCorruptedPopUpButtonConroller : MonoBehaviour {

    public GameObject popUpCorrupted;
    private bool isClosing = false;

    private void FixedUpdate() {

        if(isClosing) {
            Vector3 targetAngle = new Vector3(90, 0, 0);

            if (Vector3.Distance(popUpCorrupted.transform.eulerAngles, targetAngle) > 0.01f) {
                popUpCorrupted.transform.Rotate(2.5f, 0, 0);
            } else {
                isClosing = false;
                popUpCorrupted.transform.eulerAngles = new Vector3(-90, 0, 0);
            }
        }
    }

    public void FermerPopUpCorruption() {
        isClosing = true;
    }
}