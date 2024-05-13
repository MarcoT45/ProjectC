using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuController : MonoBehaviour {

    public TextMeshProUGUI coinNumberText;

    private void Start() {
        coinNumberText.text = GameManager.Instance.GetPlayerCoins().ToString();
    }

    private void Update() {
        
    }

}