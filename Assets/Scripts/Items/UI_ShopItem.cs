using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;


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

    private LocalizeStringEvent localizedStringEventTitle;
    private LocalizeStringEvent localizedStringEventDescription;

    private void Awake()
    {
        localizedStringEventTitle = tmpNom.gameObject.GetComponent<LocalizeStringEvent>();
        localizedStringEventDescription = tmpDescription.gameObject.GetComponent<LocalizeStringEvent>();
    }

    void Start()
    {
        cardBackIsActive = false;

        string itemId = itemData.GetNumero().ToString();
        uiSprite.sprite = itemData.GetSprite();
        localizedStringEventTitle.StringReference.SetReference("EquipementTable", "Item_" + itemId + "_Name");
        localizedStringEventDescription.StringReference.SetReference("EquipementTable", "Item_" + itemId + "_Name");
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
        tmpNom.text = itemData.GetName().GetLocalizedString();
        tmpDescription.text = itemData.GetDescription().GetLocalizedString();
        tmpPrix.text = itemData.GetPrice() + "";
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        bool bought = false ;
        if(!cardBackIsActive)
        {
            bought = TryBuyItem(itemData);
            
            if(bought) Flip();
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
    public bool TryBuyItem(ItemData itemData)
    {
        int playercoins = GameManager.Instance.GetRunPlayerCoins();

        IShopCustomer shopCustomer = GameObject.FindWithTag("Player").GetComponent<IShopCustomer>();

        if (playercoins >= itemData.GetPrice() && InventoryController.Instance.GetItemList().Count < InventoryController.Instance.inventorySize)
        {
            shopCustomer.BuyItem(itemData);

            return true;
        }

        return false;
    }

/*    private bool TryBuyItem(ItemData itemData)
    {
        UI_Shop ui_shop = GameObject.FindWithTag("UI_Shop").GetComponent<UI_Shop>();
        
        return ui_shop.TryBuyItem(itemData);
    }*/
}
