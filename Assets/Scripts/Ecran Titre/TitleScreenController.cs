using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreenController : MonoBehaviour {

    [SerializeField] private AudioClip titleScreenTrack;

    public TextMeshProUGUI texteJouer;
    public TextMeshProUGUI texteOptions;
    public TextMeshProUGUI texteQuitter;
    private int buttonNumber = 0;

    public GameObject popUpOptions;
    private bool isOptionsOpening = false;

    private void Start() {
        MusicManager.Instance.PlayMusicClip(titleScreenTrack);
    }

    private void Update() {
        if(ControlsManager.Instance.controlsState == ControlsState.TitleScreen & !isOptionsOpening) {
            Deplacer();
            Valider();
        }
    }

    private void FixedUpdate() {

        if(isOptionsOpening) {
            Vector3 targetAngle = new Vector3(0, 0, 0);

            if (Vector3.Distance(popUpOptions.transform.eulerAngles, targetAngle) > 0.01f) {
                popUpOptions.transform.Rotate(2.5f, 0, 0);
            } else {
                popUpOptions.transform.eulerAngles = targetAngle;
                isOptionsOpening = false;
                ControlsManager.Instance.UpdateState(2);
            }
        }
    }

    private void Deplacer() {
        if (ControlsManager.Instance.DeplacerPressed) {
            int val = buttonNumber + (int) ControlsManager.Instance.DeplacerValue.y * -1;
            switch (val) {
                case -1:
                    buttonNumber = 2;
                    texteJouer.color = new Color(255, 255, 255, 255);
                    texteQuitter.color = new Color(255, 0, 0, 255);
                    break;
                case 0:
                    buttonNumber = 0;
                    texteJouer.color = new Color(255, 0, 0, 255);
                    texteOptions.color = new Color(255, 255, 255, 255);
                    break;
                case 1:
                    buttonNumber = 1;
                    texteJouer.color = new Color(255, 255, 255, 255);
                    texteOptions.color = new Color(255, 0, 0, 255);
                    texteQuitter.color = new Color(255, 255, 255, 255);
                    break;
                case 2:
                    buttonNumber = 2;
                    texteOptions.color = new Color(255, 255, 255, 255);
                    texteQuitter.color = new Color(255, 0, 0, 255);
                    break;
                case 3:
                    buttonNumber = 0;
                    texteJouer.color = new Color(255, 0, 0, 255);
                    texteQuitter.color = new Color(255, 255, 255, 255);
                    break;
            }
        }
    }

    private void Valider() {
        if (ControlsManager.Instance.ValiderPressed) {
            switch (buttonNumber) {
                case 0:
                    ControlsManager.Instance.UpdateState(5);
                    SceneManager.LoadScene(1);
                    break;
                case 1:
                    isOptionsOpening = true;
                    break;
                case 2:
                    Application.Quit();
                    break;
            }
        }
    }

    public void OnPlayButtonHover() {
        buttonNumber = 0;
        texteJouer.color = new Color(255, 0, 0, 255);
        texteOptions.color = new Color(255, 255, 255, 255);
        texteQuitter.color = new Color(255, 255, 255, 255);
    }

    public void OnPlayButtonClick() {
        ControlsManager.Instance.UpdateState(5);
        SceneManager.LoadScene(1);
    }

    public void OnOptionsButtonHover() {
        buttonNumber = 1;
        texteJouer.color = new Color(255, 255, 255, 255);
        texteOptions.color = new Color(255, 0, 0, 255);
        texteQuitter.color = new Color(255, 255, 255, 255);
    }

    public void OnOptionsButtonClick() {
        isOptionsOpening = true;
    }

    public void OnQuitButtonHover() {
        buttonNumber = 2;
        texteJouer.color = new Color(255, 255, 255, 255);
        texteOptions.color = new Color(255, 255, 255, 255);
        texteQuitter.color = new Color(255, 0, 0, 255);
    }

    public void OnQuitButtonClick() {
        Application.Quit();
    }

}