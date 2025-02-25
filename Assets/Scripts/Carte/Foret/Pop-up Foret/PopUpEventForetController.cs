using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using TMPro;
using DG.Tweening;

public class PopUpEventForetController : MonoBehaviour {

    // Elements de la pop-up
    public GameObject conteneur;
    public GameObject buissonGauche;
    public GameObject buissonDroite;
    public GameObject partieCentrale;
    public GameObject zoneTexteBas;
    public GameObject sujetPopUp;

    // Texte dans la zone de texte de la pop-up
    public TextMeshProUGUI textePopUp;

    // Sons de la pop-up
    public AudioClip bushSFXTrack;
    public AudioClip textSFXTrack;

    // La liste des images pour la pop-up
    public List<Sprite> spriteList = new List<Sprite>();

    // Variable pour gerer les differents elements de la pop-up
    private int randomNumberEvent;
    private string textValue;

    public void GeneratePopUpEvent(Noeud n) {
        SetInfoEvent(n.eventNumber);
        OpenPopUpAnimation();
    }

    private async void OpenPopUpAnimation() {
        this.gameObject.SetActive(true);
        await conteneur.transform.DOScale(new Vector3(1, 1 ,1), 2f).AsyncWaitForCompletion();

        partieCentrale.GetComponent<Image>().DOFade(1.0f, 2.5f);
        sujetPopUp.GetComponent<Image>().DOFade(1.0f, 2.5f);
        await partieCentrale.GetComponent<RectTransform>().DOAnchorPosY(10, 2.5f, false).AsyncWaitForCompletion();

        buissonGauche.GetComponent<RectTransform>().DOAnchorPosX(-200, 2.5f, false);
        buissonDroite.GetComponent<RectTransform>().DOAnchorPosX(200, 2.5f, false);
        SoundFXManager.Instance.PlaySoundFXClip(bushSFXTrack, this.transform);

        await zoneTexteBas.GetComponent<RectTransform>().DOAnchorPosY(-55, 2.0f, false).AsyncWaitForCompletion();
        StartWritingText();
    }

    private void SetInfoEvent(int eventNumber) {
        switch (eventNumber) {
            case 1: // Evenement aléatoire
                GenerateRandomEvent();
                break;
            case 3: // Feu de camp
                sujetPopUp.GetComponent<Image>().sprite = spriteList[0];
                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RestEventIntro");
                break;
            case 4: // Magasin
                sujetPopUp.GetComponent<Image>().sprite = spriteList[1];
                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ShopEventIntro");
                break;
            case 5: // Echange
                sujetPopUp.GetComponent<Image>().sprite = spriteList[2];
                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "TradeEventIntro");
                break;
            case 6: // Coffre
                sujetPopUp.GetComponent<Image>().sprite = spriteList[3];
                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ChestEventIntro");
                break;
        }
    }

    private void GenerateRandomEvent() {
        randomNumberEvent = Random.Range(0, 2);
        switch (randomNumberEvent) {
            case 0:
                sujetPopUp.GetComponent<Image>().sprite = spriteList[4];
                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RandomEvent1Intro");
                break;
            case 1:
                sujetPopUp.GetComponent<Image>().sprite = spriteList[5];
                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RandomEvent2Intro");
                break;
        }
    }

    private void StartWritingText() {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText () {
        int cpt = 0;
        foreach (char letter in textValue.ToCharArray()) {
            textePopUp.text += letter;
            if(cpt%3 == 0) 
                SoundFXManager.Instance.PlaySoundFXClip(textSFXTrack, this.transform);
            yield return new WaitForSeconds (0.02f);
            cpt++;

            if (textePopUp.text == textValue) {
                Debug.Log("On a écrit tout le texte ! On peut passer à la suite !");
            }
        }
    }

}