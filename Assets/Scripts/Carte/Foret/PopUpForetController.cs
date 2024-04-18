using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpForetController : MonoBehaviour {

    public TextMeshProUGUI textePopUp;

    public GameObject popUpContent; 
    private bool isOpening = false;
    private bool isClosing = false;

    private void FixedUpdate() {

        if(isOpening) {
            Vector3 targetAngle = new Vector3(0, 0, 0);

            if (Vector3.Distance(popUpContent.transform.eulerAngles, targetAngle) > 0.01f) {
                popUpContent.transform.Rotate(2.5f, 0, 0);
            } else {
                popUpContent.transform.eulerAngles = targetAngle;
                isOpening = false;
            }
        }

        if(isClosing) {
            Vector3 targetAngle = new Vector3(90, 0, 0);

            if (Vector3.Distance(popUpContent.transform.eulerAngles, targetAngle) > 0.01f) {
                popUpContent.transform.Rotate(2.5f, 0, 0);
            } else {
                isClosing = false;
                popUpContent.transform.eulerAngles = new Vector3(-90, 0, 0);
                this.gameObject.SetActive(false);
            }
        }
    }

    public void CreatePopUpEvent(Noeud n) {
        UpdateTextPopUp();
        DisplayPopUp();
    }

    private void UpdateTextPopUp() {
        textePopUp.text = "Un évenement à lieu ! Ceci est un test !";
    }

    private void DisplayPopUp() {
        this.gameObject.SetActive(true);
        isOpening = true;
    }

    public void UndisplayPopUp() {
        isClosing = true;
    }

}