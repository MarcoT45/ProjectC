using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsWindowController : MonoBehaviour {

    public TextMeshProUGUI texteLangue;
    public TextMeshProUGUI texteGlobal;
    public TextMeshProUGUI texteMusique;
    public TextMeshProUGUI texteSFX;
    public TextMeshProUGUI texteReset;
    private int buttonNumber = 0;

    public Slider sliderGeneral;
    public Slider sliderMusique;
    public Slider sliderSFX;

    public GameObject popUpOptions;
    private bool isClosingOptionWindow = false;

    public GameObject popUpConfirmReset;
    private bool isOpeningPopUpReset = false;

    private void Update() {
        if(ControlsManager.Instance.controlsState == ControlsState.Options & !isClosingOptionWindow & !isOpeningPopUpReset) {
            Deplacer();
            Valider();
            Fermer();
        }
    }

    private void FixedUpdate() {
        if(isClosingOptionWindow) {
            Vector3 targetAngle = new Vector3(90, 0, 0);

            if (Vector3.Distance(popUpOptions.transform.eulerAngles, targetAngle) > 0.01f) {
                popUpOptions.transform.Rotate(2.5f, 0, 0);
            } else {
                popUpOptions.transform.eulerAngles = new Vector3(-90, 0, 0);
                isClosingOptionWindow = false;
                ControlsManager.Instance.UpdateState(1);
            }
        }

        if(isOpeningPopUpReset) {
            Vector3 targetAngle2 = new Vector3(0, 0, 0);

            if (Vector3.Distance(popUpConfirmReset.transform.eulerAngles, targetAngle2) > 0.01f) {
                popUpConfirmReset.transform.Rotate(2.5f, 0, 0);
            } else {
                popUpConfirmReset.transform.eulerAngles = targetAngle2;
                isOpeningPopUpReset = false;
                ControlsManager.Instance.UpdateState(4);
            }
        }
    }

    private void Deplacer() {
        if (ControlsManager.Instance.DeplacerPressed) {
            if((int) ControlsManager.Instance.DeplacerValue.x == 0) {
                int val = buttonNumber + (int) ControlsManager.Instance.DeplacerValue.y * -1;
                switch (val) {
                    case -1:
                        buttonNumber = 4;
                        texteReset.color = new Color(255, 0, 0, 255);
                        texteLangue.color = new Color(255, 255, 255, 255);
                        break;
                    case 0:
                        buttonNumber = 0;
                        texteLangue.color = new Color(255, 0, 0, 255);
                        texteGlobal.color = new Color(255, 255, 255, 255);
                        break;
                    case 1:
                        buttonNumber = 1;
                        texteLangue.color = new Color(255, 255, 255, 255);
                        texteGlobal.color = new Color(255, 0, 0, 255);
                        texteMusique.color = new Color(255, 255, 255, 255);
                        break;
                    case 2:
                        buttonNumber = 2;
                        texteGlobal.color = new Color(255, 255, 255, 255);
                        texteMusique.color = new Color(255, 0, 0, 255);
                        texteSFX.color = new Color(255, 255, 255, 255);
                        break;
                    case 3:
                        buttonNumber = 3;
                        texteMusique.color = new Color(255, 255, 255, 255);
                        texteSFX.color = new Color(255, 0, 0, 255);
                        texteReset.color = new Color(255, 255, 255, 255);
                        break;
                    case 4:
                        buttonNumber = 4;
                        texteSFX.color = new Color(255, 255, 255, 255);
                        texteReset.color = new Color(255, 0, 0, 255);
                        break;
                    case 5:
                        buttonNumber = 0;
                        texteReset.color = new Color(255, 255, 255, 255);
                        texteLangue.color = new Color(255, 0, 0, 255);
                        break;
                }
            } else {
                int modif = (int) ControlsManager.Instance.DeplacerValue.x;

                switch (buttonNumber) {
                    case 0:
                        if(modif < 0) {
                            LanguePrecedente();
                        } else {
                            LangueSuivante();
                        }
                        break;
                    case 1:
                        if(modif < 0) {
                            sliderGeneral.value = sliderGeneral.value - 0.1f;
                        } else {
                            sliderGeneral.value = sliderGeneral.value + 0.1f;
                        }
                        break;
                    case 2:
                        if(modif < 0) {
                            sliderMusique.value = sliderMusique.value - 0.1f;
                        } else {
                            sliderMusique.value = sliderMusique.value + 0.1f;
                        }
                        break;
                    case 3:
                        if(modif < 0) {
                            sliderSFX.value = sliderSFX.value - 0.1f;
                        } else {
                            sliderSFX.value = sliderSFX.value + 0.1f;
                        }
                        break;
                }
            }
        }
    }

    private void Valider() {
        if (ControlsManager.Instance.ValiderPressed) {
            if(buttonNumber == 4) {
                OpenPopUpReset();
            }
        }
    }

    private void Fermer() {
        if (ControlsManager.Instance.FermerPressed) {
            FermerOptionsWindow();
        }
    }

    public void FermerOptionsWindow() {
        isClosingOptionWindow = true;
    }

    public void OpenPopUpReset() {
        isOpeningPopUpReset = true;
    }

    public void LanguePrecedente() {
        GameObject language = GameObject.Find("Langue Actuelle");
        LanguageOptionController l = (LanguageOptionController) language.GetComponent(typeof(LanguageOptionController));
        l.ChangeLanguage(-1);
    }

    public void LangueSuivante() {
        GameObject language = GameObject.Find("Langue Actuelle");
        LanguageOptionController l = (LanguageOptionController) language.GetComponent(typeof(LanguageOptionController));
        l.ChangeLanguage(1);
    }

}