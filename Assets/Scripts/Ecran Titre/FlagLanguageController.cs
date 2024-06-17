using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagLanguageController : MonoBehaviour {

    public List<Sprite> flags;
    private int currentValue = 0;

    private void Start() {
        int languageId = PlayerPrefs.GetInt("LocaleKey");
        this.GetComponent<Image>().sprite = flags[languageId];
        currentValue = languageId;
    }

    public void ChangeLanguage() {

        switch(currentValue) {
            case 0:
                currentValue = 1;
                break;
            case 1:
                currentValue = 0;
                break;
        }

        this.GetComponent<Image>().sprite = flags[currentValue];

        GameObject locM = GameObject.Find("LocalizationManager");
        LocaleSelector sel = (LocaleSelector) locM.GetComponent(typeof(LocaleSelector));
        sel.ChangeLocale(currentValue);
    }

}