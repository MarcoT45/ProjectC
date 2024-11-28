using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingController : MonoBehaviour {

    private Color couleur1 = new Color (0f, 0f, 0f, 1f);
    private Color couleur2 = new Color (0f, 0f, 0f, 0f);

    private void Update() {
        this.GetComponent<SpriteRenderer>().color = Color.Lerp(couleur1, couleur2, Mathf.PingPong(Time.time, 0.9f));
    }

}