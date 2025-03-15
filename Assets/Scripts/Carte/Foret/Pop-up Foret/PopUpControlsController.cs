using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using TMPro;

public class PopUpControlsController : MonoBehaviour {

    // Elements qui composent la barre
    public GameObject touche1;
    public TextMeshProUGUI texteTouche1;
    public GameObject touche2;
    public TextMeshProUGUI texteTouche2;
    public GameObject touche3;
    public TextMeshProUGUI texteTouche3;

    // Sprites des touches
    public Sprite keyboardTouche1;
    public Sprite keyboardTouche2;
    public Sprite keyboardTouche3;
    public Sprite playstationTouche1;
    public Sprite playstationTouche2;
    public Sprite playstationTouche3;
    public Sprite xboxTouche1;
    public Sprite xboxTouche2;
    public Sprite xboxTouche3;

    // Variables pour gerer la barre
    private string lastDeviceUsed = "";

    private void Update() {
        if(lastDeviceUsed != ControlsManager.Instance.GetCurrentDevice()) {
            lastDeviceUsed = ControlsManager.Instance.GetCurrentDevice();
            
            if (lastDeviceUsed == "Keyboard" || lastDeviceUsed == "Mouse") {
                touche1.GetComponent<Image>().sprite = keyboardTouche1;
                touche2.GetComponent<Image>().sprite = keyboardTouche2;
                touche3.GetComponent<Image>().sprite = keyboardTouche3;
            } else if (lastDeviceUsed.Contains("DualShock")) {
                touche1.GetComponent<Image>().sprite = playstationTouche1;
                touche2.GetComponent<Image>().sprite = playstationTouche2;
                touche3.GetComponent<Image>().sprite = playstationTouche3;
            } else {
                touche1.GetComponent<Image>().sprite = xboxTouche1;
                touche2.GetComponent<Image>().sprite = xboxTouche2;
                touche3.GetComponent<Image>().sprite = xboxTouche3;
            }
        }
    }

    public void SetTradeControls() {
        touche1.SetActive(true);
        texteTouche1.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ControleDeplacement");
        touche2.SetActive(true);
        texteTouche2.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ControleConfirmer");
        touche3.SetActive(false);
        texteTouche3.text = "";
    }

    public void SetShopControls() {
        touche1.SetActive(true);
        texteTouche1.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ControleDeplacement");
        touche2.SetActive(true);
        texteTouche2.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ControleConfirmer");
        touche3.SetActive(true);
        texteTouche3.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ControleQuitter");
    }
    
}