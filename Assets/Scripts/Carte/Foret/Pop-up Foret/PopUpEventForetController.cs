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
    public GameObject objetCoffre;

    // Fenetre des choix
    public GameObject choiceWindow;
    public TextMeshProUGUI texteChoice1;
    public TextMeshProUGUI texteChoice2;
    public GameObject cursor1;
    public GameObject cursor2;
    public GameObject leaveWindow;

    // Texte dans la zone de texte de la pop-up
    public TextMeshProUGUI textePopUp;

    // Gameobject pour l'animation de repos
    public GameObject voletNoirHaut;
    public GameObject voletNoirBas;

    // Sons de la pop-up
    public AudioClip popupOpenSFXTrack;
    public AudioClip bushSFXTrack;
    public AudioClip textSFXTrack;
    public AudioClip selectChoiceSFXTrack;
    public AudioClip confirmChoiceSFXTrack;
    public AudioClip restSFXTrack;
    public AudioClip dodgeSFXTrack;
    public AudioClip punchSFXTrack;
    public AudioClip punchedSFXTrack;
    public AudioClip drumRollSFXTrack;
    public AudioClip fanfareSFXTrack;

    // La liste des images pour la pop-up
    public List<Sprite> spriteList = new List<Sprite>();
    public Sprite openChest;

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

         if(ControlsManager.Instance.controlsState == ControlsState.CarteLeaveWindow) {
            ValiderPartir();
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

            SoundFXManager.Instance.PlaySoundFXClip(selectChoiceSFXTrack, this.transform);
        }
    }

    private void ValiderChoix() {
        if (ControlsManager.Instance.ValiderPressed) {
            if(choiceNumber == 1) {
                ContinuerPopUpEvent();
            } else {
                FermerPopUpEvent();
            }
            SoundFXManager.Instance.PlaySoundFXClip(confirmChoiceSFXTrack, this.transform);
        }
    }

    public void MouseDeplacerChoix(int choiceValue) {
        if(choiceNumber != choiceValue) {
            SoundFXManager.Instance.PlaySoundFXClip(selectChoiceSFXTrack, this.transform);
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
        SoundFXManager.Instance.PlaySoundFXClip(confirmChoiceSFXTrack, this.transform);
    }

    private void ValiderPartir() {
        if (ControlsManager.Instance.ValiderPressed) {
            FermerPopUpEvent();
            SoundFXManager.Instance.PlaySoundFXClip(confirmChoiceSFXTrack, this.transform);
        }
    }

    public void MouseValiderPartir() {
        FermerPopUpEvent();
        SoundFXManager.Instance.PlaySoundFXClip(confirmChoiceSFXTrack, this.transform);
    }

    ////////////////////////////////// Partie pour les controles ////////////////////////////////////////

    public void GeneratePopUpEvent(Noeud n) {
        eventType = n.eventNumber;
        SetInfoEvent();
        OpenPopUpAnimation();
    }

    private async void OpenPopUpAnimation() {
        this.gameObject.SetActive(true);
        SoundFXManager.Instance.PlaySoundFXClip(popupOpenSFXTrack, this.transform);
        await conteneur.transform.DOScale(new Vector3(1, 1 ,1), 1.5f).AsyncWaitForCompletion();

        partieCentrale.GetComponent<Image>().DOFade(1.0f, 1.5f);
        sujetPopUp.GetComponent<Image>().DOFade(1.0f, 1.5f);
        await partieCentrale.GetComponent<RectTransform>().DOAnchorPosY(0, 1.5f, false).AsyncWaitForCompletion();

        buissonGauche.GetComponent<RectTransform>().DOAnchorPosX(-200, 2.0f, false);
        buissonDroite.GetComponent<RectTransform>().DOAnchorPosX(200, 2.0f, false);
        SoundFXManager.Instance.PlaySoundFXClip(bushSFXTrack, this.transform);

        await zoneTexteBas.GetComponent<RectTransform>().DOAnchorPosY(-55, 2.0f, false).AsyncWaitForCompletion();
        StartWritingTextIntro();
    }

    private void SetInfoEvent() {
        switch (eventType) {
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
        randomNumberEvent = Random.Range(0, 1); // JE METS A 1 ICI POUR QUE L'EVENT ALEATOIRE SOIT TOUJOURS LE 1ER POUR TESTER L'ANIM
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

    private void StartWritingTextIntro() {
        StartCoroutine(TypeTextIntro());
    }

    IEnumerator TypeTextIntro () {
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
        await conteneur.transform.DOScale(new Vector3(0, 0 ,0), 1.5f).AsyncWaitForCompletion();
        partieCentrale.GetComponent<Image>().DOFade(0f, 0.5f);
        sujetPopUp.GetComponent<Image>().DOFade(0f, 0.5f);
        partieCentrale.GetComponent<RectTransform>().DOAnchorPosY(-110, 0.5f, false);
        buissonGauche.GetComponent<RectTransform>().DOAnchorPosX(-60, 0.5f, false);
        buissonDroite.GetComponent<RectTransform>().DOAnchorPosX(60, 0.5f, false);
        zoneTexteBas.GetComponent<RectTransform>().DOAnchorPosY(-87, 0.5f, false);
        objetCoffre.GetComponent<Image>().DOFade(0f, 0.5f);
        objetCoffre.GetComponent<RectTransform>().DOAnchorPosY(17, 0.5f, false);
        textePopUp.text = "";
        choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 0.5f);
        leaveWindow.transform.DOScale(new Vector3(0, 0 ,0), 0.5f);
        cursor1.SetActive(true);
        cursor2.SetActive(false);
        this.gameObject.SetActive(false);
        ControlsManager.Instance.UpdateState(300);
    }

    private void ContinuerPopUpEvent() {
        // On met les controles du joueur en pause
        ControlsManager.Instance.UpdateState(302);

        switch (eventType) {
            case 1: // Evenement aléatoire   
                switch (randomNumberEvent) {
                    case 0: // Evenement aléatoire n°1
                        int randomSuccessEvent = Random.Range(0, 101);
                        if(randomSuccessEvent > 39) {
                            textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RandomEvent1GoodEnd");
                            StartEvent1SuccessAnimation();
                        } else {
                            textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RandomEvent1BadEnd");
                            StartEvent1FailureAnimation();
                        }
                        break;
                    case 1: // Evenement aléatoire n°2
                        Debug.Log("Action: evenement aléatoire n°2 en travaux !");
                        ControlsManager.Instance.UpdateState(301);
                        break;
                }
                break;
            case 3: // Feu de camp
                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RestEventEnd");
                StartRestAnimation();
                break;
            case 4: // Magasin
                Debug.Log("Action: magasin en travaux !");
                ControlsManager.Instance.UpdateState(301);
                break;
            case 5: // Echange
                Debug.Log("Action: echange en travaux !");
                ControlsManager.Instance.UpdateState(301);
                break;
            case 6: // Coffre
                int randomItemNumber;
                int randomRarityRate = Random.Range(0, 101);
                ItemData randomItem;
                
                if(randomRarityRate > 90) {
                    randomItemNumber = Random.Range(0, GameManager.Instance.itemsTriRarete[2].Count);
                    randomItem = GameManager.Instance.itemsTriRarete[2][randomItemNumber];
                } else if (randomRarityRate > 60) {
                    randomItemNumber = Random.Range(0, GameManager.Instance.itemsTriRarete[1].Count);
                    randomItem = GameManager.Instance.itemsTriRarete[1][randomItemNumber];
                } else {
                    randomItemNumber = Random.Range(0, GameManager.Instance.itemsTriRarete[0].Count);
                    randomItem = GameManager.Instance.itemsTriRarete[0][randomItemNumber];
                }

                objetCoffre.GetComponent<Image>().sprite = randomItem.GetSprite();

                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ChestEventEnd");
                StartChestAnimation();
                break;
        }
    }

    // Animation de l'event aléatoire 1 si on reussit
    private async void StartEvent1SuccessAnimation() {
        await choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 0.5f).AsyncWaitForCompletion();
        textePopUp.text = "";

        SoundFXManager.Instance.PlaySoundFXClip(punchSFXTrack, this.transform);
        await choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 0.5f).AsyncWaitForCompletion();

        await sujetPopUp.GetComponent<RectTransform>().DOShakeAnchorPos(2.0f, 5.0f, 5, 10f, false, true).AsyncWaitForCompletion();
        sujetPopUp.GetComponent<Image>().DOFade(0f, 1f);

        // APPLIQUER L'EFFET DE L'EVENEMENT ICI, GAGNER DE L'ARGENT (200 pièces ?)
        StartWritingTextEnd();
    }

    // Animation de l'event aléatoire 1 si on échoue
    private async void StartEvent1FailureAnimation() {
        await choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 0.5f).AsyncWaitForCompletion();
        textePopUp.text = "";

        SoundFXManager.Instance.PlaySoundFXClip(dodgeSFXTrack, this.transform);
        await sujetPopUp.GetComponent<RectTransform>().DOAnchorPosX(-20, 0.5f, false).AsyncWaitForCompletion();
        await sujetPopUp.GetComponent<RectTransform>().DOAnchorPosX(0, 0.5f, false).AsyncWaitForCompletion();
        
        SoundFXManager.Instance.PlaySoundFXClip(punchedSFXTrack, this.transform);
        await choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 0.25f).AsyncWaitForCompletion();
        await conteneur.GetComponent<RectTransform>().DOShakeAnchorPos(2.0f, 5.0f, 5, 10f, false, true).AsyncWaitForCompletion();

        voletNoirHaut.GetComponent<RectTransform>().DOAnchorPosY(50, 0.5f, false);
        await voletNoirBas.GetComponent<RectTransform>().DOAnchorPosY(-50, 0.5f, false).AsyncWaitForCompletion();

        sujetPopUp.GetComponent<Image>().DOFade(0f, 0.1f);
        await choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 2.5f).AsyncWaitForCompletion();

        voletNoirHaut.GetComponent<RectTransform>().DOAnchorPosY(121, 2f, false);
        await voletNoirBas.GetComponent<RectTransform>().DOAnchorPosY(-121, 2f, false).AsyncWaitForCompletion();

        // APPLIQUER L'EFFET DE L'EVENEMENT ICI, PERDRE DE L'ARGENT (20%)
        StartWritingTextEnd();
    }

    // Animation lorsque l'on choisit de se reposer au feu de camp
    private async void StartRestAnimation() {
        await choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 0.5f).AsyncWaitForCompletion();
        textePopUp.text = "";

        voletNoirHaut.GetComponent<RectTransform>().DOAnchorPosY(50, 0.75f, false);
        await voletNoirBas.GetComponent<RectTransform>().DOAnchorPosY(-50, 0.75f, false).AsyncWaitForCompletion();

        voletNoirHaut.GetComponent<RectTransform>().DOAnchorPosY(65, 1f, false);
        await voletNoirBas.GetComponent<RectTransform>().DOAnchorPosY(-65, 1f, false).AsyncWaitForCompletion();

        voletNoirHaut.GetComponent<RectTransform>().DOAnchorPosY(50, 2f, false);
        await voletNoirBas.GetComponent<RectTransform>().DOAnchorPosY(-50, 2f, false).AsyncWaitForCompletion();

        SoundFXManager.Instance.PlaySoundFXClip(restSFXTrack, this.transform);
        await choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 2.5f).AsyncWaitForCompletion();

        voletNoirHaut.GetComponent<RectTransform>().DOAnchorPosY(121, 2f, false);
        await voletNoirBas.GetComponent<RectTransform>().DOAnchorPosY(-121, 2f, false).AsyncWaitForCompletion();

        // APPLIQUER L'EFFET DU REPOS ICI, RECUPERER DES POINTS DE VIES
        StartWritingTextEnd();
    }

    // Animation lorsque l'on choisit d'ouvrir le coffre
    private async void StartChestAnimation() {
        await choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 0.5f).AsyncWaitForCompletion();
        textePopUp.text = "";

        SoundFXManager.Instance.PlaySoundFXClip(drumRollSFXTrack, this.transform);
        await sujetPopUp.GetComponent<RectTransform>().DOShakeAnchorPos(4.0f, new Vector3(10, 0, 0), 15, 0, false, false).AsyncWaitForCompletion();
        sujetPopUp.GetComponent<Image>().sprite = openChest;

        SoundFXManager.Instance.PlaySoundFXClip(fanfareSFXTrack, this.transform);
        objetCoffre.GetComponent<Image>().DOFade(1f, 3f);
        await objetCoffre.GetComponent<RectTransform>().DOAnchorPosY(50, 3f, false).AsyncWaitForCompletion();

        // AJOUTER L'OBJET DU COFFRE DANS L'INVENTAIRE DU JOUEUR
        StartWritingTextEnd();
    }

    private void StartWritingTextEnd() {
        StartCoroutine(TypeTextEnd());
    }

    IEnumerator TypeTextEnd () {
        int compteur = 0;
        foreach (char letter in textValue.ToCharArray()) {
            textePopUp.text += letter;
            if((compteur%3) == 0) 
                SoundFXManager.Instance.PlaySoundFXClip(textSFXTrack, this.transform);
            yield return new WaitForSeconds (0.02f);
            compteur++;

            if (textePopUp.text == textValue) {
                OpenLeaveWindow();
            }
        }
    }

    private async void OpenLeaveWindow() {
        await leaveWindow.transform.DOScale(new Vector3(1, 1 ,1), 0.5f).AsyncWaitForCompletion();
        ControlsManager.Instance.UpdateState(303);
    }

}