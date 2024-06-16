using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour {

    private bool active = false;

    private void Start() {
        int languageId = PlayerPrefs.GetInt("LocaleKey");
        ChangeLocale(languageId);
    }

    public void ChangeLocale(int localeID) {
        if (this.active == true)
            return ;
        StartCoroutine(SetLocale(localeID));
    }

    IEnumerator SetLocale(int _localeID) {
        this.active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        PlayerPrefs.SetInt("LocaleKey", _localeID);
        this.active = false;
    }

}