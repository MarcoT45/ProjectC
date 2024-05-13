using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpForetTradeButtonController : MonoBehaviour {

    public GameObject popup;

    public void PlayAction() {
        PopUpForetController pu = (PopUpForetController) popup.GetComponent(typeof(PopUpForetController));
        pu.MakeTrade();
    }
    
}