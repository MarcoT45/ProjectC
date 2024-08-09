using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayButtonController : MonoBehaviour {

    public TextMeshProUGUI text;

    public void Jouer() {
        SceneManager.LoadScene(1);
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