using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Shop : MonoBehaviour
{
    private Transform shopContainer;
    private IShopCustomer shopCustomer;

    [SerializeField] private GameObject shopItemTemplate;


    void Awake()
    {
        shopContainer = transform.Find("ShopContainer");

        Hide();
    }

    public void CreateItemInShop(ItemData itemData, int index)
    {
        float shopItemWidth = 500f;
        GameObject item = Instantiate(shopItemTemplate, shopContainer);
        RectTransform itemRectTransform = item.GetComponent<RectTransform>();
        UI_ShopItem uiShopItem = item.GetComponent<UI_ShopItem>();

        itemRectTransform.gameObject.SetActive(true);
        itemRectTransform.anchoredPosition = new Vector2(shopItemWidth * index - shopItemWidth, 0);

        uiShopItem.itemData = itemData;

    }

    public void TryBuyItem(ItemData itemData)
    {
        int playercoins = GameManager.Instance.GetPlayerCoins();

        if (playercoins >= itemData.GetPrice())
        {
            shopCustomer.BuyItem(itemData);
        }
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
