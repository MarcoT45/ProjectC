using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpForetController : MonoBehaviour {

    // Pop-up partie générale
    public GameObject boutonPartir;
    public TextMeshProUGUI textePopUp;
    public GameObject popUpContent;
    public TextMeshProUGUI texteArgentJoueurBarreDuHaut;
    private GameObject sujet;
    private bool isOpening = false;
    private bool isClosing = false;

    // Pop-up coffre
    public GameObject boutonOuvrir;
    public GameObject coffrePrefab;
    public GameObject coffreOuverturePrefab;
    public GameObject encartObjetTrouve;
    public TextMeshProUGUI texteNomObjetTrouve;
    public GameObject imageObjetTrouve;

    // Pop-up feu de camp
    public GameObject boutonReposer;
    public GameObject feuDeCampPrefab;
    public GameObject feuDeCampEteintPrefab;

    // Pop-up échange
    public GameObject boutonEchanger;
    public GameObject puitsPrefab;
    public GameObject puitsVidePrefab;
    public GameObject encartEchange;

    // Pop-up marchand
    public GameObject marchandPrefab;
    public GameObject encartArgentJoueur;
    public TextMeshProUGUI texteArgentJoueur;
    private ItemData objet1;
    public GameObject encartObjet1;
    public GameObject imageObjet1;
    public TextMeshProUGUI textePrice1;
    private ItemData objet2;
    public GameObject encartObjet2;
    public GameObject imageObjet2;
    public TextMeshProUGUI textePrice2;
    private ItemData objet3;
    public GameObject encartObjet3;
    public GameObject imageObjet3;
    public TextMeshProUGUI textePrice3;

    // FAIRE UN ENCART INFO POUR LES OBJETS DE LA BOUTIQUE (OnHover)!

    // Pop-up évenement
    public GameObject boutonAction;
    private int randomNumberEvent = 0;
    public TextMeshProUGUI boutonActionTexte;
    public GameObject event0Prefab;
    public GameObject event1Prefab;
    public GameObject event2Prefab;

    private void FixedUpdate() {

        if(isOpening) {
            Vector3 targetAngle = new Vector3(0, 0, 0);

            if (Vector3.Distance(popUpContent.transform.eulerAngles, targetAngle) > 0.01f) {
                popUpContent.transform.Rotate(2.5f, 0, 0);
            } else {
                popUpContent.transform.eulerAngles = targetAngle;
                isOpening = false;
            }
        }

        if(isClosing) {
            Vector3 targetAngle = new Vector3(90, 0, 0);

            if (Vector3.Distance(popUpContent.transform.eulerAngles, targetAngle) > 0.01f) {
                popUpContent.transform.Rotate(2.5f, 0, 0);
            } else {
                isClosing = false;
                popUpContent.transform.eulerAngles = new Vector3(-90, 0, 0);
                Destroy(sujet);
                ResetPopUpButton();
                this.gameObject.SetActive(false);
            }
        }
    }

    public void CreatePopUpEvent(Noeud n) {
        UpdateTextPopUp(n.eventNumber);
        DisplayPopUp();
    }

    private void UpdateTextPopUp(int eventNumber) {

        switch (eventNumber) {
            case 1: // Evenement aléatoire
                GenerateRandomEvent();
                boutonAction.SetActive(true);
                boutonPartir.SetActive(true);
                break;
            case 3: // Feu de camp
                textePopUp.text = "Vous vous reposez auprès d'un feu de camp";
                sujet = Instantiate(feuDeCampPrefab, popUpContent.transform);
                boutonReposer.SetActive(true);
                break;
            case 4: // Magasin
                textePopUp.text = "Un marchand attire votre attention";
                sujet = Instantiate(marchandPrefab, popUpContent.transform);

                List<ItemData> listItem = GameManager.Instance.GetAllItems();
                int a=0, b=0, c=0;

                while(a==b || b==c || c==a) {
                    a = Random.Range(0, listItem.Count);
                    b = Random.Range(0, listItem.Count);
                    c = Random.Range(0, listItem.Count);
                }

                objet1 = listItem[a];
                imageObjet1.GetComponent<Image>().sprite = objet1.GetSprite();
                textePrice1.text = objet1.GetPrice()+" G";
                objet2 = listItem[b];
                imageObjet2.GetComponent<Image>().sprite = objet2.GetSprite();
                textePrice2.text = objet2.GetPrice()+" G";
                objet3 = listItem[c];
                imageObjet3.GetComponent<Image>().sprite = objet3.GetSprite();
                textePrice3.text = objet3.GetPrice()+" G";

                texteArgentJoueur.text = GameManager.Instance.GetRunPlayerCoins().ToString();

                encartArgentJoueur.SetActive(true);
                encartObjet1.SetActive(true);
                encartObjet2.SetActive(true);
                encartObjet3.SetActive(true);
                boutonPartir.SetActive(true);
                break;
            case 5: // Echange
                textePopUp.text = "Vous découvrez un puits magique";
                sujet = Instantiate(puitsPrefab, popUpContent.transform);
                boutonEchanger.SetActive(true);
                boutonPartir.SetActive(true);
                break;
            case 6: // Coffre
                textePopUp.text = "Vous trouvez un coffre au milieu de la foret";
                sujet = Instantiate(coffrePrefab, popUpContent.transform);
                boutonOuvrir.SetActive(true);
                break;
        }

        sujet.transform.SetAsFirstSibling();
        sujet.name = "Sujet";
    }

    private void ResetPopUpButton() {
        boutonPartir.SetActive(false);
        boutonOuvrir.SetActive(false);
        boutonEchanger.SetActive(false);
        boutonReposer.SetActive(false);
        boutonAction.SetActive(false);
        encartObjetTrouve.SetActive(false);
        encartEchange.SetActive(false);
        encartArgentJoueur.SetActive(false);
        encartObjet1.SetActive(false);
        encartObjet2.SetActive(false);
        encartObjet3.SetActive(false);
    }

    private void DisplayPopUp() {
        this.gameObject.SetActive(true);
        isOpening = true;
    }

    public void UndisplayPopUp() {
        isClosing = true;
    }

    public void OpenChest() {
        textePopUp.text = "Le coffre contenait de l'équipement !";
        Destroy(sujet);
        sujet = Instantiate(coffreOuverturePrefab, popUpContent.transform);
        sujet.transform.SetAsFirstSibling();
        sujet.name = "Sujet";

        List<ItemData> listItem = GameManager.Instance.GetAllItems();
        int randomItemValue = Random.Range(0, listItem.Count);
        texteNomObjetTrouve.text = listItem[randomItemValue].GetName();
        imageObjetTrouve.GetComponent<Image>().sprite = listItem[randomItemValue].GetSprite();
        encartObjetTrouve.SetActive(true);

        // AJOUTER ICI listItem[randomItemValue] DANS L'INVENTAIRE DU JOUEUR

        boutonOuvrir.SetActive(false);
        boutonPartir.SetActive(true);
    }

    public void Rest() {
        textePopUp.text = "Vous vous réveillez en meilleure forme !";
        Destroy(sujet);
        sujet = Instantiate(feuDeCampEteintPrefab, popUpContent.transform);
        sujet.transform.SetAsFirstSibling();
        sujet.name = "Sujet";

        // RENDRE DES POINTS DE VIE AU JOUEUR ICI

        boutonReposer.SetActive(false);
        boutonPartir.SetActive(true);
    }

    public void MakeTrade() {
        Destroy(sujet);
        encartEchange.SetActive(true);    
    }

    public void CancelTrade() {
        sujet = Instantiate(puitsPrefab, popUpContent.transform);
        sujet.transform.SetAsFirstSibling();
        sujet.name = "Sujet";
        encartEchange.SetActive(false);    
    }

    public void ConfirmTrade() {
        encartEchange.SetActive(false);
        textePopUp.text = "Le puits ne semble plus réagir !";
        Destroy(sujet);
        sujet = Instantiate(puitsVidePrefab, popUpContent.transform);
        sujet.transform.SetAsFirstSibling();
        sujet.name = "Sujet";

        // ICI ON SUPPRIME L'ITEM SELECTIONNE ET ON EN AJOUTE UN NOUVEAU (AFFICHER NOUVEL ITEM ?)

        boutonEchanger.SetActive(false);
        boutonPartir.SetActive(true);
    }

    public void BuyItem(int itemNumber) {
        int money = GameManager.Instance.GetRunPlayerCoins();

        switch (itemNumber) {
            case 1:
                if (money >= objet1.GetPrice()) {

                    // ICI ON PLACE L'OBJET ACHETE DANS LE SAC

                    encartObjet1.SetActive(false);
                    GameManager.Instance.AddCoinsToRunPlayerCoins(objet1.GetPrice()*-1);
                    texteArgentJoueur.text = GameManager.Instance.GetRunPlayerCoins().ToString();
                    texteArgentJoueurBarreDuHaut.text = GameManager.Instance.GetRunPlayerCoins().ToString();
                    textePopUp.text = "Merci de votre achat !";
                } else {
                    textePopUp.text = "Tu essaies de me rouler ?";
                }
                break;
            case 2:
                if (money >= objet2.GetPrice()) {

                    // ICI ON PLACE L'OBJET ACHETE DANS LE SAC

                    encartObjet2.SetActive(false);
                    GameManager.Instance.AddCoinsToRunPlayerCoins(objet2.GetPrice()*-1);
                    texteArgentJoueur.text = GameManager.Instance.GetRunPlayerCoins().ToString();
                    texteArgentJoueurBarreDuHaut.text = GameManager.Instance.GetRunPlayerCoins().ToString();
                    textePopUp.text = "Merci de votre achat !";
                } else {
                    textePopUp.text = "Tu essaies de me rouler ?";
                }
                break;
            case 3:
                if (money >= objet3.GetPrice()) {

                    // ICI ON PLACE L'OBJET ACHETE DANS LE SAC

                    encartObjet3.SetActive(false);
                    GameManager.Instance.AddCoinsToRunPlayerCoins(objet3.GetPrice()*-1);
                    texteArgentJoueur.text = GameManager.Instance.GetRunPlayerCoins().ToString();
                    texteArgentJoueurBarreDuHaut.text = GameManager.Instance.GetRunPlayerCoins().ToString();
                    textePopUp.text = "Merci de votre achat !";
                } else {
                    textePopUp.text = "Tu essaies de me rouler ?";
                }
                break;
        }
    }

    private void GenerateRandomEvent() {
        randomNumberEvent = Random.Range(0, 3);

        switch (randomNumberEvent) {
            case 0:
                textePopUp.text = "Vous trouvez un coffre étrange";
                boutonActionTexte.text = "Ouvrir";
                sujet = Instantiate(event0Prefab, popUpContent.transform);
                break;
            case 1:
                textePopUp.text = "Vous trouvez des baies dans la foret";
                boutonActionTexte.text = "Manger";
                sujet = Instantiate(event1Prefab, popUpContent.transform);
                break;
            case 2:
                textePopUp.text = "Vous entendez un cri au loin";
                boutonActionTexte.text = "Y aller";
                sujet = Instantiate(event2Prefab, popUpContent.transform);
                break;
        }

        sujet.transform.SetAsFirstSibling();
        sujet.name = "Sujet";
    }

    public void StartAction() {
        int randomResult = Random.Range(0, 2);

        // ICI ON APPLIQUE LE RESULTAT DES EVENEMENTS EN FONCTION DES CAS
        
        switch (randomNumberEvent) {
            case 0:
                if(randomResult == 1) {
                    textePopUp.text = "Vous trouvez un objet !";
                } else {
                    textePopUp.text = "Un mimic vous attaque !";
                }
                break;
            case 1:
                if(randomResult == 1) {
                    textePopUp.text = "Vous vous sentez revitaliser !";
                } else {
                    textePopUp.text = "Vous ne vous sentez pas trés bien...";
                }
                break;
            case 2:
                if(randomResult == 1) {
                    textePopUp.text = "RAS mais vous trouvez une bourse remplie";
                } else {
                    textePopUp.text = "Une embuscade !";
                }
                break;
        }

        boutonAction.SetActive(false);
    }

}