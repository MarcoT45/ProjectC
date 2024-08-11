using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EquipediaBookContentController : MonoBehaviour {

    // Pour savoir si le livre est ouvert
    public GameObject popUpBookLeft;
    public GameObject popUpBookRight;

    // GameObject et TextMeshPro du livre (Page Gauche)
    public TextMeshProUGUI nbItemDiscoveredText;
    public TextMeshProUGUI nbPage;
    public GameObject previousPage;
    public GameObject nextPage;
    public List<GameObject> emplacementList;

    // GameObject et TextMeshPro du livre (Page Gauche)
    public TextMeshProUGUI numeroItem;
    public GameObject typeItem;
    public TextMeshProUGUI nameItem;
    public GameObject spriteItem;
    public GameObject rarityItem;
    public TextMeshProUGUI priceItem;
    public TextMeshProUGUI attackItem;
    public TextMeshProUGUI defenseItem;
    public TextMeshProUGUI speedItem;
    public TextMeshProUGUI luckItem;
    public TextMeshProUGUI descriptionItem;

    // Info pour le livre
    private List<ItemData> catalogInfo;
    private int currentPage = 1;
    private int totalPage;
    private int nbItemDiscovered = 0;
    private int numberSelected = 1;

    // Sprite nécessaire
    public Sprite selectedSprite;
    public Sprite unselectedSprite;
    public Sprite questionmarkSprite;
    public List<Sprite> rarityItemSprite;
    public List<Sprite> typeItemSprite;

    private void Start() {

        catalogInfo = GameManager.Instance.GetAllItems();

        foreach (var i in catalogInfo) {
            if (i.GetDiscovered()) {
                nbItemDiscovered++;                
            }
        }

        nbItemDiscoveredText.text = nbItemDiscovered + "/" + catalogInfo.Count;

        // Pour arrondir l'entier à la valeur supérieur
        totalPage = (catalogInfo.Count + 12 - 1 ) / 12; 
        nbPage.text = currentPage + "/" + totalPage;

        // On affiche les infos pour changer de page ou non (ca ne sert à rien pour l'initialisation je le mets pour la logique)
        if(currentPage == 1) {
            previousPage.SetActive(false);
        }
        if(totalPage == 1) {
            nextPage.SetActive(false);
        }

        DisplayItemListForLeftPage();
        DisplayItemForRightPage();
    }

    private void Update() {

        if (isBookOpen()) {
            if (Input.GetKeyDown("q") || Input.GetKeyDown("a")) {
                if(numberSelected > 1 && emplacementList[numberSelected-2].activeSelf) {
                    Image icon = (Image) emplacementList[numberSelected-1].GetComponent(typeof(Image));
                    Image icon2 = (Image) emplacementList[numberSelected-2].GetComponent(typeof(Image));

                    icon.sprite = unselectedSprite;
                    icon2.sprite = selectedSprite;

                    numberSelected = numberSelected - 1;
                    DisplayItemForRightPage();
                }
            }

            if (Input.GetKeyDown("d")) {
                if(numberSelected < 12 && emplacementList[numberSelected].activeSelf) {
                    Image icon = (Image) emplacementList[numberSelected-1].GetComponent(typeof(Image));
                    Image icon2 = (Image) emplacementList[numberSelected].GetComponent(typeof(Image));

                    icon.sprite = unselectedSprite;
                    icon2.sprite = selectedSprite;

                    numberSelected = numberSelected + 1;
                    DisplayItemForRightPage();
                }
            }

            if (Input.GetKeyDown("z") || Input.GetKeyDown("w")) {
                if(numberSelected > 4 && emplacementList[numberSelected-5].activeSelf) {
                    Image icon = (Image) emplacementList[numberSelected-1].GetComponent(typeof(Image));
                    Image icon2 = (Image) emplacementList[numberSelected-5].GetComponent(typeof(Image));

                    icon.sprite = unselectedSprite;
                    icon2.sprite = selectedSprite;

                    numberSelected = numberSelected - 4;
                    DisplayItemForRightPage();
                }
            }

            if (Input.GetKeyDown("s")) {
                if(numberSelected < 9 && emplacementList[numberSelected+3].activeSelf) {
                    Image icon = (Image) emplacementList[numberSelected-1].GetComponent(typeof(Image));
                    Image icon2 = (Image) emplacementList[numberSelected+3].GetComponent(typeof(Image));

                    icon.sprite = unselectedSprite;
                    icon2.sprite = selectedSprite;

                    numberSelected = numberSelected + 4;
                    DisplayItemForRightPage();
                }
            }

            if (Input.GetKeyDown("i")) {
                if(currentPage != 1) {
                    currentPage--;
                    nbPage.text = currentPage + "/" + totalPage;
                    if(currentPage == 1) {
                        previousPage.SetActive(false);
                    }
                    nextPage.SetActive(true);
                    DisplayItemListForLeftPage();

                    Image icon = (Image) emplacementList[numberSelected-1].GetComponent(typeof(Image));
                    icon.sprite = unselectedSprite;

                    numberSelected = 1;

                    Image icon2 = (Image) emplacementList[numberSelected-1].GetComponent(typeof(Image));
                    icon2.sprite = selectedSprite;

                    DisplayItemForRightPage();
                }
            }

            if (Input.GetKeyDown("p")) {
                if(currentPage != totalPage) {
                    currentPage++;
                    nbPage.text = currentPage + "/" + totalPage;
                    previousPage.SetActive(true);
                    if(currentPage == totalPage) {
                        nextPage.SetActive(false);
                    }
                    DisplayItemListForLeftPage();

                    Image icon = (Image) emplacementList[numberSelected-1].GetComponent(typeof(Image));
                    icon.sprite = unselectedSprite;

                    numberSelected = 1;

                    Image icon2 = (Image) emplacementList[numberSelected-1].GetComponent(typeof(Image));
                    icon2.sprite = selectedSprite;

                    DisplayItemForRightPage();
                }
            }
        }
    }

    private void DisplayItemListForLeftPage() {
        ReactivateAllBox();

        for(int i = 12*(currentPage-1); i < 12*currentPage; i++) {
            if(i > catalogInfo.Count - 1) {
                GameObject box = GameObject.Find("Emplacement Item "+ ((i%12)+1));
                box.SetActive(false);
            } else {
                GameObject iconObject = GameObject.Find("Icone Emplacement Item "+ ((i%12)+1));
                Image icon = (Image) iconObject.GetComponent(typeof(Image));

                if(catalogInfo[i].GetDiscovered()) {
                    icon.sprite = catalogInfo[i].GetSprite();
                } else {
                    icon.sprite = questionmarkSprite;
                }
            }
        }
    }

    private void DisplayItemForRightPage() {
        if(catalogInfo[(currentPage-1)*12 + numberSelected - 1].GetDiscovered()) {
            numeroItem.text = catalogInfo[(currentPage-1)*12 + numberSelected - 1].GetNumero().ToString();
            nameItem.text = catalogInfo[(currentPage-1)*12 + numberSelected - 1].GetName();
            priceItem.text = catalogInfo[(currentPage-1)*12 + numberSelected - 1].GetPrice().ToString();
            attackItem.text = catalogInfo[(currentPage-1)*12 + numberSelected - 1].GetAttack().ToString();
            defenseItem.text = catalogInfo[(currentPage-1)*12 + numberSelected - 1].GetDefense().ToString();
            speedItem.text = catalogInfo[(currentPage-1)*12 + numberSelected - 1].GetSpeed().ToString();
            luckItem.text = catalogInfo[(currentPage-1)*12 + numberSelected - 1].GetLuck().ToString();
            descriptionItem.text = catalogInfo[(currentPage-1)*12 + numberSelected - 1].GetDescription();
            Image icon = (Image) typeItem.GetComponent(typeof(Image));
            icon.sprite = typeItemSprite[(int) catalogInfo[(currentPage-1)*12 + numberSelected - 1].GetItemType()];
            Image icon2 = (Image) spriteItem.GetComponent(typeof(Image));
            icon2.sprite = catalogInfo[(currentPage-1)*12 + numberSelected - 1].GetSprite();
            Image icon3 = (Image) rarityItem.GetComponent(typeof(Image));
            icon3.sprite = rarityItemSprite[catalogInfo[(currentPage-1)*12 + numberSelected - 1].GetRarity() - 1];
        } else {
            numeroItem.text = catalogInfo[(currentPage-1)*12 + numberSelected - 1].GetNumero().ToString();
            nameItem.text = "???";
            priceItem.text = "?";
            attackItem.text = "?";
            defenseItem.text = "?";
            speedItem.text = "?";
            luckItem.text = "?";
            descriptionItem.text = "???";

            Image icon = (Image) typeItem.GetComponent(typeof(Image));
            icon.sprite = questionmarkSprite;
            Image icon2 = (Image) spriteItem.GetComponent(typeof(Image));
            icon2.sprite = questionmarkSprite;
            Image icon3 = (Image) rarityItem.GetComponent(typeof(Image));
            icon3.sprite = questionmarkSprite;
        }
    }

    private void ReactivateAllBox () {
        for(int i = 0; i < 12; i++) {
            emplacementList[i].SetActive(true);
        }
    }

    private bool isBookOpen() {
        Vector3 targetAngle = new Vector3(0, 0, 0);
        if (Vector3.Distance(popUpBookLeft.transform.eulerAngles, targetAngle) == 0 || Vector3.Distance(popUpBookRight.transform.eulerAngles, targetAngle) == 0) {
            return true;
        } else {
            return false;
        }
    }

}