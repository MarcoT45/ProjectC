using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UIInventoryDescription : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private TMP_Text title;

    [SerializeField]
    private TMP_Text description;

    private LocalizeStringEvent localizedStringEventTitle;
    private LocalizeStringEvent localizedStringEventDesc;

    public void Awake()
    {
        ResetDescription();
        localizedStringEventTitle = title.gameObject.GetComponent<LocalizeStringEvent>();
        localizedStringEventDesc = description.gameObject.GetComponent<LocalizeStringEvent>();
    }

    public void ResetDescription()
    {
        this.itemImage.gameObject.SetActive(false);
        this.itemImage.sprite = null;
        this.title.text = "";
        this.description.text = "";
    }

    public void SetDescription(ItemData itemData)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = itemData.GetSprite();

        string itemId = itemData.GetNumero().ToString();
        // this.title.text = itemData.GetName().GetLocalizedString();
        localizedStringEventTitle.StringReference.SetReference("EquipementTable", "Item_"+itemId+"_Name");
        //this.description.text = itemData.GetDescription().GetLocalizedString();
        localizedStringEventDesc.StringReference.SetReference("EquipementTable", "Item_" + itemId + "_Desc");
    }
}
