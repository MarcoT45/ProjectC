using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpForetController : MonoBehaviour {

    public TextMeshProUGUI textePopUp;
    public GameObject conteneurPopUp;

    public GameObject feuDeCampPrefab;

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
                GameObject sujet = GameObject.Find("Sujet");
                Destroy(sujet);
                this.gameObject.SetActive(false);
            }
        }
    }

    public void CreatePopUpEvent(Noeud n) {
        UpdateTextPopUp(n.eventNumber);
        DisplayPopUp();
    }

    private void UpdateTextPopUp(int eventNumber) {

        GameObject sujet;

        switch (eventNumber) {
            case 3:
                textePopUp.text = "Vous vous reposez auprès d'un feu de camp.";
                sujet = Instantiate(feuDeCampPrefab, conteneurPopUp.transform);
                sujet.name = "Sujet";
                break;
            default:
                textePopUp.text = "Cet évenement n'a pas encore été crée :-)";
                sujet = Instantiate(feuDeCampPrefab, conteneurPopUp.transform);
                sujet.name = "Sujet";
                break;
        }

    }

    private void DisplayPopUp() {
        this.gameObject.SetActive(true);
        isOpening = true;
    }

    public void UndisplayPopUp() {
        isClosing = true;
    }

}