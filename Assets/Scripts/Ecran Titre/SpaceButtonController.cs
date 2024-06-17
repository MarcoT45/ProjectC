using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SpaceButtonController : MonoBehaviour {

    public Color couleur1;
    public Color couleur2;

    private void Update() {
        FlashingText();
        if(Input.GetKeyDown (KeyCode.Space)) {
            GameManager.Instance.NewRun();
            SceneManager.LoadScene(1);
        }
    }

    private void FlashingText() {
        this.GetComponent <TextMeshProUGUI>().color = Color.Lerp(couleur1, couleur2, Mathf.PingPong(Time.time, 1));
    }

}