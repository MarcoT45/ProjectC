using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBookController : MonoBehaviour {

    // Pour gerer l'ouverture/fermeture du livre
    public GameObject popUpBookLeft;
    public GameObject popUpBookRight;
    private bool isOpening = false;
    private bool isClosing = false;
    private bool inFrontOfBook = false;
    private bool isOpen = false;

    // Pour pouvoir bloquer le déplacement du joueur
    public GameObject character;
    private CharacterController charControl;

    // Pour afficher le nom du livre + le espace
    public GameObject titleSpace;

    private void Start() {
        charControl = (CharacterController) character.GetComponent(typeof(CharacterController));
    }

    private void FixedUpdate() {

        if(isOpening) {
            Vector3 targetAngle = new Vector3(0, 0, 0);

            if (Vector3.Distance(popUpBookLeft.transform.eulerAngles, targetAngle) > 0.01f || Vector3.Distance(popUpBookRight.transform.eulerAngles, targetAngle) < -0.01f) {
                popUpBookLeft.transform.Rotate(0, -2.5f, 0);
                popUpBookRight.transform.Rotate(0, 2.5f, 0);
            } else {
                popUpBookLeft.transform.eulerAngles = targetAngle;
                popUpBookRight.transform.eulerAngles = targetAngle;
                isOpening = false;
                isOpen = true;
            }
        }

        if(isClosing) {
            Vector3 targetAngleLeft = new Vector3(0, 90, 0);
            Vector3 targetAngleRight = new Vector3(0, -90, 0);

            if (Vector3.Distance(popUpBookLeft.transform.eulerAngles, targetAngleLeft) > 0.01f || Vector3.Distance(popUpBookRight.transform.eulerAngles, targetAngleRight) < -0.01f) {
                popUpBookLeft.transform.Rotate(0, 2.5f, 0);
                popUpBookRight.transform.Rotate(0, -2.5f, 0);
            } else {
                popUpBookLeft.transform.eulerAngles = targetAngleLeft;
                popUpBookRight.transform.eulerAngles = targetAngleRight;
                isClosing = false;
                isOpen = false;
            }
        }
    }

    private void Update() {
        if (Input.GetKeyDown("space") && inFrontOfBook && !isOpen) {
            OpenBook();
        }

        if (Input.GetKeyDown("space") && inFrontOfBook && isOpen) {
            CloseBook();
        }
    }

    public void OpenBook() {
        isOpening = true;
        charControl.OnDisable();
    }

    public void CloseBook() {
        isClosing = true;
        charControl.OnEnable();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        inFrontOfBook = true;
        titleSpace.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other) {
        inFrontOfBook = false;
        titleSpace.SetActive(false);
    }

}