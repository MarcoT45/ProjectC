using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class UI_Shop : MonoBehaviour
{
    private Transform shopContainer;
    private IShopCustomer shopCustomer;

   /* [SerializeField] private GameObject shopItemTemplate;*/
    [SerializeField] private UI_ShopItem shopItem1;
    [SerializeField] private UI_ShopItem shopItem2;
    [SerializeField] private UI_ShopItem shopItem3;


    void Awake()
    {
        shopContainer = transform.Find("ShopContainer");

        Hide();
    }


    public void CreateItemInShop(ItemData itemData, int index)
    {
        switch (index)
        {
            case 0:
                shopItem1.itemData = itemData;
                break;

            case 1:
                shopItem2.itemData = itemData;
                break;

            case 2:
                shopItem3.itemData = itemData;
                break;
        }

    }

    public bool TryBuyItem(ItemData itemData)
    {
        int playercoins = GameManager.Instance.GetRunPlayerCoins();

        if (playercoins >= itemData.GetPrice() &&  InventoryController.Instance.GetItemList().Count < InventoryController.Instance.inventorySize)
        {
            shopCustomer.BuyItem(itemData);

            return true;
        }

        return false;
    }


    public void Show(IShopCustomer shopCustomer)
    {
        this.shopCustomer = shopCustomer;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
