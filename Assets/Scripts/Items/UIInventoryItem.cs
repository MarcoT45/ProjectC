using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIInventoryItem : MonoBehaviour, IPointerClickHandler, IDropHandler
{
    [SerializeField]
    private Image itemImage;

    private ItemData itemData;

    public delegate void OnItemDroppedOn(UIInventoryItem uiItem, DraggableItem draggedItem);
    public static event OnItemDroppedOn onItemDroppedOn;

    public delegate void OnItemClicked(UIInventoryItem uiItem);
    public static event OnItemClicked onItemClicked;

    public void Awake()
    {
        ResetData();
    }

    public ItemData GetItemData()
    {
        return itemData;
    }

    public void SetData(ItemData itemData)
    {
        this.itemData = itemData;
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = itemData.GetSprite();
    }

    public void ResetData()
    {
        this.itemData = null;
        itemImage.sprite = null;
        itemImage.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.childCount > 0 && itemData != null)
        {
            if (eventData.clickCount == 1)
            {
                onItemClicked?.Invoke(this);
            }
            else if(eventData.clickCount == 2)
            {
                EquipmentController.Instance.Equip(itemData);
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        Transform parent = draggableItem.originalParent;
        UIInventoryItem uiItem = parent.GetComponent<UIInventoryItem>();
        UIEquipmentItem uiEquipment = parent.GetComponent<UIEquipmentItem>();

        //Si drag n drop entre éléments de l'inventaire. Case vide NOK
        if (uiItem != null && itemData != null)
        {
            if (transform.childCount > 0 && itemData != null)
            {
                onItemDroppedOn?.Invoke(this, null);
            }
        }

        //Si drag n drop provenant de l'equipement. Case vide OK
        if (uiEquipment != null)
        {
            //Sur case vide
            if (itemData == null)
            {
                onItemDroppedOn?.Invoke(null, draggableItem);
            }
            else
            {
                onItemDroppedOn?.Invoke(this, draggableItem);
            }
        }
    }
}
