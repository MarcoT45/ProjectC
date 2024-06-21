using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseOptionMenuButtonController : MonoBehaviour {

    public GameObject popUpOptions;
    private bool isClosing = false;

    private void FixedUpdate() {

        if(isClosing) {
            Vector3 targetAngle = new Vector3(90, 0, 0);

            if (Vector3.Distance(popUpOptions.transform.eulerAngles, targetAngle) > 0.01f) {
                popUpOptions.transform.Rotate(2.5f, 0, 0);
            } else {
                isClosing = false;
                popUpOptions.transform.eulerAngles = new Vector3(-90, 0, 0);
            }
        }
    }

    public void FermerMenuOption() {
        isClosing = true;
    }

}