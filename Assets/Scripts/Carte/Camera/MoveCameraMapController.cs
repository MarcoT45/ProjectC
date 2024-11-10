using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraMapController : MonoBehaviour {

    private void Update() {
        if ( Input.GetAxis("Mouse ScrollWheel") != 0 && this.transform.position.y + (5 * Input.GetAxis("Mouse ScrollWheel")) < 21 && this.transform.position.y + (5 * Input.GetAxis("Mouse ScrollWheel")) > 0) {
            this.transform.position = new Vector3(0, this.transform.position.y + (5 * Input.GetAxis("Mouse ScrollWheel")), -10);
        }
    }

    public void SetCameraToPlayerPosition(float posY) {
        
        if (posY < 0)
            posY = 0;

        if (posY > 21)
            posY = 21;

        this.transform.position = new Vector3(0, posY, -10);
    }
}   