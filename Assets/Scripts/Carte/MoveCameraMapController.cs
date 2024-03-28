using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraMapController : MonoBehaviour {

    private void Start() {
        // Déplacer la caméra en fonction de la position du joueur
    }

    private void Update() {
        if ( Input.GetAxis("Mouse ScrollWheel") != 0 && this.transform.position.y + (5 * Input.GetAxis("Mouse ScrollWheel")) < 9 && this.transform.position.y + (5 * Input.GetAxis("Mouse ScrollWheel")) > 0) {
            this.transform.position = new Vector3(0, this.transform.position.y + (5 * Input.GetAxis("Mouse ScrollWheel")), -10);
        }
    }

}   