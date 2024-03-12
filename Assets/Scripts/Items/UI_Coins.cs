using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Coins : MonoBehaviour
{
    private Transform coinContainer;

    private void Awake()
    {
        coinContainer = transform.Find("CoinsContainer");

    }

    //Peut etre à ne pas laisser dans un Update
    private void Update()
    {
        int coins = GameManager.Instance.GetPlayerCoins();
        coinContainer.Find("Texte").GetComponent<TextMeshProUGUI>().text = coins + " G";
    }
}
