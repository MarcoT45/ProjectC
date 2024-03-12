using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class UI_ShopItem : MonoBehaviour, IPointerClickHandler
{
    public GameObject cardBack;
    public GameObject cardFront;
    public bool cardBackIsActive;

    public TextMeshProUGUI tmpNom;
    public TextMeshProUGUI tmpDescription;
    public TextMeshProUGUI tmpPrix;
    public Image uiSprite;
    public ItemData itemData;

    // Start is called before the first frame update
    void Start()
    {
        cardBackIsActive = false;

        uiSprite.sprite = itemData.GetSprite();
        tmpNom.text = itemData.GetName();
        tmpDescription.text = itemData.GetDescription();
        tmpPrix.text = itemData.GetPrice()+"";
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ReloadItemData(ItemData itemData)
    {
        this.itemData = itemData;
        uiSprite.sprite = itemData.GetSprite();
        tmpNom.text = itemData.GetName();
        tmpDescription.text = itemData.GetDescription();
        tmpPrix.text = itemData.GetPrice() + "";
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(!cardBackIsActive)
        {
            TryBuyItem(itemData);
            Flip();
        }
    }

    //Faire un vrai Flip pour plus tard
    public void Flip()
    {
        if (cardBackIsActive) 
        { 
            cardBack.SetActive(false);
            cardBackIsActive = false;
            cardFront.SetActive(true);
        }
        else
        {
            cardBack.SetActive(true);
            cardBackIsActive = true;
            cardFront.SetActive(false);
        }
    }

    private void TryBuyItem(ItemData itemData)
    {
        UI_Shop ui_shop = GameObject.FindWithTag("UI_Shop").GetComponent<UI_Shop>();
        ui_shop.TryBuyItem(itemData);
    }
}
