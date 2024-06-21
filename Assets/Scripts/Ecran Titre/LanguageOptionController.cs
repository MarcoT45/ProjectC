using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanguageOptionController : MonoBehaviour {

    private List<string> languageList = new List<string>{"English", "Français"};
    private int currentValue = 0;

    private void Start() {
        int languageId = PlayerPrefs.GetInt("LocaleKey");
        this.GetComponent<TextMeshProUGUI>().text = languageList[languageId];
        currentValue = languageId;
    }

    public void ChangeLanguage(int nextValue) {

        if( (currentValue+nextValue) < 0) {
            currentValue = languageList.Count - 1;
        } else {
            if( (currentValue+nextValue) > (languageList.Count - 1) ) {
                currentValue = 0;
            } else {
                currentValue = currentValue + nextValue;
            }
        }

        this.GetComponent<TextMeshProUGUI>().text = languageList[currentValue];
        
        GameObject locM = GameObject.Find("LocalizationManager");
        LocaleSelector sel = (LocaleSelector) locM.GetComponent(typeof(LocaleSelector));
        sel.ChangeLocale(currentValue);
    }

}