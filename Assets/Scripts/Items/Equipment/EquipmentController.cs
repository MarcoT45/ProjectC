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

    }
    #endregion

    private InventoryController inventory;
    private ItemData[] currentEquipement;

    public delegate void OnEquipmentChanged(ItemData newItem, ItemData oldItem);
    public static event OnEquipmentChanged onEquipmentChanged;

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
                            Debug.Log("unequip " + equippedItemData.GetName());
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

        //On trigger le delegate / ?.invoke pour savoir si des méthode y sont rattachées
        onEquipmentChanged?.Invoke(newItem, oldItem);

        ResetCurrentHitCounter();
    }

    public void Unequip(ItemData itemData)
    {
        int indexEquipmentType = (int) itemData.GetItemType();

        if(indexEquipmentType >= 0 && indexEquipmentType < currentEquipement.Length)
        {
            Debug.Log(indexEquipmentType);
            if (currentEquipement[indexEquipmentType] != null)
            {
                Debug.Log("equipement != nul");
                ItemData oldItem = currentEquipement[indexEquipmentType];

                Debug.Log(currentEquipement[indexEquipmentType].ToString());

                inventory.AddItem(oldItem);

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
            currentHitCounter = (int) currentEquipement[(int) ItemType.Arme].GetAttack();
        } else {
            currentHitCounter = 0;
        }
    }

}