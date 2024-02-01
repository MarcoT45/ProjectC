using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlayerController : MonoBehaviour {

    private Vector3 targetPos;

    private void Start() {
        this.targetPos = this.transform.position;
    }

    private void Update() {
        /*
        while (this.transform.position != this.targetPos) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetPos, 0.1f * Time.deltaTime);
        }
        */
    }

    public void UpdateTargetPos(Vector3 newPos) {
        this.targetPos = newPos;
    }

}