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

    [SerializeField]
    private Transform statPanel;

    [SerializeField]
    private GameObject statPrefab;

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

        ClearStats();
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

        ClearStats();

        //Boucler sur les stats et les afficher
        foreach (var stat in itemData.stats)
        {
            if (stat.Value != 0)
            {
                GameObject statGO = Instantiate(statPrefab, statPanel);
                statGO.transform.SetParent(statPanel);
                TMP_Text text = statGO.GetComponent<TMP_Text>();
                text.text = $"{stat.Key}: {stat.Value.ToString()}";
            }
        }
    }

    //Enlever les stats précédentes
    public void ClearStats()
    {
        if (statPanel.transform.childCount > 0)
        {
            foreach (Transform child in statPanel.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
