using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoScrollDescriptionController : MonoBehaviour {

    // Pour savoir si le livre est ouvert
    public GameObject popUpBookLeft;
    public GameObject popUpBookRight;

    private float previousPosition;

    private void Start() {
        previousPosition = this.transform.localPosition.y;
    }

    private void FixedUpdate() {
        if(isBookOpen()) {
            if(isDescriptionLongEnough()){
                if(ScrollToTheEnd()) {
                    this.transform.localPosition = new Vector3(this.transform.localPosition.x, 0, 0);
                    previousPosition = this.transform.localPosition.y;
                }
            }
        }
    }

    private bool ScrollToTheEnd() {
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 0.1f, 0);

        if(previousPosition + 0.1f > this.transform.localPosition.y + 0.1f) {
            return true;
        } else {
            previousPosition = this.transform.localPosition.y;
            return false;
        }
    }

    private bool isDescriptionLongEnough() {
        RectTransform rt = (RectTransform) this.GetComponent(typeof(RectTransform));
        if(rt.sizeDelta.y > 15f) {
            return true;
        } else {
            return false;
        }
    }

    private bool isBookOpen() {
        Vector3 targetAngle = new Vector3(0, 0, 0);
        if (Vector3.Distance(popUpBookLeft.transform.eulerAngles, targetAngle) == 0 || Vector3.Distance(popUpBookRight.transform.eulerAngles, targetAngle) == 0) {
            return true;
        } else {
            return false;
        }
    }

}