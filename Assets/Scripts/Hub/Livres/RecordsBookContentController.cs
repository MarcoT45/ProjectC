using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecordsBookContentController : MonoBehaviour {

    public TextMeshProUGUI statNbRunMade;
    public TextMeshProUGUI statNbRunWon;
    public TextMeshProUGUI statShortestWinTime;
    public TextMeshProUGUI statLongestWinTime;
    public TextMeshProUGUI statNbBossDefeated;
    public TextMeshProUGUI statNbEnnemiesDefeated;
    public TextMeshProUGUI statNbNormalBattleWon;
    public TextMeshProUGUI statNbEliteBattleWon;
    public TextMeshProUGUI statTotalCoinsObtained;
    public TextMeshProUGUI statMaxMoneyRecord;
    public TextMeshProUGUI statMoneySpent;
    public TextMeshProUGUI statNbChestOpened;
    public TextMeshProUGUI statNbTradeMade;
    public TextMeshProUGUI statNbEventEncountered;

    private void Start() {
        this.statNbRunMade.text = GameManager.Instance.GetStatNbRunMade().ToString();
        this.statNbRunWon.text = GameManager.Instance.GetStatNbRunWon().ToString();

        if (GameManager.Instance.GetStatNbRunWon() > 0) {
            this.statShortestWinTime.text = GameManager.Instance.GetStatShortestWinTime().ToString(@"mm\:ss\.ms");
        } else {
            this.statShortestWinTime.text = "--:--.---";
        }

        if (GameManager.Instance.GetStatNbRunWon() > 0) {
            this.statLongestWinTime.text = GameManager.Instance.GetStatLongestWinTime().ToString(@"mm\:ss\.ms");
        } else {
            this.statLongestWinTime.text = "--:--.---";
        }

        this.statNbBossDefeated.text = GameManager.Instance.GetStatNbBossDefeated().ToString();
        this.statNbEnnemiesDefeated.text = GameManager.Instance.GetStatNbEnnemiesDefeated().ToString();
        this.statNbNormalBattleWon.text = GameManager.Instance.GetStatNbNormalBattleWon().ToString();
        this.statNbEliteBattleWon.text = GameManager.Instance.GetStatNbEliteBattleWon().ToString();
        this.statTotalCoinsObtained.text = GameManager.Instance.GetStatTotalCoinsObtained().ToString();
        this.statMaxMoneyRecord.text = GameManager.Instance.GetStatMaxMoneyRecord().ToString();
        this.statMoneySpent.text = GameManager.Instance.GetStatMoneySpent().ToString();
        this.statNbChestOpened.text = GameManager.Instance.GetStatNbChestOpened().ToString();
        this.statNbTradeMade.text = GameManager.Instance.GetStatNbTradeMade().ToString();
        this.statNbEventEncountered.text = GameManager.Instance.GetStatNbEventEncountered().ToString();
    }

}