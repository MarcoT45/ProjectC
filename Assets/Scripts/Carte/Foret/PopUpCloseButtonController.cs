using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpCloseButtonController : MonoBehaviour {

    public GameObject popup;

    public void ClosePopUp() {
        PopUpForetController pu = (PopUpForetController) popup.GetComponent(typeof(PopUpForetController));
        pu.UndisplayPopUp();
    }

}