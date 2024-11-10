using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ShopController : NPC, ITalkable
{
    [SerializeField] private UI_Shop uiShop;
    [SerializeField] private int maxItemsInShop;
    [SerializeField] private DialogueText dialogueText;
    private List<ItemData> shopItems;
    public int reloadPrice;

    private void Start()
    {
        this.ResetItems();
        this.AddItemsInShop();
        this.DisplayItems();
    }

    protected override void Update()
    {
        base.Update();

;       if(shopItems.Count == 0)
        {
            this.AddItemsInShop();
            this.DisplayItems();
        }
    }

    private void AddItemsInShop()
    {
       // List<ItemData> items = GameManager.Instance.GetAllItems(); 
       /* int random = Random.Range(1, 101); // 1 - 100

        foreach (ItemData item in items)
        {
            if (random <= item.GetRarity())
            {
                shopItems.Add(item);
            }
        }*/

        //Boucle sur nb item dans le shop
        for(int i = 0; i < 3; i++)
        {
            int random = Random.Range(1, 101);
            //int rarity = 0;
            List<List<ItemData>> items = GameManager.Instance.itemsTriRarete;
            ItemData itemDataRandom = null;

            if (random < 61)
            {
                //rarity = 1;
                itemDataRandom = items[0][Random.Range(0, items[0].Count)];

            }
            else if ( random < 91)
            {
                //rarity = 2;
                itemDataRandom = items[1][Random.Range(0, items[1].Count)];
            }
            else
            {
                //rarity = 3;
                itemDataRandom = items[0][Random.Range(0, items[2].Count)];
            }
/*
            itemDataRandom = items[Random.Range(0, items.Count)];


            while (itemDataRandom.GetRarity() != rarity)
            {
                itemDataRandom = items[Random.Range(0, items.Count)];
            }
*/

            shopItems.Add(itemDataRandom);
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
            uiShop.CreateItemInShop(shopItems[0], 0);
            uiShop.CreateItemInShop(shopItems[1], 1);
            uiShop.CreateItemInShop(shopItems[2], 2);
        }
    }

    public void ReloadItems()
    {
        int playercoins = GameManager.Instance.GetRunPlayerCoins();

        if (playercoins >= reloadPrice)
        {
            GameManager.Instance.SetRunPlayerCoins(playercoins - reloadPrice);
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

//Plus nécessaire pour le moment
/*
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
    }*/

    public override void Interact()
    {
        Talk(dialogueText);
    }

    public void Talk(DialogueText dialogueText)
    {
        //Appel du DialogueManager avec le DialogueText adequat et une action pour utiliser une suite à un choix
        DialogueManager.Instance.DisplayDialogue(dialogueText, (onChoiceSelected) => ChoiceDone(onChoiceSelected));
    }

    //Fonction gérant les choix fait suite au dialogue du vendeur
    public void ChoiceDone(int choiceMade)
    {
        IShopCustomer shopCustomer = collidingPlayer.GetComponent<IShopCustomer>();
        switch (choiceMade)
        {
            //Acheter
            case 0:

                if (shopCustomer != null)
                {
                    uiShop.Show(shopCustomer);
                }
                break;

            //Vendre
            case 1:
                break;

            //Partir
            case 2:
                DialogueManager.Instance.EndDialogue();
                ControlsManager.Instance.UpdateState(5);
                break;

        }
    }
}
