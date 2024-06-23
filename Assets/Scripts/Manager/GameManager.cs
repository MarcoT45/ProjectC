using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class GameManager : MonoBehaviour {
    
    private static GameManager instance = null;
    public static GameManager Instance => instance;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // En haut le singleton 
    // En bas la partie jeu

    // Variables informations joueur
    private int startCoins = 0;
    public Catalog catalog;
    public Bestiary bestiary;

    // Variables statistiques totales du joueur
    private int statNbRunMade = 0;
    private int statNbRunWon = 0;
    private TimeSpan statShortestWinTime = new TimeSpan (99, 99, 99, 99, 99);
    private TimeSpan statLongestWinTime = new TimeSpan (0, 0, 0, 0, 0);
    private int statNbBossDefeated = 0;
    private int statNbEnnemiesDefeated = 0;
    private int statNbNormalBattleWon = 0;
    private int statNbEliteBattleWon = 0;
    private int statTotalCoinsObtained = 0;
    private int statMaxMoneyRecord = 0;
    private int statMoneySpent = 0;
    private int statNbChestOpened = 0;
    private int statNbTradeMade = 0;
    private int statNbEventEncountered = 0;

    // Variables d'une run
    private int runPlayerCoins;
    private int runTotalPlayerCoinsObtained;
    private int runMaxMoney;
    private List<ItemData> runListItemsObtained;
    private List<MonsterData> runListMonstersDefeated;
    private int runNbBossDefeated;
    private int runNbNormalBattleWon;
    private int runNbEliteBattleWon;
    private int runMoneySpent;
    private int runNbChestOpened;
    private int runNbTradeMade;
    private int runNbEventEncountered;
    private DateTime runStartingTime;
    private DateTime runEndingTime;

    public void Start() {
        // AU LANCEMENT DU GAMEMANAGER CHARGER LES DONNEES SAUVEGARDEES
        ResetRun();
    }

    public void ResetRun() {
        this.runPlayerCoins = this.startCoins;
        this.runTotalPlayerCoinsObtained = 0;
        this.runMaxMoney = 0;
        this.runListItemsObtained = new List<ItemData>();
        this.runListMonstersDefeated = new List<MonsterData>();
        this.runNbBossDefeated = 0;
        this.runNbNormalBattleWon = 0;
        this.runNbEliteBattleWon = 0;
        this.runMoneySpent = 0;
        this.runNbChestOpened = 0;
        this.runNbTradeMade = 0;
        this.runNbEventEncountered = 0;
    }

    public void UpdateGlobalStats(bool win) {
        this.statNbRunMade += 1;
        this.statNbBossDefeated += this.runNbBossDefeated;
        this.statNbEnnemiesDefeated += this.runListMonstersDefeated.Count;
        this.statNbNormalBattleWon += this.runNbNormalBattleWon;
        this.statNbEliteBattleWon += this.runNbEliteBattleWon;
        this.statMoneySpent += this.runMoneySpent;
        this.statNbChestOpened += this.runNbChestOpened;
        this.statNbTradeMade += this.runNbTradeMade;
        this.statNbEventEncountered += this.runNbEventEncountered;
        this.statTotalCoinsObtained += this.runTotalPlayerCoinsObtained;

        if (this.runMaxMoney > this.statMaxMoneyRecord) {
            this.statMaxMoneyRecord = this.runMaxMoney;
        }
        
        if (win) {
            this.statNbRunWon = this.statNbRunWon + 1;

            TimeSpan timeRun = this.runEndingTime - this.runStartingTime;
            if(timeRun < this.statShortestWinTime) {
                this.statShortestWinTime = timeRun;
            }

            if(timeRun > this.statLongestWinTime) {
                this.statLongestWinTime = timeRun;
            }
        }

        this.startCoins = (int) (this.runPlayerCoins * 0.1);
    }

    public List<ItemData> GetAllItems() {
        return this.catalog.GetAllItems();
    }

    public List<MonsterData> GetAllMonsters() {
        return this.bestiary.GetAllMonsters();
    }

    public int GetRunPlayerCoins() {
        return this.runPlayerCoins;
    }

    public void SetRunPlayerCoins(int coins) {
        this.runPlayerCoins = coins;
    }

    public void AddCoinsToRunPlayerCoins(int number) {
        this.runPlayerCoins += number;
    }

    public int GetRunTotalPlayerCoinsObtained() {
        return this.runTotalPlayerCoinsObtained;
    }

    public void SetRunTotalPlayerCoinsObtained(int coins) {
        this.runTotalPlayerCoinsObtained = coins;
    }

    public int GetRunMaxMoney() {
        return this.runMaxMoney;
    }

    public void SetRunMaxMoney(int coins) {
        this.runMaxMoney = coins;
    }

    public void AddCoinsToRunMaxMoney(int number) {
        this.runMaxMoney += number;
    }

    public List<ItemData> GetRunListItemsObtained() {
        return this.runListItemsObtained;
    }

    public void AddRunListItemsObtained(ItemData item) {
        this.runListItemsObtained.Add(item);
    }

    public List<MonsterData> GetRunListMonstersDefeated() {
        return this.runListMonstersDefeated;
    }

    public void AddRunListMonstersDefeated(MonsterData monster) {
        this.runListMonstersDefeated.Add(monster);
    }

    public int GetRunNbBossDefeated() {
        return this.runNbBossDefeated;
    }

    public void SetRunNbBossDefeated(int number) {
        this.runNbBossDefeated = number;
    }

    public void AddNumberToRunNbBossDefeated(int number) {
        this.runNbBossDefeated += number;
    }

    public int GetRunNbNormalBattleWon() {
        return this.runNbNormalBattleWon;
    }

    public void SetRunNbNormalBattleWon(int number) {
        this.runNbNormalBattleWon = number;
    }

    public void AddNumberToRunNbNormalBattleWon(int number) {
        this.runNbNormalBattleWon += number;
    }

    public int GetRunNbEliteBattleWon() {
        return this.runNbEliteBattleWon;
    }

    public void SetRunNbEliteBattleWon(int number) {
        this.runNbEliteBattleWon = number;
    }

    public void AddNumberToRunNbEliteBattleWon(int number) {
        this.runNbEliteBattleWon += number;
    }

    public int GetRunMoneySpent() {
        return this.runMoneySpent;
    }

    public void SetRunMoneySpent(int number) {
        this.runMoneySpent = number;
    }

    public void AddNumberToRunMoneySpent(int number) {
        this.runMoneySpent += number;
    }

    public int GetRunNbChestOpened() {
        return this.runNbChestOpened;
    }

    public void SetRunNbChestOpened(int number) {
        this.runNbChestOpened = number;
    }

    public void AddNumberToRunNbChestOpened(int number) {
        this.runNbChestOpened += number;
    }

    public int GetRunNbTradeMade() {
        return this.runNbTradeMade;
    }

    public void SetRunNbTradeMade(int number) {
        this.runNbTradeMade = number;
    }

    public void AddNumberToRunNbTradeMade(int number) {
        this.runNbTradeMade += number;
    }

    public int GetRunNbEventEncountered() {
        return this.runNbEventEncountered;
    }

    public void SetRunNbEventEncountered(int number) {
        this.runNbEventEncountered = number;
    }

    public void AddNumberToRunNbEventEncountered(int number) {
        this.runNbEventEncountered += number;
    }

    public void StartTimer() {
        this.runStartingTime = DateTime.Now;
    }

    public void EndTimer() {
        this.runEndingTime = DateTime.Now;
    }

    // ********** PARTIE CARTE/MAP DE RUN ********** //

    public List<Noeud> mapData = new List<Noeud>();

    public List<Noeud> GetMapData() {
        return mapData;
    }

    public void SetMapData(List<Noeud> data) {
        mapData = data;
    }

    public void ResetMapData() {
        mapData = new List<Noeud>();
    }

}
