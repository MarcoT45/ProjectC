using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemInfoShopController : MonoBehaviour {

    private ItemData info;
    private bool sellable = true;
    private bool equip = false;

    public GameObject sprite;
    public GameObject equipText;
    public GameObject price;
    public GameObject priceCurrency;
    public GameObject sold;

    public ItemData GetItemInfo() {
        return info;
    }

    public void SetItemInfo(ItemData i) {
        info = i;
    }

    public bool GetSellableInfo() {
        return sellable;
    }

    public void SetSellableInfo(bool s) {
        sellable = s;

        if(!s) {
            sprite.SetActive(false);
            equipText.SetActive(false);
            price.SetActive(false);
            priceCurrency.SetActive(false);
            sold.SetActive(true);
        }
    }

    public bool GetEquip() {
        return equip;
    }

    public void SetEquip(bool e) {
        equip = e;

        if(e) {
            equipText.SetActive(true);
        }
    }

}