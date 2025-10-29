using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    #region  Singleton
    private static EquipmentController instance = null;
    public static EquipmentController Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance =  this;
        }
        DontDestroyOnLoad(this.gameObject);

        // Appels supplémentaires non dépendante de la partie Singleton
        totalStats.Clear();
    }
    #endregion

    private InventoryController inventory;
    private ItemData[] currentEquipement;
    private CharacterStats totalStats = new CharacterStats();

    public delegate void OnEquipmentChanged(ItemData newItem, ItemData oldItem);
    public static event OnEquipmentChanged onEquipmentChanged;

    public delegate void OnStatsChanged();
    public static event OnStatsChanged onStatsChanged;

    public void OnEnable()
    {
        UIInventoryItem.onItemDroppedOn += HandleEquipmentDrag;
        UIEquipmentItem.onEquipmentDrop += HandleDrop;
    }

    public void OnDisable()
    {
        UIInventoryItem.onItemDroppedOn -= HandleEquipmentDrag;
        UIEquipmentItem.onEquipmentDrop -= HandleDrop;
    }

    public void Start()
    {
        inventory = InventoryController.Instance;
        int numSlots = System.Enum.GetNames(typeof(ItemType)).Length;
        currentEquipement = new ItemData[numSlots];
    }

    public void Update()
    {
        //Press E to equip an item for testing
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventory.GetItemList().Count > 0)
            {
                Debug.Log("Equip first item in inventory: " + inventory.GetItemList()[0].GetName());
                Equip(inventory.GetItemList()[0]);
            }
        }
    }

    public void HandleDrop(DraggableItem draggableItem)
    {
        Transform parent = draggableItem.originalParent;
        UIInventoryItem uiItem = parent.GetComponent<UIInventoryItem>();

        if (uiItem != null && uiItem.GetItemData() != null)
        {
            Equip(uiItem.GetItemData());
        }
    }

    public void HandleEquipmentDrag(UIInventoryItem uiItem, DraggableItem draggableItem)
    {
        //draggableItem non null pour savoir s'il provient de l'equipement
        if (draggableItem != null)
        {
            Transform parent = draggableItem.originalParent;
            UIEquipmentItem uiEquipmentItem = parent.GetComponent<UIEquipmentItem>();

            if (uiEquipmentItem != null && uiEquipmentItem.GetItemData() != null)
            {
                if (uiItem == null)
                {
                    //Debug.Log("ui null " + uiEquipmentItem.GetItemData().GetName());
                    Unequip(uiEquipmentItem.GetItemData());
                }
                else
                {
                    if (uiItem.GetItemData() != null)
                    {
                        ItemData equippedItemData = uiEquipmentItem.GetItemData();
                        ItemData inventoryItemData = uiItem.GetItemData();

                        if (equippedItemData.GetItemType() == inventoryItemData.GetItemType())
                        {
                            //Debug.Log("equip");
                            Equip(inventoryItemData);
                        }
                        else
                        {
                            Unequip(equippedItemData);
                        }
                    }
                }
            }
        }
    }

    public void Equip(ItemData newItem)
    {
        //Récupère l'index de la valeur de l'enum. ex: Casque = 1
        int equipSlot = (int) newItem.GetItemType();

        ItemData oldItem = null;

        //Si il y a déjà un equipement, on ajoute l'ancien dans l'inventaire
        if (currentEquipement[equipSlot] != null)
        {
            oldItem = currentEquipement[equipSlot];
            inventory.AddItem(oldItem);

        }

        currentEquipement[equipSlot] = newItem;
        inventory.RemoveItem(newItem);
        RecalculateStats();

        //On trigger le delegate / ?.invoke pour savoir si des méthode y sont rattachées
        onEquipmentChanged?.Invoke(newItem, oldItem);

        ResetCurrentHitCounter();
    }

    public void Unequip(ItemData itemData)
    {
        int indexEquipmentType = (int) itemData.GetItemType();

        if(indexEquipmentType >= 0 && indexEquipmentType < currentEquipement.Length)
        {
            if (currentEquipement[indexEquipmentType] != null)
            {
                ItemData oldItem = currentEquipement[indexEquipmentType];

                inventory.AddItem(oldItem);
                RecalculateStats();

                onEquipmentChanged?.Invoke(null, oldItem);
                currentEquipement[indexEquipmentType] = null;

                ResetCurrentHitCounter();
            }
        }
        else
        {
            Debug.Log("index erreur "+ indexEquipmentType);
        }
    }

    public void TriggerPassives(EquipmentTriggerType trigger, GameObject context = null, float value = 0f)
    {
        foreach (ItemData item in currentEquipement)
        {
            if (item != null && item.passiveEffects != null)
            {
                foreach(EquipmentPassive passive in item.passiveEffects)
                {
                    if(passive != null)
                    {
                        passive.ApplyEffect(this.gameObject, trigger, context, value);
                    }
                }
            }
        }
    }

    public CharacterStats GetTotalStats()
    {
        return totalStats;
    }
    private void RecalculateStats()
    {
        totalStats.Clear();
        foreach (var item in currentEquipement)
        {
            if (item != null)
            {
                totalStats.Add(item.stats);
            }
        }

        onStatsChanged?.Invoke();
    }

    public ItemData[] GetCurrentEquipement()
    {
        return currentEquipement;
    }

    public ItemData GetCasque() {
        return currentEquipement[(int) ItemType.Casque];
    }

    public ItemData GetTorse() {
        return currentEquipement[(int) ItemType.Torse];
    }

    public ItemData GetBottes() {
        return currentEquipement[(int) ItemType.Bottes];
    }

    public ItemData GetArme() {
        return currentEquipement[(int) ItemType.Arme];
    }

    public ItemData GetAccessoireJ() {
        return currentEquipement[(int) ItemType.Accessoire];
    }

    public ItemData GetAccessoireK() {
        return currentEquipement[(int) ItemType.Accessoire + 1];
    }


    // Test pour le compteur de coup disponible/restant

    private int currentHitCounter = 0;

    public int GetCurrentHitCounter() {
        return currentHitCounter;
    }

    public void MinusHitCounter() {
        currentHitCounter--;
    }

    public void ResetCurrentHitCounter() {
        if(currentEquipement[(int) ItemType.Arme] != null) {
            currentHitCounter = (int) currentEquipement[(int) ItemType.Arme].GetStats().atk;
        } else {
            currentHitCounter = 0;
        }
    }

}