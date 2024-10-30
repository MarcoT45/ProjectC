using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpaceButtonController : MonoBehaviour {

    public Color couleur1;
    public Color couleur2;

    public GameObject playButton;
    public GameObject optionButton;
    public GameObject quitButton;

    private void Update() {
        if(ControlsManager.Instance.controlsState == ControlsState.InputTest) {
            Valider();
        }

        FlashingText();
    }

    private void Valider() {
        if (ControlsManager.Instance.ValiderPressed) {
            this.gameObject.SetActive(false);
            playButton.SetActive(true);
            optionButton.SetActive(true);
            quitButton.SetActive(true);
            ControlsManager.Instance.UpdateState(1);
        }
    }

    private void FlashingText() {
        this.GetComponent <TextMeshProUGUI>().color = Color.Lerp(couleur1, couleur2, Mathf.PingPong(Time.time, 1.5f));
    }

}