using System.Collections;
using System.Collections.Generic;
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

    public void Start()
    {
        inventory = InventoryController.Instance;
        int numSlots = System.Enum.GetNames(typeof(ItemType)).Length;
        currentEquipement = new ItemData[numSlots];
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

        //On trigger le delegate / ?.invoke pour savoir si des méthode y sont rattachées
        onEquipmentChanged?.Invoke(newItem, oldItem);

        currentEquipement[equipSlot] = newItem;

    }

    public void Unequip(ItemData itemData)
    {
        int indexEquipmentType = (int) itemData.GetItemType();

        if (currentEquipement[indexEquipmentType] != null)
        {
            ItemData oldItem = currentEquipement[indexEquipmentType];
            inventory.AddItem(oldItem);

            currentEquipement[indexEquipmentType] = null;

            onEquipmentChanged?.Invoke(null, oldItem);

        }
    }

}
