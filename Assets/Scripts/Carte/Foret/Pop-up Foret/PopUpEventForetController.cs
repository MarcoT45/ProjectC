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

    // Fenetre des choix
    public GameObject choiceWindow;
    public TextMeshProUGUI texteChoice1;
    public TextMeshProUGUI texteChoice2;
    public GameObject cursor1;
    public GameObject cursor2;

    // Texte dans la zone de texte de la pop-up
    public TextMeshProUGUI textePopUp;

    // Sons de la pop-up
    public AudioClip bushSFXTrack;
    public AudioClip textSFXTrack;
    public AudioClip menuMoveSFXTrack;

    // La liste des images pour la pop-up
    public List<Sprite> spriteList = new List<Sprite>();

    // Variable pour gerer les differents elements de la pop-up
    private int randomNumberEvent;
    private string textValue;
    private int eventType;
    private int choiceNumber = 1;

    ////////////////////////////////// Partie pour les controles ////////////////////////////////////////

    private void Update() {
        if(ControlsManager.Instance.controlsState == ControlsState.CarteChoiceWindow) {
            DeplacerChoix();
            ValiderChoix();
        }
    }

    private void DeplacerChoix() {
        if (ControlsManager.Instance.DeplacerPressed) {
            int val = (int) ControlsManager.Instance.DeplacerValue.y;
            switch (val) {
                case -1:
                    if ( (choiceNumber - 1) == 0) {
                        choiceNumber = 2;
                    } else {
                        choiceNumber = choiceNumber - 1;
                    }
                    break;
                case 1:
                    if ( (choiceNumber + 1) == 3) {
                        choiceNumber = 1;
                    } else {
                        choiceNumber = choiceNumber + 1;
                    }
                    break;
            }

            if(choiceNumber == 1) {
                cursor1.SetActive(true);
                cursor2.SetActive(false);
            } else {
                cursor1.SetActive(false);
                cursor2.SetActive(true);
            }

            SoundFXManager.Instance.PlaySoundFXClip(menuMoveSFXTrack, this.transform);
        }
    }

    private void ValiderChoix() {
        if (ControlsManager.Instance.ValiderPressed) {
            if(choiceNumber == 1) {
                ContinuerPopUpEvent();
            } else {
                FermerPopUpEvent();
            }
        }
    }

    public void MouseDeplacerChoix(int choiceValue) {
        if(choiceNumber != choiceValue) {
            SoundFXManager.Instance.PlaySoundFXClip(menuMoveSFXTrack, this.transform);
            choiceNumber = choiceValue;
            if(choiceNumber == 1) {
                cursor1.SetActive(true);
                cursor2.SetActive(false);
            } else {
                cursor1.SetActive(false);
                cursor2.SetActive(true);
            }
        }
    }

    public void MouseValiderChoix() {
        if(choiceNumber == 1) {
            ContinuerPopUpEvent();
        } else {
            FermerPopUpEvent();
        }
    }

    ////////////////////////////////// Partie pour les controles ////////////////////////////////////////

    public void GeneratePopUpEvent(Noeud n) {
        eventType = n.eventNumber;
        SetInfoEvent(eventType);
        OpenPopUpAnimation();
    }

    private async void OpenPopUpAnimation() {
        this.gameObject.SetActive(true);
        await conteneur.transform.DOScale(new Vector3(1, 1 ,1), 2f).AsyncWaitForCompletion();

        partieCentrale.GetComponent<Image>().DOFade(1.0f, 2.5f);
        sujetPopUp.GetComponent<Image>().DOFade(1.0f, 2.5f);
        await partieCentrale.GetComponent<RectTransform>().DOAnchorPosY(0, 2.5f, false).AsyncWaitForCompletion();

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
                texteChoice1.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionRest");
                texteChoice2.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionLeave");
                break;
            case 4: // Magasin
                sujetPopUp.GetComponent<Image>().sprite = spriteList[1];
                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ShopEventIntro");
                texteChoice1.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionTrade");
                texteChoice2.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionLeave");
                break;
            case 5: // Echange
                sujetPopUp.GetComponent<Image>().sprite = spriteList[2];
                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "TradeEventIntro");
                texteChoice1.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionExchange");
                texteChoice2.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionLeave");
                break;
            case 6: // Coffre
                sujetPopUp.GetComponent<Image>().sprite = spriteList[3];
                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ChestEventIntro");
                texteChoice1.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionOpen");
                texteChoice2.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionLeave");
                break;
        }
    }

    private void GenerateRandomEvent() {
        randomNumberEvent = Random.Range(0, 2);
        switch (randomNumberEvent) {
            case 0:
                sujetPopUp.GetComponent<Image>().sprite = spriteList[4];
                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RandomEvent1Intro");
                texteChoice1.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionPunch");
                texteChoice2.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionLeave");
                break;
            case 1:
                sujetPopUp.GetComponent<Image>().sprite = spriteList[5];
                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RandomEvent2Intro");
                texteChoice1.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionSalute");
                texteChoice2.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionLeave");
                break;
        }
    }

    private void StartWritingText() {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText () {
        int compteur = 0;
        foreach (char letter in textValue.ToCharArray()) {
            textePopUp.text += letter;
            if((compteur%3) == 0) 
                SoundFXManager.Instance.PlaySoundFXClip(textSFXTrack, this.transform);
            yield return new WaitForSeconds (0.02f);
            compteur++;

            if (textePopUp.text == textValue) {
                OpenChoiceWindow();
            }
        }
    }

    private async void OpenChoiceWindow() {
        await choiceWindow.transform.DOScale(new Vector3(1, 1 ,1), 0.5f).AsyncWaitForCompletion();
        ControlsManager.Instance.UpdateState(301);
        cursor1.SetActive(true);
        cursor2.SetActive(false);
        choiceNumber = 1;
    }

    private async void FermerPopUpEvent() {
        await conteneur.transform.DOScale(new Vector3(0, 0 ,0), 0.5f).AsyncWaitForCompletion();
        partieCentrale.GetComponent<Image>().DOFade(0f, 0.5f);
        sujetPopUp.GetComponent<Image>().DOFade(0f, 0.5f);
        partieCentrale.GetComponent<RectTransform>().DOAnchorPosY(-110, 0.5f, false);
        buissonGauche.GetComponent<RectTransform>().DOAnchorPosX(-60, 0.5f, false);
        buissonDroite.GetComponent<RectTransform>().DOAnchorPosX(60, 0.5f, false);
        zoneTexteBas.GetComponent<RectTransform>().DOAnchorPosY(-87, 0.5f, false);
        textePopUp.text = "";
        choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 0.5f);
        cursor1.SetActive(true);
        cursor2.SetActive(false);
        this.gameObject.SetActive(false);
        ControlsManager.Instance.UpdateState(300);
    }

    private void ContinuerPopUpEvent() {
        Debug.Log("En travaux pour le moment ! Il faut executer les actions et changements ici !");
    }

}