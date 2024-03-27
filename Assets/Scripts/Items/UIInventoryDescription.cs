using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryDescription : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private TMP_Text title;

    [SerializeField]
    private TMP_Text description;

    public void Awake()
    {
        ResetDescription();
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
        this.title.text = itemData.GetName();
        this.description.text = itemData.GetDescription();
    }
}
