using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLanguageButtonController : MonoBehaviour {

    public void NextLanguage() {
        GameObject language = GameObject.Find("Langue Actuelle");
        LanguageOptionController l = (LanguageOptionController) language.GetComponent(typeof(LanguageOptionController));
        l.ChangeLanguage(1);
    }
}