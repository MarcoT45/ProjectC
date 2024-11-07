using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpForetBuyButtonController : MonoBehaviour {

    public GameObject popup;
    public int itemNumber;

    public void PlayAction() {
        PopUpForetController pu = (PopUpForetController) popup.GetComponent(typeof(PopUpForetController));
        pu.BuyItem(itemNumber);
    }

}