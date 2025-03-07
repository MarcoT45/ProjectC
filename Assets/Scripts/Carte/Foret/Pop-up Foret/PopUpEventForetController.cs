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

    // Gameobject & TextMesh pour la partie échange
    public GameObject fenetreInfoEchange;
    public TextMeshProUGUI nomObjetEchange;
    public GameObject rarityObjetEchange;
    public TextMeshProUGUI atkObjetEchange;
    public TextMeshProUGUI defObjetEchange;
    public TextMeshProUGUI vitObjetEchange;
    public TextMeshProUGUI chnObjetEchange;
    public TextMeshProUGUI descObjetEchange;
    public GameObject fenetreObjetEchange;
    public GameObject spriteObjetEchange;
    public GameObject flecheGaucheObjetEchange;
    public GameObject flecheDroiteObjetEchange;

    // La liste des images pour la rareté
    public List<Sprite> raritySpriteList = new List<Sprite>();

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
    public AudioClip splashSFXTrack;
    public AudioClip wellShakingSFXTrack;
    public AudioClip mimicBiteSFXTrack;

    // La liste des images pour la pop-up
    public List<Sprite> spriteList = new List<Sprite>();
    public Sprite openChest;
    public Sprite openSuspiciousChest;
    public Sprite openTrapChest;

    // Variable pour gerer les differents elements de la pop-up
    private int randomNumberEvent;
    private string textValue;
    private int eventType;
    private int choiceNumber = 1;
    private List<ItemData> allPlayerItems = new List<ItemData>();
    private int tradeItemIndex = 0;

    ////////////////////////////////// Partie pour les controles ////////////////////////////////////////

    private void Update() {
        if(ControlsManager.Instance.controlsState == ControlsState.CarteChoiceWindow) {
            DeplacerChoix();
            ValiderChoix();
        }

        if(ControlsManager.Instance.controlsState == ControlsState.CarteLeaveWindow) {
            ValiderPartir();
        }

        if(ControlsManager.Instance.controlsState == ControlsState.CarteTradeWindow) {
            ChangeTradeItem();
            ConfirmTradeItem();
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

    private void ChangeTradeItem() {
        if (ControlsManager.Instance.DeplacerPressed) {
            int val = (int) ControlsManager.Instance.DeplacerValue.x;
            switch (val) {
                case -1:
                    if (tradeItemIndex == 0) {
                        tradeItemIndex = allPlayerItems.Count - 1;
                    } else {
                        tradeItemIndex = tradeItemIndex - 1;
                    }
                    break;
                case 1:
                    if (tradeItemIndex == allPlayerItems.Count - 1) {
                        tradeItemIndex = 0;
                    } else {
                        tradeItemIndex = tradeItemIndex + 1;
                    }
                    break;
            }

            if(allPlayerItems.Count > 1) {
                SoundFXManager.Instance.PlaySoundFXClip(selectChoiceSFXTrack, this.transform);
            }

            ChargerInfoTradeItem(tradeItemIndex);
        }
    }

    public void MousePreviousItem() {
        if (tradeItemIndex == 0) {
            tradeItemIndex = allPlayerItems.Count - 1;
        } else {
            tradeItemIndex = tradeItemIndex - 1;
        }

        if(allPlayerItems.Count > 1) {
            SoundFXManager.Instance.PlaySoundFXClip(selectChoiceSFXTrack, this.transform);
        }

        ChargerInfoTradeItem(tradeItemIndex);
    }

    public void MouseNextItem() {
        if (tradeItemIndex == allPlayerItems.Count - 1) {
            tradeItemIndex = 0;
        } else {
            tradeItemIndex = tradeItemIndex + 1;
        }

        if(allPlayerItems.Count > 1) {
            SoundFXManager.Instance.PlaySoundFXClip(selectChoiceSFXTrack, this.transform);
        }

        ChargerInfoTradeItem(tradeItemIndex);
    }

    private void ConfirmTradeItem() {
        if (ControlsManager.Instance.ValiderPressed) {
            ControlsManager.Instance.UpdateState(302);

            if( EquipmentController.Instance.GetCasque() == allPlayerItems[tradeItemIndex] ||
                EquipmentController.Instance.GetTorse() == allPlayerItems[tradeItemIndex] ||
                EquipmentController.Instance.GetBottes() == allPlayerItems[tradeItemIndex] ||
                EquipmentController.Instance.GetArme() == allPlayerItems[tradeItemIndex] ||
                EquipmentController.Instance.GetAccessoireJ() == allPlayerItems[tradeItemIndex] ||
                EquipmentController.Instance.GetAccessoireK() == allPlayerItems[tradeItemIndex]) {

                EquipmentController.Instance.Unequip(allPlayerItems[tradeItemIndex]);
            }

            InventoryController.Instance.RemoveItem(allPlayerItems[tradeItemIndex]);

            int randomItemNumber;
            int randomRarityRate = Random.Range(0, 101);
            ItemData randomItem;
                
            if(randomRarityRate > 80) {
                randomItemNumber = Random.Range(0, GameManager.Instance.itemsTriRarete[2].Count);
                randomItem = GameManager.Instance.itemsTriRarete[2][randomItemNumber];
            } else if (randomRarityRate > 50) {
                randomItemNumber = Random.Range(0, GameManager.Instance.itemsTriRarete[1].Count);
                randomItem = GameManager.Instance.itemsTriRarete[1][randomItemNumber];
            } else {
                randomItemNumber = Random.Range(0, GameManager.Instance.itemsTriRarete[0].Count);
                randomItem = GameManager.Instance.itemsTriRarete[0][randomItemNumber];
            }

            objetCoffre.GetComponent<Image>().sprite = randomItem.GetSprite();
            InventoryController.Instance.AddItem(randomItem);

            StartTradeItemAnimation();
        }
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
        randomNumberEvent = Random.Range(0, 2); // POUR REGLER SUR QUEL EVENT ON TOMBE
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
                texteChoice1.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionOpen");
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
        flecheGaucheObjetEchange.GetComponent<Image>().DOFade(1f, 0.5f);
        flecheDroiteObjetEchange.GetComponent<Image>().DOFade(1f, 0.5f);
        spriteObjetEchange.GetComponent<Image>().DOFade(1f, 0.5f);
        fenetreObjetEchange.GetComponent<RectTransform>().DOAnchorPosY(50, 1f, false);
        fenetreObjetEchange.GetComponent<RectTransform>().DOAnchorPosX(-61, 1f, false);
        cursor1.SetActive(true);
        cursor2.SetActive(false);
        this.gameObject.SetActive(false);
        ControlsManager.Instance.UpdateState(300);
    }

    private void ContinuerPopUpEvent() {
        // On met les controles du joueur en pause
        ControlsManager.Instance.UpdateState(302);

        int randomItemNumber;
        int randomRarityRate = Random.Range(0, 101);
        ItemData randomItem;

        switch (eventType) {
            case 1: // Evenement aléatoire
                int randomSuccessEvent = Random.Range(0, 101);
                switch (randomNumberEvent) {
                    case 0: // Evenement aléatoire n°1 (Dayo)
                        if(randomSuccessEvent > 39) {
                            textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RandomEvent1GoodEnd");
                            StartEvent1SuccessAnimation();
                        } else {
                            textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RandomEvent1BadEnd");
                            StartEvent1FailureAnimation();
                        }
                        break;
                    case 1: // Evenement aléatoire n°2 (Mimic)
                        if(randomSuccessEvent > 29) {
                            if(randomRarityRate > 70) {
                                randomItemNumber = Random.Range(0, GameManager.Instance.itemsTriRarete[2].Count);
                                randomItem = GameManager.Instance.itemsTriRarete[2][randomItemNumber];
                            } else {
                                randomItemNumber = Random.Range(0, GameManager.Instance.itemsTriRarete[1].Count);
                                randomItem = GameManager.Instance.itemsTriRarete[1][randomItemNumber];
                            }

                            objetCoffre.GetComponent<Image>().sprite = randomItem.GetSprite();

                            InventoryController.Instance.AddItem(randomItem);

                            textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RandomEvent2GoodEnd");
                            StartEvent2SuccessAnimation();
                        } else {
                            textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RandomEvent2BadEnd");
                            StartEvent2FailureAnimation();
                        }
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

                // FAIRE LA PARTIE MAGASIN DE LA POP-UP
                // REFLECHIR A COMMENT CA MARCHE ET AUX CONTROLES
                // NE PAS OUBLIER LE MESSAGE SI ON A PAS ASSEZ D'ARGENT POUR ACHETER

                break;
            case 5: // Echange
                allPlayerItems = new List<ItemData>();

                int itemCount = 0;

                if(EquipmentController.Instance.GetCasque() != null) {
                    itemCount++;
                    allPlayerItems.Add(EquipmentController.Instance.GetCasque());
                }
                if(EquipmentController.Instance.GetTorse() != null) {
                    itemCount++;
                    allPlayerItems.Add(EquipmentController.Instance.GetTorse());
                }
                if(EquipmentController.Instance.GetBottes() != null){
                    itemCount++;
                    allPlayerItems.Add(EquipmentController.Instance.GetBottes());
                }
                if(EquipmentController.Instance.GetArme() != null){
                    itemCount++;
                    allPlayerItems.Add(EquipmentController.Instance.GetArme());
                }
                if(EquipmentController.Instance.GetAccessoireJ() != null){
                    itemCount++;
                    allPlayerItems.Add(EquipmentController.Instance.GetAccessoireJ());
                }
                if(EquipmentController.Instance.GetAccessoireK() != null){
                    itemCount++;
                    allPlayerItems.Add(EquipmentController.Instance.GetAccessoireK());
                }

                itemCount = itemCount + InventoryController.Instance.GetItemList().Count;
                foreach (ItemData item in InventoryController.Instance.GetItemList()) {
                    allPlayerItems.Add(item);
                }

                if(itemCount == 0) {
                    textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "TradeEventNoItemEnd");
                    StartTradeNoItemAnimation();
                } else {
                    textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "TradeEventEnd");
                    tradeItemIndex = 0;
                    ChargerInfoTradeItem(tradeItemIndex);
                    OpenTradeMenuAnimation();
                }
                break;
            case 6: // Coffre
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

                InventoryController.Instance.AddItem(randomItem);

                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ChestEventEnd");
                StartChestAnimation();
                break;
        }
    }

    private void ChargerInfoTradeItem(int index) {
        spriteObjetEchange.GetComponent<Image>().sprite = allPlayerItems[index].GetSprite();
        nomObjetEchange.text = allPlayerItems[index].GetName().GetLocalizedString();
        atkObjetEchange.text = allPlayerItems[index].GetAttack().ToString();
        defObjetEchange.text = allPlayerItems[index].GetDefense().ToString();
        vitObjetEchange.text = allPlayerItems[index].GetSpeed().ToString();
        chnObjetEchange.text = allPlayerItems[index].GetLuck().ToString();
        descObjetEchange.text = allPlayerItems[index].GetDescription().GetLocalizedString();
  
        switch (allPlayerItems[index].GetRarity()) {
            case 1:
                rarityObjetEchange.GetComponent<Image>().sprite = raritySpriteList[0];
                break;
            case 2:
                rarityObjetEchange.GetComponent<Image>().sprite = raritySpriteList[1];
                break;
            case 3:
                rarityObjetEchange.GetComponent<Image>().sprite = raritySpriteList[2];
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

        int randomCoinsQuantity = Random.Range(100, 201);
        GameManager.Instance.AddCoinsToRunPlayerCoins(randomCoinsQuantity);

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

        GameManager.Instance.AddCoinsToRunPlayerCoins( (int) (GameManager.Instance.GetRunPlayerCoins() * 0.2f * -1));

        StartWritingTextEnd();
    }

    // Animation de l'event aléatoire 2 si on reussit
    private async void StartEvent2SuccessAnimation() {
        await choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 0.5f).AsyncWaitForCompletion();
        textePopUp.text = "";

        SoundFXManager.Instance.PlaySoundFXClip(drumRollSFXTrack, this.transform);
        await sujetPopUp.GetComponent<RectTransform>().DOShakeAnchorPos(4.0f, new Vector3(10, 0, 0), 15, 0, false, false).AsyncWaitForCompletion();
        sujetPopUp.GetComponent<Image>().sprite = openSuspiciousChest;

        SoundFXManager.Instance.PlaySoundFXClip(fanfareSFXTrack, this.transform);
        objetCoffre.GetComponent<Image>().DOFade(1f, 3f);
        await objetCoffre.GetComponent<RectTransform>().DOAnchorPosY(50, 3f, false).AsyncWaitForCompletion();

        StartWritingTextEnd();
    }

    // Animation de l'event aléatoire 2 si on échoue
    private async void StartEvent2FailureAnimation() {
        await choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 0.5f).AsyncWaitForCompletion();
        textePopUp.text = "";

        SoundFXManager.Instance.PlaySoundFXClip(drumRollSFXTrack, this.transform);
        await sujetPopUp.GetComponent<RectTransform>().DOShakeAnchorPos(4.0f, new Vector3(10, 0, 0), 15, 0, false, false).AsyncWaitForCompletion();
        sujetPopUp.GetComponent<Image>().sprite = openTrapChest;

        SoundFXManager.Instance.PlaySoundFXClip(mimicBiteSFXTrack, this.transform);
        await conteneur.GetComponent<RectTransform>().DOShakeAnchorPos(1.0f, 5.0f, 5, 10f, false, true).AsyncWaitForCompletion();

        SoundFXManager.Instance.PlaySoundFXClip(mimicBiteSFXTrack, this.transform);
        await conteneur.GetComponent<RectTransform>().DOShakeAnchorPos(1.0f, 5.0f, 5, 10f, false, true).AsyncWaitForCompletion();

        // APPLIQUER L'EFFET DE PERTE DES PV ICI, ENLEVER 2 POINTS DE VIES
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

    // Animation lorsque l'on n'a pas d'item à échanger
    private async void StartTradeNoItemAnimation() {
        await choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 0.5f).AsyncWaitForCompletion();
        textePopUp.text = "";
        StartWritingTextEnd();
    }

    // Animation qui ouvre le menu de l'échange
    private async void OpenTradeMenuAnimation() {
        await choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 0.5f).AsyncWaitForCompletion();
        textePopUp.text = "";

        fenetreObjetEchange.GetComponent<RectTransform>().DOAnchorPosY(10, 1f, false);
        await fenetreInfoEchange.GetComponent<RectTransform>().DOAnchorPosX(67, 1f, false).AsyncWaitForCompletion();

        ControlsManager.Instance.UpdateState(304);
    }

    // Animation lorsque l'on a au moins un item à échanger
    private async void StartTradeItemAnimation() {
        flecheGaucheObjetEchange.GetComponent<Image>().DOFade(0f, 1f);
        flecheDroiteObjetEchange.GetComponent<Image>().DOFade(0f, 1f);
        fenetreObjetEchange.GetComponent<RectTransform>().DOAnchorPosX(0, 1f, false);
        await fenetreInfoEchange.GetComponent<RectTransform>().DOAnchorPosX(190, 1f, false).AsyncWaitForCompletion();

        spriteObjetEchange.GetComponent<Image>().DOFade(0f, 1f);
        await fenetreObjetEchange.GetComponent<RectTransform>().DOAnchorPosY(-10, 1f, false).AsyncWaitForCompletion();
        
        SoundFXManager.Instance.PlaySoundFXClip(splashSFXTrack, this.transform);
        await choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 1.25f).AsyncWaitForCompletion();

        SoundFXManager.Instance.PlaySoundFXClip(wellShakingSFXTrack, this.transform);
        await sujetPopUp.GetComponent<RectTransform>().DOShakeAnchorPos(1.25f, new Vector3(5, 0, 0), 30, 0, false, false).AsyncWaitForCompletion();

        SoundFXManager.Instance.PlaySoundFXClip(fanfareSFXTrack, this.transform);
        objetCoffre.GetComponent<Image>().DOFade(1f, 3f);
        await objetCoffre.GetComponent<RectTransform>().DOAnchorPosY(50, 3f, false).AsyncWaitForCompletion();

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