 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    #region  Singleton
    private static InventoryController instance = null;
    public static InventoryController Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        //Awake mais pas Singleton
        playerInput = new PlayerMovement();
        menuOpenCloseAction = playerInput.Main.MenuOpenClose;
    }
    #endregion

    private List<ItemData> items;
    public bool isMenuOpenCloseInputPressed;

    public int inventorySize = 30;

    [SerializeField]
    private UIInventory inventoryUI;

    [SerializeField]
    private UIInventoryDescription inventoryDescriptionUI;

    private PlayerMovement playerInput;
    private InputAction menuOpenCloseAction;

    public delegate void OnInventoryChanged();
    public static event OnInventoryChanged onInventoryChanged;

    private void OnEnable()
    {
        UIInventory.onSwapItems += HandleSwapItems;
        UIInventory.onDescriptionRequested += UpdateDescription;
        playerInput.Enable();
    }

    private void OnDisable()
    {
        UIInventory.onSwapItems -= HandleSwapItems;
        playerInput.Disable();
    }

    public void Start()
    {
        items = new List<ItemData>();
        inventoryUI.InitializeInventoryUI(inventorySize);

        //------ A retirer plus tard
        foreach (ItemData item in GameManager.Instance.GetAllItems())
        {
            AddItem(item);
            AddItem(item);
        }
        //------
    }

    public void Update()
    {
        isMenuOpenCloseInputPressed = menuOpenCloseAction.triggered;
        if (isMenuOpenCloseInputPressed)
        {
            if (inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.Show();
            }
            else
            {
                inventoryUI.Hide();
            }
        }
    }

    public List<ItemData> GetItemList()
    {
        return items;
    }

    public void AddItem(ItemData item)
    {
        if(items.Count < 30 && !items.Contains(item))
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
}
