using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarteJoueurForetController : MonoBehaviour {

    private Vector3 targetPos;
    private bool targetPosReached;

    private void Start() {
        targetPos = this.transform.position;  
        targetPosReached = true;      
    }

    private void Update() {
        if (this.transform.position != targetPos) {
            this.transform.position = Vector2.MoveTowards(this.transform.position, this.targetPos, 1.0f * Time.deltaTime);
        }
        else {
            if(!targetPosReached) {
                targetPosReached = true;
                GameObject carte = GameObject.Find("Carte Foret");
                CarteForetController carteController = (CarteForetController) carte.GetComponent(typeof(CarteForetController));
                carteController.UnlockPath();
            }
        }
    }

    public void UpdateTargetPosition(Vector3 newTargetPos) {
        targetPos = newTargetPos;
        targetPosReached = false;
    }

}