using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIEquipmentItem : MonoBehaviour, IPointerClickHandler/*, IDropHandler*/
{
    [SerializeField]
    private Image itemImage;

    private ItemData itemData;

    public delegate void OnEquipmentDrop(DraggableItem dragItem);
    public static event OnEquipmentDrop onEquipmentDrop;

    public void Awake()
    {
        itemImage.gameObject.SetActive(false);
    }

    public ItemData GetItemData()
    {
        return itemData;
    }

    public void SetData(ItemData newItem)
    {
        if(newItem != null)
        {
            this.itemData = newItem;
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = itemData.GetSprite();
        }
        else
        {
            this.itemData = null;
            itemImage.sprite = null;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2 && itemData != null)
        {
            EquipmentController.Instance.Unequip(itemData);
            itemImage.gameObject.SetActive(false);
        }
    }

    /*public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (draggableItem != null)
        {
            onEquipmentDrop?.Invoke(draggableItem);
        }
    }*/
}
