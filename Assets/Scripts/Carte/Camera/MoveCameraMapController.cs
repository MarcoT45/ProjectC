using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraMapController : MonoBehaviour {

    private float mouseSpeed = 0.65f;
    private float controllerSpeed = 0.1f;

    private void Update() {
        if(ControlsManager.Instance.controlsState == ControlsState.Carte) {
            MoveCamera();
        } 
    }

    private void MoveCamera() {

        if (ControlsManager.Instance.CameraPressed) {
            if(ControlsManager.Instance.GetCurrentDevice() == "Mouse") {
                if (ControlsManager.Instance.CameraValue.y != 0 && 
                    this.transform.position.y + (mouseSpeed * ControlsManager.Instance.CameraValue.y) < 21 &&
                    this.transform.position.y + (mouseSpeed * ControlsManager.Instance.CameraValue.y) > 0) {
                    
                    this.transform.position = new Vector3(0, this.transform.position.y + (mouseSpeed * ControlsManager.Instance.CameraValue.y), -10);
                }
            }
        }

        if (ControlsManager.Instance.CameraHold) {
            if(ControlsManager.Instance.GetCurrentDevice() != "Mouse") {
                if (ControlsManager.Instance.CameraValue.y != 0 && 
                    this.transform.position.y + (controllerSpeed * ControlsManager.Instance.CameraValue.y) < 21 &&
                    this.transform.position.y + (controllerSpeed * ControlsManager.Instance.CameraValue.y) > 0) {
                    
                    this.transform.position = new Vector3(0, this.transform.position.y + (controllerSpeed * ControlsManager.Instance.CameraValue.y), -10);
                }
            }
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