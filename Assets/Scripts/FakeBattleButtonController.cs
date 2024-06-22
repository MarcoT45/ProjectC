using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FakeBattleButtonController : MonoBehaviour {

    private int randomCoinsValue;
    public TextMeshProUGUI coinsNumberWinText;

    private void Start() {
        this.randomCoinsValue = Random.Range(40, 101);
        coinsNumberWinText.text = "Vous avez gagné " + randomCoinsValue + " piéces !";
    }

    public void ReturnToMap() {
        GameManager.Instance.AddCoinsToRunPlayerCoins(randomCoinsValue);
        SceneManager.LoadScene(1);
    }

}