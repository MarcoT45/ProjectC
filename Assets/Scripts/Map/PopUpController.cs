using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpController : MonoBehaviour {

    public void ClosePopUp() {
        GameObject map;
        map = GameObject.Find("Map Generator");
        MapGeneratorController mapGenerator = (MapGeneratorController) map.GetComponent(typeof(MapGeneratorController));
        mapGenerator.ClosedPopUpUpdate();
        this.gameObject.SetActive(false);
    }

}