using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpForetConfirmTradeButtonController : MonoBehaviour {

    public GameObject popup;

    public void PlayAction() {
        PopUpForetController pu = (PopUpForetController) popup.GetComponent(typeof(PopUpForetController));
        pu.ConfirmTrade();
    }

}