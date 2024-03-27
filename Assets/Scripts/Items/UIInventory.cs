using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UIInventory : MonoBehaviour
{
    private InventoryController inventory;

    [SerializeField]
    private UIInventoryItem itemSlotPrefab;

    [SerializeField]
    private RectTransform contentPanel;

    private List<UIInventoryItem> listUiItems = new List<UIInventoryItem>();

    private int currentlyDraggedItemIndex = -1;

    public delegate void OnSwapItems(int itemIndex1, int itemIndex2);
    public static event OnSwapItems onSwapItems;

    public delegate void OnDescriptionRequested(int index);
    public static event OnDescriptionRequested onDescriptionRequested;

    private void Awake()
    {
        inventory = InventoryController.Instance;
    }

    public void OnEnable()
    {
        InventoryController.onInventoryChanged += UpdateInventoryUI;
        DraggableItem.onItemBeginDrag += HandleBeginDrag;
        DraggableItem.onItemEndDrag += UpdateInventoryUI;
        UIInventoryItem.onItemDroppedOn += HandleSwap;
        UIInventoryItem.onItemClicked += HandleClick;
        UpdateInventoryUI();

    }

    public void OnDisable()
    {
        InventoryController.onInventoryChanged -= UpdateInventoryUI;
        DraggableItem.onItemBeginDrag -= HandleBeginDrag;
        UIInventoryItem.onItemDroppedOn -= HandleSwap;
    }

    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            UIInventoryItem uiItem = Instantiate(itemSlotPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            uiItem.transform.localScale = new Vector3(1, 1, 1);
            listUiItems.Add(uiItem);
        }

    }

    public void UpdateInventoryUI()
    {

        ResetAll();
        foreach (ItemData item in inventory.GetItemList())
        {
            int index = inventory.GetItemList().IndexOf(item);
            if (listUiItems.Count > index)
            {
                listUiItems[index].SetData(item);
            }
        }
    }

    public void ResetAll()
    {
        foreach (UIInventoryItem uiItem in listUiItems)
        {
            uiItem.ResetData();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void HandleBeginDrag(UIInventoryItem uiItem)
    {
        int index = listUiItems.IndexOf(uiItem);
        if (index > -1)
        {
            currentlyDraggedItemIndex = index;
            onDescriptionRequested?.Invoke(index);
        }
    }

    public void HandleClick(UIInventoryItem uiItem)
    {
        int index = listUiItems.IndexOf(uiItem);
        if (index > -1)
        {
            onDescriptionRequested?.Invoke(index);
        }
    }

    public void HandleSwap(UIInventoryItem uiItem)
    {
        int index = listUiItems.IndexOf(uiItem);
        if (index > -1)
        {
            onSwapItems?.Invoke(currentlyDraggedItemIndex, index);
        }

    }
}
