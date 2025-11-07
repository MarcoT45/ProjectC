using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                itemDataRandom = items[2][Random.Range(0, items[2].Count)];
            }

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
        //On ferme le dialogue suite au choix
        DialogueManager.Instance.EndDialogue();

        IShopCustomer shopCustomer = this.collidingPlayer.GetComponent<IShopCustomer>();
        switch (choiceMade)
        {
            //Acheter
            case 0:
                Debug.Log("Opening shop UI");
                ControlsManager.Instance.UpdateState(110);

                if (shopCustomer != null)
                {
                    uiShop.Show(shopCustomer);
                }else
                {
                    Debug.LogWarning("No IShopCustomer found on the player.");
                    ControlsManager.Instance.UpdateState(5);
                }
                break;

            //Vendre
            case 1:
                break;

            //Partir
            case 2:
                ControlsManager.Instance.UpdateState(5);
                break;

        }
    }
}
