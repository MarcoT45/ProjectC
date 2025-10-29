using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InventoryController : MonoBehaviour
{
    #region  Singleton
    private static InventoryController instance = null;
    public static InventoryController Instance => instance;

    private void Awake()
    {
        Debug.Log("InventoryController Awake - Setting up Singleton Instance");
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Multiple instances of InventoryController detected. Destroying duplicate.");
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

    }
    #endregion

    public List<ItemData> items;
    public int inventorySize = 30;

    [SerializeField]
    private UIInventory inventoryUI;

    [SerializeField]
    private UIInventoryDescription inventoryDescriptionUI;

    public delegate void OnInventoryChanged();
    public static event OnInventoryChanged onInventoryChanged;

    private void OnEnable()
    {
        UIInventory.onSwapItems += HandleSwapItems;
        UIInventory.onDescriptionRequested += UpdateDescription;
        //EquipmentController.onEquipmentChanged += HandleEquipmentChange;
    }


    private void OnDisable()
    {
        UIInventory.onSwapItems -= HandleSwapItems;
        UIInventory.onDescriptionRequested -= UpdateDescription;
        //EquipmentController.onEquipmentChanged -= HandleEquipmentChange;
    }

    public void Start()
    {
        Debug.Log("InventoryController Start - Initializing Inventory");
        items = new List<ItemData>();
        inventoryUI.InitializeInventoryUI(inventorySize);

        //Vérification de l'instance de GameManager
        if ( GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is null. Cannot load items into inventory.");
            return;
        }

        //------ A retirer plus tard
        foreach (ItemData item in GameManager.Instance.GetAllItems())
        {
            AddItem(item);
        }
        //------
    }

    public void Update()
    {
        if (ControlsManager.Instance.controlsState == ControlsState.CharacterHub ||
            ControlsManager.Instance.controlsState == ControlsState.Combat || 
            ControlsManager.Instance.controlsState == ControlsState.Inventaire )
        {
            if (ControlsManager.Instance.InventairePressed)
            {
                Debug.Log("Inventory Toggle Pressed");
                //ControlsState previousControlsState = ControlsManager.Instance.controlsState;
                ToggleInventoryUI();
            }
        }
    }

    public List<ItemData> GetItemList()
    {
        return items;
    }

    public void AddItem(ItemData item)
    {
        if(items.Count < inventorySize )
        {
            items.Add(item);
        }

        onInventoryChanged?.Invoke();
    }

    public void RemoveItem(ItemData item) 
    {
        items.Remove(item);
        onInventoryChanged?.Invoke();
    }

    public void ClearItems() { items.Clear(); }

    public void UpdateDescription(int index)
    {
        ItemData item = items[index];
        inventoryDescriptionUI.SetDescription(item); 
    }

    public void HandleSwapItems(int itemIndex1, int itemIndex2)
    {
        ItemData item1 = items[itemIndex1];
        items[itemIndex1] = items[itemIndex2];
        items[itemIndex2] = item1;

        onInventoryChanged?.Invoke();
    }

    public void HandleEquipmentChange(ItemData newItem, ItemData oldItem)
    {
        if(newItem != null)
        {
            RemoveItem(newItem);
        }

        if(oldItem != null)
        {
            AddItem(oldItem);
        }

    }

    public void ToggleInventoryUI()
    {
        if (inventoryUI.isActiveAndEnabled == false)
        {
            inventoryUI.Show();
            ControlsManager.Instance.controlsState = ControlsState.Inventaire;
        }
        else
        {
            inventoryUI.Hide();
            ControlsManager.Instance.controlsState = ControlsState.CharacterHub;
        }

        Debug.Log("Controls State: " + ControlsManager.Instance.controlsState);
    }
}
