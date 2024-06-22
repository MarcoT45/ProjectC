using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private int startCoins;
    [SerializeField] private Catalog catalog;
    [SerializeField] private Bestiary bestiary;

    // Variables statistiques totales du joueur
    private int statNbRunMade;
    private int statNbRunWon;
    // Type de variable temps dépend de comment le timer est géré (un float/datetime/autre ?)
    // private DateTime statShortestWinTime;
    // private DateTime statLongestWinTime;
    private int statNbBossDefeated;
    private int statNbEnnemiesDefeated;
    private int statNbNormalBattleWon;
    private int statNbEliteBattleWon;
    private int statMaxMoneyRecord;
    private int statMoneySpent;
    private int statNbChestOpened;
    private int statNbTradeMade;
    private int statNbEventEncountered;

    // Variables d'une run
    private int runPlayerCoins;
    private int runTotalPlayerCoinsObtained;
    private List<ItemData> runListItemsObtained;
    private List<MonsterData> runListMonstersDefeated;
    private int runNbBossDefeated;
    private int runNbEnnemiesDefeated;
    private int runNbNormalBattleWon;
    private int runNbEliteBattleWon;
    private int runMoneySpent;
    private int runNbChestOpened;
    private int runNbTradeMade;
    private int runNbEventEncountered;

    public void Start() {
        // AU LANCEMENT DU GAMEMANAGER CHARGER LES DONNEES SAUVEGARDEES
        ResetRun();
    }

    public void ResetRun() {
        this.runPlayerCoins = this.startCoins;
        this.runTotalPlayerCoinsObtained = 0;
        this.runListItemsObtained = new List<ItemData>();
        this.runListMonstersDefeated = new List<MonsterData>();
        this.runNbBossDefeated = 0;
        this.runNbEnnemiesDefeated = 0;
        this.runNbNormalBattleWon = 0;
        this.runNbEliteBattleWon = 0;
        this.runMoneySpent = 0;
        this.runNbChestOpened = 0;
        this.runNbTradeMade = 0;
        this.runNbEventEncountered = 0;
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
