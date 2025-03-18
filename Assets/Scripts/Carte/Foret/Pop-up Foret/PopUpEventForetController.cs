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
    public GameObject zoneTouche;
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

    // Gameobject & TextMesh pour la partie magasin
    public GameObject fenetreMagasinGauche;
    public GameObject marchandItemsConteneur;
    public GameObject fenetreMagasinDroite;
    public GameObject joueurItemsConteneur;
    public GameObject fenetreMagasinInfoItem;
    public GameObject itemPrefab;
    public Sprite cadreItem;
    public Sprite cadreItemSelected;
    public TextMeshProUGUI nomObjetShop;
    public GameObject rarityObjetShop;
    public TextMeshProUGUI atkObjetShop;
    public TextMeshProUGUI defObjetShop;
    public TextMeshProUGUI vitObjetShop;
    public TextMeshProUGUI chnObjetShop;
    public TextMeshProUGUI descObjetShop;

    private int marchandNbItem = 0;
    private int joueurNbItem  = 0;
    private int itemMarchandPosition = 0;
    private int itemJoueurPosition = 0;

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
    public GameObject isEquippedEchange;

    // La liste des images pour la rareté
    public List<Sprite> raritySpriteList = new List<Sprite>();

    // Gameobject pour l'animation de repos
    public GameObject voletNoirHaut;
    public GameObject voletNoirBas;

    // Gameobject pour l'animation de l'evenement 3
    public GameObject monstreGauche;
    public GameObject monstreDroite;

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
    public AudioClip monsterAmbushSFXTrack;
    public AudioClip monsterDeathSFXTrack;
    public AudioClip wolfDeathSFXTrack;
    public AudioClip buyingSellingSFXTrack;
    public AudioClip cantBuySellSFXTrack;

    // La liste des images pour la pop-up
    public List<Sprite> spriteList = new List<Sprite>();
    public Sprite openChest;
    public Sprite openSuspiciousChest;
    public Sprite openTrapChest;
    public Sprite whiteWolf;

    // Variable pour gerer les differents elements de la pop-up
    private int randomNumberEvent;
    private string textValue;
    private int eventType;
    private int choiceNumber = 1;
    private List<ItemData> allPlayerItems = new List<ItemData>();
    private int tradeItemIndex = 0;
    private int tradeEquipCpt = 0;

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

        if(ControlsManager.Instance.controlsState == ControlsState.CarteShopWindow) {
            ChangeShopItem();
            ConfirmShopItem();
            CloseShopItem();
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

    private void ChangeShopItem() {
        int val = 0;
        GameObject encartItem;
        ItemInfoShopController it;

        if (ControlsManager.Instance.DeplacerPressed) {
            val = (int) ControlsManager.Instance.DeplacerValue.x;
            switch (val) {
                case -1:
                    if(itemMarchandPosition == 0) {
                        if(marchandNbItem > 0) {
                            if(itemJoueurPosition != 0) {
                                encartItem = GameObject.Find("Joueur Item "+ itemJoueurPosition);
                                encartItem.GetComponent<Image>().sprite = cadreItem;
                                itemJoueurPosition = 0;
                            }

                            itemMarchandPosition = 1;
                            encartItem = GameObject.Find("Marchand Item "+ itemMarchandPosition);
                            encartItem.GetComponent<Image>().sprite = cadreItemSelected;
                            it = (ItemInfoShopController) encartItem.GetComponent(typeof(ItemInfoShopController));

                            SoundFXManager.Instance.PlaySoundFXClip(selectChoiceSFXTrack, this.transform);
                            
                            if(it.GetSellableInfo()) {
                                AfficherInfosItemShop(it.GetItemInfo());
                            } else {
                                fenetreMagasinInfoItem.transform.DOScale(new Vector3(0, 0 ,0), 0.25f);
                            }
                        }
                    }
                    break;
                case 1:
                    if(itemJoueurPosition == 0) {
                        if(joueurNbItem > 0) {
                            if(itemMarchandPosition != 0) {
                                encartItem = GameObject.Find("Marchand Item "+ itemMarchandPosition);
                                encartItem.GetComponent<Image>().sprite = cadreItem;
                                itemMarchandPosition = 0;
                            }

                            itemJoueurPosition = 1;
                            encartItem = GameObject.Find("Joueur Item "+ itemJoueurPosition);
                            encartItem.GetComponent<Image>().sprite = cadreItemSelected;
                            it = (ItemInfoShopController) encartItem.GetComponent(typeof(ItemInfoShopController));

                            SoundFXManager.Instance.PlaySoundFXClip(selectChoiceSFXTrack, this.transform);

                            if(it.GetSellableInfo()) {
                                AfficherInfosItemShop(it.GetItemInfo());
                            } else {
                                fenetreMagasinInfoItem.transform.DOScale(new Vector3(0, 0 ,0), 0.25f);
                            }
                        }
                    }
                    break;
            }

            val = (int) ControlsManager.Instance.DeplacerValue.y;
            switch (val) {
                case -1:
                    if(itemMarchandPosition != 0 && itemMarchandPosition != marchandNbItem) {
                        encartItem = GameObject.Find("Marchand Item "+ itemMarchandPosition);
                        encartItem.GetComponent<Image>().sprite = cadreItem;

                        itemMarchandPosition++;
                        encartItem = GameObject.Find("Marchand Item "+ itemMarchandPosition);
                        encartItem.GetComponent<Image>().sprite = cadreItemSelected;
                        it = (ItemInfoShopController) encartItem.GetComponent(typeof(ItemInfoShopController));

                        SoundFXManager.Instance.PlaySoundFXClip(selectChoiceSFXTrack, this.transform);

                        if(it.GetSellableInfo()) {
                            AfficherInfosItemShop(it.GetItemInfo());
                        } else {
                            fenetreMagasinInfoItem.transform.DOScale(new Vector3(0, 0 ,0), 0.25f);
                        }
                    }

                    if(itemJoueurPosition != 0 && itemJoueurPosition != joueurNbItem) {
                        encartItem = GameObject.Find("Joueur Item "+ itemJoueurPosition);
                        encartItem.GetComponent<Image>().sprite = cadreItem;

                        itemJoueurPosition++;
                        encartItem = GameObject.Find("Joueur Item "+ itemJoueurPosition);
                        encartItem.GetComponent<Image>().sprite = cadreItemSelected;
                        it = (ItemInfoShopController) encartItem.GetComponent(typeof(ItemInfoShopController));

                        SoundFXManager.Instance.PlaySoundFXClip(selectChoiceSFXTrack, this.transform);

                        if(it.GetSellableInfo()) {
                            AfficherInfosItemShop(it.GetItemInfo());
                        } else {
                            fenetreMagasinInfoItem.transform.DOScale(new Vector3(0, 0 ,0), 0.25f);
                        }
                    }
                    break;
                case 1:
                    if(itemMarchandPosition != 0 && itemMarchandPosition != 1) {
                        encartItem = GameObject.Find("Marchand Item "+ itemMarchandPosition);
                        encartItem.GetComponent<Image>().sprite = cadreItem;

                        itemMarchandPosition--;
                        encartItem = GameObject.Find("Marchand Item "+ itemMarchandPosition);
                        encartItem.GetComponent<Image>().sprite = cadreItemSelected;
                        it = (ItemInfoShopController) encartItem.GetComponent(typeof(ItemInfoShopController));

                        SoundFXManager.Instance.PlaySoundFXClip(selectChoiceSFXTrack, this.transform);

                        if(it.GetSellableInfo()) {
                            AfficherInfosItemShop(it.GetItemInfo());
                        } else {
                            fenetreMagasinInfoItem.transform.DOScale(new Vector3(0, 0 ,0), 0.25f);
                        }
                    }

                    if(itemJoueurPosition != 0 && itemJoueurPosition != 1) {
                        encartItem = GameObject.Find("Joueur Item "+ itemJoueurPosition);
                        encartItem.GetComponent<Image>().sprite = cadreItem;

                        itemJoueurPosition--;
                        encartItem = GameObject.Find("Joueur Item "+ itemJoueurPosition);
                        encartItem.GetComponent<Image>().sprite = cadreItemSelected;
                        it = (ItemInfoShopController) encartItem.GetComponent(typeof(ItemInfoShopController));

                        SoundFXManager.Instance.PlaySoundFXClip(selectChoiceSFXTrack, this.transform);

                        if(it.GetSellableInfo()) {
                            AfficherInfosItemShop(it.GetItemInfo());
                        } else {
                            fenetreMagasinInfoItem.transform.DOScale(new Vector3(0, 0 ,0), 0.25f);
                        }
                    }
                    break;
            }
        }
    }

    private void AfficherInfosItemShop(ItemData i) {
        fenetreMagasinInfoItem.transform.DOScale(new Vector3(1, 1 ,1), 0.25f);

        nomObjetShop.text = i.GetName().GetLocalizedString();
        atkObjetShop.text = i.GetAttack().ToString();
        defObjetShop.text = i.GetDefense().ToString();
        vitObjetShop.text = i.GetSpeed().ToString();
        chnObjetShop.text = i.GetLuck().ToString();
        descObjetShop.text = i.GetDescription().GetLocalizedString();
  
        switch (i.GetRarity()) {
            case 1:
                rarityObjetShop.GetComponent<Image>().sprite = raritySpriteList[0];
                break;
            case 2:
                rarityObjetShop.GetComponent<Image>().sprite = raritySpriteList[1];
                break;
            case 3:
                rarityObjetShop.GetComponent<Image>().sprite = raritySpriteList[2];
                break;
        }
    }

    private void ConfirmShopItem() {
        GameObject encartItem;
        ItemInfoShopController it;

        GameObject itemChildObjet;
        ItemInfoShopController itemInfo;

        if (ControlsManager.Instance.ValiderPressed) {
            if(itemMarchandPosition != 0) {
                encartItem = GameObject.Find("Marchand Item "+ itemMarchandPosition);
                it = (ItemInfoShopController) encartItem.GetComponent(typeof(ItemInfoShopController));

                if(it.GetSellableInfo() && (GameManager.Instance.GetRunPlayerCoins() - it.GetItemInfo().GetPrice()) >= 0) {
                    encartItem.GetComponent<Image>().sprite = cadreItem;
                    itemMarchandPosition = 0;

                    InventoryController.Instance.AddItem(it.GetItemInfo());

                    joueurNbItem++;
                    itemChildObjet = Instantiate(itemPrefab, joueurItemsConteneur.transform);
                    itemChildObjet.name = "Joueur Item "+joueurNbItem;
                    itemChildObjet.transform.GetChild(0).GetComponent<Image>().sprite = it.GetItemInfo().GetSprite();
                    itemChildObjet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (it.GetItemInfo().GetPrice()*0.2).ToString();
                    itemInfo = (ItemInfoShopController) itemChildObjet.GetComponent(typeof(ItemInfoShopController));
                    itemInfo.SetItemInfo(it.GetItemInfo());

                    GameManager.Instance.AddCoinsToRunPlayerCoins(it.GetItemInfo().GetPrice() * -1);
                    it.SetSellableInfo(false);
                    SoundFXManager.Instance.PlaySoundFXClip(buyingSellingSFXTrack, this.transform);
                } else {
                    SoundFXManager.Instance.PlaySoundFXClip(cantBuySellSFXTrack, this.transform);
                }
                
                fenetreMagasinInfoItem.transform.DOScale(new Vector3(0, 0 ,0), 0.25f);
            }

            if(itemJoueurPosition != 0) {
                encartItem = GameObject.Find("Joueur Item "+ itemJoueurPosition);
                it = (ItemInfoShopController) encartItem.GetComponent(typeof(ItemInfoShopController));

                if(it.GetSellableInfo()) {
                    encartItem.GetComponent<Image>().sprite = cadreItem;
                    itemJoueurPosition = 0;

                    if(it.GetEquip()) {
                        EquipmentController.Instance.Unequip(it.GetItemInfo());
                    }

                    InventoryController.Instance.RemoveItem(it.GetItemInfo());

                    GameManager.Instance.AddCoinsToRunPlayerCoins((int) (it.GetItemInfo().GetPrice() * 0.2f));
                    it.SetSellableInfo(false);
                    SoundFXManager.Instance.PlaySoundFXClip(buyingSellingSFXTrack, this.transform);
                } else {
                    SoundFXManager.Instance.PlaySoundFXClip(cantBuySellSFXTrack, this.transform);
                }

                fenetreMagasinInfoItem.transform.DOScale(new Vector3(0, 0 ,0), 0.25f);
            }
        }
    }

    private void CloseShopItem() {
        if (ControlsManager.Instance.FermerPressed) {
            ControlsManager.Instance.UpdateState(302);
            CloseShopMenuAnimation();
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
        randomNumberEvent = Random.Range(0, 3);
        switch (randomNumberEvent) {
            case 0:
                sujetPopUp.GetComponent<Image>().sprite = spriteList[4];
                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RandomEvent1Intro");
                texteChoice1.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionAttack");
                texteChoice2.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionLeave");
                break;
            case 1:
                sujetPopUp.GetComponent<Image>().sprite = spriteList[5];
                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RandomEvent2Intro");
                texteChoice1.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionOpen");
                texteChoice2.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionLeave");
                break;
            case 2:
                sujetPopUp.GetComponent<Image>().sprite = whiteWolf;
                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RandomEvent3Intro");
                texteChoice1.text = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ActionObserve");
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
        monstreGauche.GetComponent<RectTransform>().DOAnchorPosX(-150, 1f, false);
        monstreDroite.GetComponent<RectTransform>().DOAnchorPosX(150, 1f, false);
        monstreGauche.GetComponent<Image>().DOFade(0f, 0.5f);
        monstreDroite.GetComponent<Image>().DOFade(0f, 0.5f);
        cursor1.SetActive(true);
        cursor2.SetActive(false);
        isEquippedEchange.SetActive(false);

        // On nettoie aussi les listes marchands au cas ou on a un autre marchand après un autre event
        foreach (Transform child in marchandItemsConteneur.transform) {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in joueurItemsConteneur.transform) {
            GameObject.Destroy(child.gameObject);
        }
        marchandNbItem = 0;
        joueurNbItem  = 0;
        itemMarchandPosition = 0;
        itemJoueurPosition = 0;

        this.gameObject.SetActive(false);
        ControlsManager.Instance.UpdateState(300);
    }

    private void ContinuerPopUpEvent() {
        // On met les controles du joueur en pause
        ControlsManager.Instance.UpdateState(302);

        int randomItemNumber;
        int randomRarityRate = Random.Range(0, 101);
        ItemData randomItem;

        GameObject popUpControls = GameObject.Find("Zone des touches");
        PopUpControlsController p = (PopUpControlsController) popUpControls.GetComponent(typeof(PopUpControlsController));

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
                    case 2: // Evenement aléatoire n°3 (Loup Blanc)
                        if(randomSuccessEvent > 49) {
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

                            textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RandomEvent3GoodEnd");
                            StartEvent3SuccessAnimation();
                        } else {
                            textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RandomEvent3BadEnd");
                            StartEvent3FailureAnimation();
                        }
                        break;
                }
                break;
            case 3: // Feu de camp
                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "RestEventEnd");
                StartRestAnimation();
                break;
            case 4: // Magasin
                GameObject itemChildObjet;
                ItemInfoShopController itemInfo;
                int j = 1;

                // On genere 3 items pour le marchand
                for (int i = 1; i < 4; i++) {
                    randomRarityRate = Random.Range(0, 101);

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

                    itemChildObjet = Instantiate(itemPrefab, marchandItemsConteneur.transform);
                    itemChildObjet.name = "Marchand Item "+i;
                    itemChildObjet.transform.GetChild(0).GetComponent<Image>().sprite = randomItem.GetSprite();
                    itemChildObjet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = randomItem.GetPrice().ToString();

                    itemInfo = (ItemInfoShopController) itemChildObjet.GetComponent(typeof(ItemInfoShopController));
                    itemInfo.SetItemInfo(randomItem);

                    marchandNbItem++;
                }

                // On genere les encarts des items du joueur
                if(EquipmentController.Instance.GetCasque() != null) {
                    itemChildObjet = Instantiate(itemPrefab, joueurItemsConteneur.transform);
                    itemChildObjet.name = "Joueur Item "+j;
                    itemChildObjet.transform.GetChild(0).GetComponent<Image>().sprite = EquipmentController.Instance.GetCasque().GetSprite();
                    itemChildObjet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (EquipmentController.Instance.GetCasque().GetPrice()*0.2).ToString();
                    itemInfo = (ItemInfoShopController) itemChildObjet.GetComponent(typeof(ItemInfoShopController));
                    itemInfo.SetItemInfo(EquipmentController.Instance.GetCasque());
                    itemInfo.SetEquip(true);
                    joueurNbItem++;
                    j++;
                }
                if(EquipmentController.Instance.GetTorse() != null) {
                    itemChildObjet = Instantiate(itemPrefab, joueurItemsConteneur.transform);
                    itemChildObjet.name = "Joueur Item "+j;
                    itemChildObjet.transform.GetChild(0).GetComponent<Image>().sprite = EquipmentController.Instance.GetTorse().GetSprite();
                    itemChildObjet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (EquipmentController.Instance.GetTorse().GetPrice()*0.2).ToString();
                    itemInfo = (ItemInfoShopController) itemChildObjet.GetComponent(typeof(ItemInfoShopController));
                    itemInfo.SetItemInfo(EquipmentController.Instance.GetTorse());
                    itemInfo.SetEquip(true);
                    joueurNbItem++;
                    j++;
                }
                if(EquipmentController.Instance.GetBottes() != null){
                    itemChildObjet = Instantiate(itemPrefab, joueurItemsConteneur.transform);
                    itemChildObjet.name = "Joueur Item "+j;
                    itemChildObjet.transform.GetChild(0).GetComponent<Image>().sprite = EquipmentController.Instance.GetBottes().GetSprite();
                    itemChildObjet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (EquipmentController.Instance.GetBottes().GetPrice()*0.2).ToString();
                    itemInfo = (ItemInfoShopController) itemChildObjet.GetComponent(typeof(ItemInfoShopController));
                    itemInfo.SetItemInfo(EquipmentController.Instance.GetBottes());
                    itemInfo.SetEquip(true);
                    joueurNbItem++;
                    j++;
                }
                if(EquipmentController.Instance.GetArme() != null){
                    itemChildObjet = Instantiate(itemPrefab, joueurItemsConteneur.transform);
                    itemChildObjet.name = "Joueur Item "+j;
                    itemChildObjet.transform.GetChild(0).GetComponent<Image>().sprite = EquipmentController.Instance.GetArme().GetSprite();
                    itemChildObjet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (EquipmentController.Instance.GetArme().GetPrice()*0.2).ToString();
                    itemInfo = (ItemInfoShopController) itemChildObjet.GetComponent(typeof(ItemInfoShopController));
                    itemInfo.SetItemInfo(EquipmentController.Instance.GetArme());
                    itemInfo.SetEquip(true);
                    joueurNbItem++;
                    j++;
                }
                if(EquipmentController.Instance.GetAccessoireJ() != null){
                    itemChildObjet = Instantiate(itemPrefab, joueurItemsConteneur.transform);
                    itemChildObjet.name = "Joueur Item "+j;
                    itemChildObjet.transform.GetChild(0).GetComponent<Image>().sprite = EquipmentController.Instance.GetAccessoireJ().GetSprite();
                    itemChildObjet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (EquipmentController.Instance.GetAccessoireJ().GetPrice()*0.2).ToString();
                    itemInfo = (ItemInfoShopController) itemChildObjet.GetComponent(typeof(ItemInfoShopController));
                    itemInfo.SetItemInfo(EquipmentController.Instance.GetAccessoireJ());
                    itemInfo.SetEquip(true);
                    joueurNbItem++;
                    j++;
                }
                if(EquipmentController.Instance.GetAccessoireK() != null){
                    itemChildObjet = Instantiate(itemPrefab, joueurItemsConteneur.transform);
                    itemChildObjet.name = "Joueur Item "+j;
                    itemChildObjet.transform.GetChild(0).GetComponent<Image>().sprite = EquipmentController.Instance.GetAccessoireK().GetSprite();
                    itemChildObjet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (EquipmentController.Instance.GetAccessoireK().GetPrice()*0.2).ToString();
                    itemInfo = (ItemInfoShopController) itemChildObjet.GetComponent(typeof(ItemInfoShopController));
                    itemInfo.SetItemInfo(EquipmentController.Instance.GetAccessoireK());
                    itemInfo.SetEquip(true);
                    joueurNbItem++;
                    j++;
                }

                foreach (ItemData item in InventoryController.Instance.GetItemList()) {
                    itemChildObjet = Instantiate(itemPrefab, joueurItemsConteneur.transform);
                    itemChildObjet.name = "Joueur Item "+j;
                    itemChildObjet.transform.GetChild(0).GetComponent<Image>().sprite = item.GetSprite();
                    itemChildObjet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (item.GetPrice()*0.2).ToString();
                    itemInfo = (ItemInfoShopController) itemChildObjet.GetComponent(typeof(ItemInfoShopController));
                    itemInfo.SetItemInfo(item);
                    joueurNbItem++;
                    j++;
                }

                textValue = LocalizationSettings.StringDatabase.GetLocalizedString("CarteEventTable", "ShopEventEnd");
                p.SetShopControls();
                OpenShopMenuAnimation();
                break;
            case 5: // Echange
                allPlayerItems = new List<ItemData>();

                int itemCount = 0;
                tradeEquipCpt = 0;

                if(EquipmentController.Instance.GetCasque() != null) {
                    itemCount++;
                    tradeEquipCpt++;
                    allPlayerItems.Add(EquipmentController.Instance.GetCasque());
                }
                if(EquipmentController.Instance.GetTorse() != null) {
                    itemCount++;
                    tradeEquipCpt++;
                    allPlayerItems.Add(EquipmentController.Instance.GetTorse());
                }
                if(EquipmentController.Instance.GetBottes() != null){
                    itemCount++;
                    tradeEquipCpt++;
                    allPlayerItems.Add(EquipmentController.Instance.GetBottes());
                }
                if(EquipmentController.Instance.GetArme() != null){
                    itemCount++;
                    tradeEquipCpt++;
                    allPlayerItems.Add(EquipmentController.Instance.GetArme());
                }
                if(EquipmentController.Instance.GetAccessoireJ() != null){
                    itemCount++;
                    tradeEquipCpt++;
                    allPlayerItems.Add(EquipmentController.Instance.GetAccessoireJ());
                }
                if(EquipmentController.Instance.GetAccessoireK() != null){
                    itemCount++;
                    tradeEquipCpt++;
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
                    p.SetTradeControls();
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

        if(index < tradeEquipCpt) {
            isEquippedEchange.SetActive(true);
        } else {
            isEquippedEchange.SetActive(false);
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

    // Animation de l'event aléatoire 3 si on reussit
    private async void StartEvent3SuccessAnimation() {
        await choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 0.5f).AsyncWaitForCompletion();
        textePopUp.text = "";

        SoundFXManager.Instance.PlaySoundFXClip(monsterAmbushSFXTrack, this.transform);
        monstreDroite.GetComponent<Image>().DOFade(1f, 1f);
        await monstreDroite.GetComponent<RectTransform>().DOAnchorPosX(50, 1f, false).AsyncWaitForCompletion();

        SoundFXManager.Instance.PlaySoundFXClip(monsterAmbushSFXTrack, this.transform);
        monstreGauche.GetComponent<Image>().DOFade(1f, 1f);
        await monstreGauche.GetComponent<RectTransform>().DOAnchorPosX(-50, 1f, false).AsyncWaitForCompletion();

        SoundFXManager.Instance.PlaySoundFXClip(monsterAmbushSFXTrack, this.transform);
        monstreDroite.GetComponent<RectTransform>().DOJumpAnchorPos(new Vector2(50, 10), 15f, 3, 1.0f, false);
        await monstreGauche.GetComponent<RectTransform>().DOJumpAnchorPos(new Vector2(-50, 10), 15f, 3, 1.0f, false).AsyncWaitForCompletion();

        SoundFXManager.Instance.PlaySoundFXClip(dodgeSFXTrack, this.transform);
        monstreDroite.GetComponent<RectTransform>().DOAnchorPosX(70, 0.35f, false);
        await sujetPopUp.GetComponent<RectTransform>().DOAnchorPosX(30, 0.35f, false).AsyncWaitForCompletion();

        SoundFXManager.Instance.PlaySoundFXClip(dodgeSFXTrack, this.transform);
        monstreGauche.GetComponent<RectTransform>().DOAnchorPosX(-70, 0.35f, false);
        await sujetPopUp.GetComponent<RectTransform>().DOAnchorPosX(-30, 0.35f, false).AsyncWaitForCompletion();

        await sujetPopUp.GetComponent<RectTransform>().DOAnchorPosX(0, 0.35f, false).AsyncWaitForCompletion();

        SoundFXManager.Instance.PlaySoundFXClip(monsterAmbushSFXTrack, this.transform);
        monstreDroite.GetComponent<RectTransform>().DOAnchorPosX(13, 0.35f, false);
        await monstreGauche.GetComponent<RectTransform>().DOAnchorPosX(-13, 0.35f, false).AsyncWaitForCompletion();

        SoundFXManager.Instance.PlaySoundFXClip(wolfDeathSFXTrack, this.transform);
        monstreDroite.GetComponent<RectTransform>().DOJumpAnchorPos(new Vector2(13, 10), 5f, 6, 2.0f, false);
        monstreGauche.GetComponent<RectTransform>().DOJumpAnchorPos(new Vector2(-13, 10), 5f, 6, 2.0f, false);
        await sujetPopUp.GetComponent<Image>().DOFade(0f, 2f).AsyncWaitForCompletion();

        monstreGauche.GetComponent<RectTransform>().DOAnchorPosX(150, 1f, false);
        await monstreDroite.GetComponent<RectTransform>().DOAnchorPosX(-150, 1f, false).AsyncWaitForCompletion();

        await choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 1.0f).AsyncWaitForCompletion();

        SoundFXManager.Instance.PlaySoundFXClip(fanfareSFXTrack, this.transform);
        objetCoffre.GetComponent<Image>().DOFade(1f, 2f);
        await objetCoffre.GetComponent<RectTransform>().DOAnchorPosY(50, 2f, false).AsyncWaitForCompletion();

        StartWritingTextEnd();
    }

    // Animation de l'event aléatoire 3 si on échoue
    private async void StartEvent3FailureAnimation() {
        await choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 0.5f).AsyncWaitForCompletion();
        textePopUp.text = "";

        SoundFXManager.Instance.PlaySoundFXClip(monsterAmbushSFXTrack, this.transform);
        monstreDroite.GetComponent<Image>().DOFade(1f, 1f);
        await monstreDroite.GetComponent<RectTransform>().DOAnchorPosX(50, 1f, false).AsyncWaitForCompletion();

        SoundFXManager.Instance.PlaySoundFXClip(monsterAmbushSFXTrack, this.transform);
        monstreGauche.GetComponent<Image>().DOFade(1f, 1f);
        await monstreGauche.GetComponent<RectTransform>().DOAnchorPosX(-50, 1f, false).AsyncWaitForCompletion();

        SoundFXManager.Instance.PlaySoundFXClip(monsterAmbushSFXTrack, this.transform);
        monstreDroite.GetComponent<RectTransform>().DOJumpAnchorPos(new Vector2(50, 10), 15f, 3, 1.0f, false);
        await monstreGauche.GetComponent<RectTransform>().DOJumpAnchorPos(new Vector2(-50, 15), 10f, 3, 1.0f, false).AsyncWaitForCompletion();

        SoundFXManager.Instance.PlaySoundFXClip(punchSFXTrack, this.transform);
        await sujetPopUp.GetComponent<RectTransform>().DOAnchorPosX(30, 0.35f, false).AsyncWaitForCompletion();

        SoundFXManager.Instance.PlaySoundFXClip(punchSFXTrack, this.transform);
        await sujetPopUp.GetComponent<RectTransform>().DOAnchorPosX(-30, 0.35f, false).AsyncWaitForCompletion();

        await sujetPopUp.GetComponent<RectTransform>().DOAnchorPosX(0, 0.35f, false).AsyncWaitForCompletion();

        SoundFXManager.Instance.PlaySoundFXClip(monsterDeathSFXTrack, this.transform);
        await monstreDroite.GetComponent<Image>().DOFade(0f, 0.5f).AsyncWaitForCompletion();

        SoundFXManager.Instance.PlaySoundFXClip(monsterDeathSFXTrack, this.transform);
        await monstreGauche.GetComponent<Image>().DOFade(0f, 0.5f).AsyncWaitForCompletion();

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

    // Animation qui ouvre le menu magasin
    private async void OpenShopMenuAnimation() {
        await choiceWindow.transform.DOScale(new Vector3(0, 0 ,0), 0.5f).AsyncWaitForCompletion();
        textePopUp.text = "";
        zoneTexteBas.GetComponent<RectTransform>().DOAnchorPosY(-88, 0.5f, false);

        fenetreMagasinGauche.GetComponent<RectTransform>().DOAnchorPosX(-88, 1f, false);
        await fenetreMagasinDroite.GetComponent<RectTransform>().DOAnchorPosX(88, 1f, false).AsyncWaitForCompletion();

        await zoneTouche.GetComponent<RectTransform>().DOAnchorPosY(-62.5f, 1f, false).AsyncWaitForCompletion();

        ControlsManager.Instance.UpdateState(305);
    }

    // Animation qui ferme le menu magasin
    private async void CloseShopMenuAnimation() {
        zoneTouche.GetComponent<RectTransform>().DOAnchorPosY(-79.5f, 0.5f, false);

        fenetreMagasinInfoItem.transform.DOScale(new Vector3(0, 0 ,0), 1f);
        fenetreMagasinGauche.GetComponent<RectTransform>().DOAnchorPosX(-166, 1f, false);
        await fenetreMagasinDroite.GetComponent<RectTransform>().DOAnchorPosX(166, 1f, false).AsyncWaitForCompletion();

        await zoneTexteBas.GetComponent<RectTransform>().DOAnchorPosY(-55, 0.5f, false).AsyncWaitForCompletion();
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
        zoneTexteBas.GetComponent<RectTransform>().DOAnchorPosY(-88, 0.5f, false);

        fenetreObjetEchange.GetComponent<RectTransform>().DOAnchorPosY(10, 1f, false);
        await fenetreInfoEchange.GetComponent<RectTransform>().DOAnchorPosX(67, 1f, false).AsyncWaitForCompletion();

        await zoneTouche.GetComponent<RectTransform>().DOAnchorPosY(-62.5f, 1f, false).AsyncWaitForCompletion();

        ControlsManager.Instance.UpdateState(304);
    }

    // Animation lorsque l'on a au moins un item à échanger
    private async void StartTradeItemAnimation() {
        zoneTouche.GetComponent<RectTransform>().DOAnchorPosY(-79.5f, 0.5f, false);
        flecheGaucheObjetEchange.GetComponent<Image>().DOFade(0f, 1f);
        flecheDroiteObjetEchange.GetComponent<Image>().DOFade(0f, 1f);
        isEquippedEchange.SetActive(false);
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

        await zoneTexteBas.GetComponent<RectTransform>().DOAnchorPosY(-55, 0.5f, false).AsyncWaitForCompletion();

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