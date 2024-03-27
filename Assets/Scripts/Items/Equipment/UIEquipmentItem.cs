using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIEquipmentItem : MonoBehaviour, IPointerClickHandler, IDropHandler
{
    [SerializeField]
    private Image itemImage;

    private ItemData itemData;

    public void Awake()
    {
        itemImage.gameObject.SetActive(false);
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
            itemImage.gameObject.SetActive(false);
            itemImage.sprite = null;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.clickCount == 2)
        {
            EquipmentController.Instance.Unequip(itemData);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}
