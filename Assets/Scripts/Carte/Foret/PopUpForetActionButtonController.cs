using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpForetActionButtonController : MonoBehaviour {

    public GameObject popup;

    public void PlayAction() {
        PopUpForetController pu = (PopUpForetController) popup.GetComponent(typeof(PopUpForetController));
        pu.StartAction();
    }

}