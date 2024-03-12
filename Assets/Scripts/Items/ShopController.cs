using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private UI_Shop uiShop;

    [SerializeField] private int maxItemsInShop;
    private List<ItemData> shopItems;
    public int reloadPrice;

    private void Start()
    {
        this.ResetItems();
        this.AddItemsInShop();
        this.DisplayItems();
    }

    void Update()
    {
        if(shopItems.Count == 0)
        {
            this.AddItemsInShop();
            this.DisplayItems();
        }
    }

    private void AddItemsInShop()
    {
        List<ItemData> items = GameManager.Instance.GetAllItems();
        int random = Random.Range(0, 101); // 1 - 100

        foreach (ItemData item in items)
        {
            if (random <= item.GetRarity())
            {
                shopItems.Add(item);
            }
        }

    }
    private void ResetItems()
    {
        shopItems = new List<ItemData>();
    }

    private void DisplayItems()
    {

        if (shopItems.Count > 0)
        {
            uiShop.CreateItemInShop(shopItems[Random.Range(0, shopItems.Count)], 0);
            uiShop.CreateItemInShop(shopItems[Random.Range(0, shopItems.Count)], 1);
            uiShop.CreateItemInShop(shopItems[Random.Range(0, shopItems.Count)], 2);
        }
    }

    public void ReloadItems()
    {
        int playercoins = GameManager.Instance.GetPlayerCoins();

        if (playercoins >= reloadPrice)
        {
            GameManager.Instance.SetPlayerCoins(playercoins - reloadPrice);
            this.ResetItems();
            GameObject[] uiShopItems = GameObject.FindGameObjectsWithTag("UI_ShopItem");
            foreach (GameObject uiItem in uiShopItems)
            {
                UI_ShopItem uiItemScript = uiItem.GetComponent<UI_ShopItem>();
                if (!uiItemScript.cardBackIsActive)
                {
                    uiItemScript.Flip();
                }
            }

            this.AddItemsInShop();
            foreach (GameObject uiItem in uiShopItems)
            {
                UI_ShopItem uiItemScript = uiItem.GetComponent<UI_ShopItem>();
                uiItemScript.ReloadItemData((shopItems[Random.Range(0, shopItems.Count)]));
                uiItemScript.Flip();
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IShopCustomer shopCustomer = other.GetComponent<IShopCustomer>();
        if(shopCustomer != null )
        {
            uiShop.Show(shopCustomer);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IShopCustomer shopCustomer = other.GetComponent<IShopCustomer>();
        if (shopCustomer != null)
        {
            uiShop.Hide();
        }
    }
}
