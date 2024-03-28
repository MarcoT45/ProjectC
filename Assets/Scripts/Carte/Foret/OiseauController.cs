using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OiseauController : MonoBehaviour {

    private float flyingSpeed = 0;
    private Vector2 targetPos;

    private void Start() {
        this.flyingSpeed = Random.Range(1f, 2f);
        this.transform.position = new Vector2(12, Random.Range(-2f, 12.5f));
        this.targetPos = new Vector2(-11, this.transform.position.y);
    }

    private void Update() {
        if (this.transform.position.x < -10) {
            this.flyingSpeed = Random.Range(1f, 2f);
            this.transform.position = new Vector2(12, Random.Range(-2f, 12.5f));
            this.targetPos = new Vector2(-11, this.transform.position.y);
        } else {
            this.transform.position = Vector2.MoveTowards(this.transform.position, this.targetPos, this.flyingSpeed * Time.deltaTime);
        }
    }

}