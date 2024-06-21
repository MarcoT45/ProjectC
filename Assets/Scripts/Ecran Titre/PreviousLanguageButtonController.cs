using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousLanguageButtonController : MonoBehaviour {

    public void PreviousLanguage() {
        GameObject language = GameObject.Find("Langue Actuelle");
        LanguageOptionController l = (LanguageOptionController) language.GetComponent(typeof(LanguageOptionController));
        l.ChangeLanguage(-1);
    }
}