using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionButtonController : MonoBehaviour {

    public TextMeshProUGUI text;

    public void OuvrirMenuOption() {
        Debug.Log("Ouverture du menu option");
    }

    public void OnButtonOver() {
        this.text.fontSize = 140;
        this.text.color = new Color(255, 0, 0, 255);
    }

    public void OnButtonExit() {
        this.text.fontSize = 108;
        this.text.color = new Color(255, 255, 255, 255);
    }

}