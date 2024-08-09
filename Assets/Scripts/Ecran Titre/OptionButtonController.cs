using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionButtonController : MonoBehaviour {

    public TextMeshProUGUI text;

    public GameObject popUpOptions;
    private bool isOpening = false;

    private void FixedUpdate() {

        if(isOpening) {
            Vector3 targetAngle = new Vector3(0, 0, 0);

            if (Vector3.Distance(popUpOptions.transform.eulerAngles, targetAngle) > 0.01f) {
                popUpOptions.transform.Rotate(2.5f, 0, 0);
            } else {
                popUpOptions.transform.eulerAngles = targetAngle;
                isOpening = false;
            }
        }
    }

    public void OuvrirMenuOption() {
        isOpening = true;
    }

    public void OnButtonOver() {
        this.text.fontSize = 32;
        this.text.color = new Color(255, 0, 0, 255);
    }

    public void OnButtonExit() {
        this.text.fontSize = 16;
        this.text.color = new Color(255, 255, 255, 255);
    }

}