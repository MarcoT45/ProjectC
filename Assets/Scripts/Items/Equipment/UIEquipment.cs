using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEquipment : MonoBehaviour
{
    private EquipmentController equipment;

    [SerializeField]
    private UIEquipmentItem helmetSlot;
    [SerializeField]
    private UIEquipmentItem chestSlot;
    [SerializeField]
    private UIEquipmentItem bootsSlot;
    [SerializeField]
    private UIEquipmentItem weaponSlot;
    [SerializeField]
    private UIEquipmentItem accessorySlot;


    public void OnEnable()
    {
        EquipmentController.onEquipmentChanged += RefreshUI;
    }

    public void OnDisable()
    {
        EquipmentController.onEquipmentChanged -= RefreshUI;
    }

    public void RefreshUI(ItemData newEquipment, ItemData oldEquipment)
    {
        ItemType type = ItemType.Casque;
        if(newEquipment != null)
        {
            type = newEquipment.GetItemType();
        }
        else if (oldEquipment != null)
        {
            type = oldEquipment.GetItemType();
        }


        switch (type)
        {
            default:

            case ItemType.Casque:
                helmetSlot.SetData(newEquipment);
                break;

            case ItemType.Torse:
                chestSlot.SetData(newEquipment);
                break;

            case ItemType.Bottes:
                bootsSlot.SetData(newEquipment);
                break;

            case ItemType.Arme:
                weaponSlot.SetData(newEquipment);
                break;

            case ItemType.Accessoire:
                accessorySlot.SetData(newEquipment);
                break;
        }
    }
}
