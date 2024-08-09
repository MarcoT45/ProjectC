using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuitButtonController : MonoBehaviour {

    public TextMeshProUGUI text;

    public void Quitter() {
        Application.Quit();
    }

    public void OnButtonOver() {
        this.text.fontSize = 32;
        this.text.color = new Color(255, 0, 0, 255);
    }

    public void OnButtonExit() {
        this.text.fontSize = 16;
        this.text.color = new Color(255, 255, 255, 255);
    }

}