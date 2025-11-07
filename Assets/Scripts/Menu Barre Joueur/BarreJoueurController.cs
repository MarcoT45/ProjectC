using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarreJoueurController : MonoBehaviour {

    public TextMeshProUGUI nbCoinsText;
    public TextMeshProUGUI nbItemHitText;
    public GameObject spriteArme;
    public GameObject spriteAccessoireJ;
    public GameObject spriteAccessoireK;
    
    private void Update() {
        // Met à jour le nombre de pièces et les icônes des équipements
        this.nbCoinsText.text = GameManager.Instance.GetRunPlayerCoins().ToString();

        if(EquipmentController.Instance.GetArme() != null) {
            this.nbItemHitText.text = EquipmentController.Instance.GetCurrentHitCounter().ToString();
            Image a = (Image) spriteArme.GetComponent(typeof(Image));
            a.sprite = EquipmentController.Instance.GetArme().GetSprite();
            this.spriteArme.SetActive(true);
        } else {
            this.nbItemHitText.text = EquipmentController.Instance.GetCurrentHitCounter().ToString();
            this.spriteArme.SetActive(false);
        }

        if(EquipmentController.Instance.GetAccessoireJ() != null) {
            Image j = (Image) spriteAccessoireJ.GetComponent(typeof(Image));
            j.sprite = EquipmentController.Instance.GetAccessoireJ().GetSprite();
            this.spriteAccessoireJ.SetActive(true);
        } else {
            this.spriteAccessoireJ.SetActive(false);
        }

        if(EquipmentController.Instance.GetAccessoireK() != null) {
            Image k = (Image) spriteAccessoireK.GetComponent(typeof(Image));
            k.sprite = EquipmentController.Instance.GetAccessoireK().GetSprite();
            this.spriteAccessoireK.SetActive(true);
        } else {
            this.spriteAccessoireK.SetActive(false);
        }
    }

}