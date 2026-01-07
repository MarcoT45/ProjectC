using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region Singleton
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
    #endregion

    // En haut le singleton 
    // En bas la partie jeu

    // Variables globales du jeu
    public bool GameIsPaused { get; private set; }

    // Variables informations joueur
    private int startCoins = 0;
    public Catalog catalog;
    public List<List<ItemData>> itemsTriRarete; 
    public Bestiary bestiary;

    // Variables statistiques totales du joueur
    private int statNbRunMade = 0;
    private int statNbRunWon = 0;
    private TimeSpan statShortestWinTime = new TimeSpan (0, 23, 59, 59, 999);
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
    private float runTimer;
    private bool runTimerIsActive;

    //Variables relatives au combat
    [SerializeField] private float outOfCombatTimerDuration = 2f;
    private float outOfCombatTimer = 0f;
    private bool inCombat = false;
    public bool InCombat => inCombat;

    public void Start() {
        string filePath = Application.persistentDataPath + "/ConeyCatchingSaveData.json";
        if (!System.IO.File.Exists(filePath)) {
            SaveManager.Instance.SaveGame(); // On crée une save si elle n'existe pas
        } else {
            SaveManager.Instance.LoadGame();  // On charge la save si elle existe


            UpdateItemCalalogDiscovered(1); // A enlever plus tard
            UpdateItemCalalogDiscovered(3); // A enlever plus tard
            UpdateItemCalalogDiscovered(11); // A enlever plus tard
            UpdateItemCalalogDiscovered(14); // A enlever plus tard
            UpdateItemCalalogDiscovered(20); // A enlever plus tard
            SetStartCoins(900); // A enlever plus tard
            SaveManager.Instance.SaveGame(); // A enlever plus tard


        }

        TriItemsParRarete();
        ResetRun();

        // Initialisation de DOTween
        DOTween.Init();
    }

    private void Update()
    {
        if(this.runTimerIsActive)
        {
            this.runTimer = this.runTimer + Time.deltaTime;
        }

        // Gestion du timer hors combat
        if(!inCombat) return;

        outOfCombatTimer -= Time.deltaTime;
        if(outOfCombatTimer <= 0f)
        {
            ExitCombat();
        }
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
        this.runTimer = 0f;
        this.runTimerIsActive = false;
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

    public int GetStartCoins() {
        return this.startCoins;
    }

    public void SetStartCoins(int coins) {
        this.startCoins = coins;
    }

    #region Items / Montres
    public void TriItemsParRarete()
    {
        List<ItemData> rarete1 = new List<ItemData>();
        List<ItemData> rarete2 = new List<ItemData>();
        List<ItemData> rarete3 = new List<ItemData>();
        itemsTriRarete = new List<List<ItemData>>();

        foreach (ItemData item in this.catalog.GetAllItems())
        {
            switch (item.GetRarity())
            {
                case 1:
                    rarete1.Add(item);
                    break;

                case 2:
                    rarete2.Add(item);
                    break;

                case 3:
                    rarete3.Add(item);
                    break;
                    
                 default:
                    rarete1.Add(item);
                    break;
            }
        }

        this.itemsTriRarete.Add(rarete1);
        this.itemsTriRarete.Add(rarete2);
        this.itemsTriRarete.Add(rarete3); 
    } 

    public List<ItemData> GetAllItems() {
        return this.catalog.GetAllItems();
    }

    public void UpdateItemCalalogDiscovered(int numero) {
        this.catalog.GetAllItems()[numero - 1].SetDiscovered(true);
    }

    public void UpdateAllItemDiscovered(List<int> numberList) {
        foreach (var i in this.catalog.GetAllItems()) {
            if (numberList.Contains(i.GetNumero())) {
                i.SetDiscovered(true);
            } else {
                i.SetDiscovered(false);
            }
        }
    }

    public List<MonsterData> GetAllMonsters() {
        return this.bestiary.GetAllMonsters();
    }

    public void UpdateMonsterBestiaryDiscovered(int numero) {
        this.bestiary.GetAllMonsters()[numero - 1].SetDiscovered(true);
    }

    public void UpdateAllMonsterDiscovered(List<int> numberList) {
        foreach (var m in this.bestiary.GetAllMonsters()) {
            if (numberList.Contains(m.GetNumero())) {
                m.SetDiscovered(true);
            } else {
                m.SetDiscovered(false);
            }
        }
    }
    #endregion

    #region Statistiques Globales Joueur Getter/Setter
    public int GetStatNbRunMade() {
        return this.statNbRunMade;
    }

    public void SetStatNbRunMade(int number) {
        this.statNbRunMade = number;
    }

    public int GetStatNbRunWon() {
        return this.statNbRunWon;
    }

    public void SetStatNbRunWon(int number) {
        this.statNbRunWon = number;
    }

    public TimeSpan GetStatShortestWinTime() {
        return this.statShortestWinTime;
    }

    public void SetStatShortestWinTime(TimeSpan time) {
        this.statShortestWinTime = time;
    }

    public TimeSpan GetStatLongestWinTime() {
        return this.statLongestWinTime;
    }

    public void SetStatLongestWinTime(TimeSpan time) {
        this.statLongestWinTime = time;
    }

    public int GetStatNbBossDefeated() {
        return this.statNbBossDefeated;
    }

    public void SetStatNbBossDefeated(int number) {
        this.statNbBossDefeated = number;
    }

    public int GetStatNbEnnemiesDefeated() {
        return this.statNbEnnemiesDefeated;
    }

    public void SetStatNbEnnemiesDefeated(int number) {
        this.statNbEnnemiesDefeated = number;
    }

    public int GetStatNbNormalBattleWon() {
        return this.statNbNormalBattleWon;
    }

    public void SetStatNbNormalBattleWon(int number) {
        this.statNbNormalBattleWon = number;
    }
 
    public int GetStatNbEliteBattleWon() {
        return this.statNbEliteBattleWon;
    }

    public void SetStatNbEliteBattleWon(int number) {
        this.statNbEliteBattleWon = number;
    }

    public int GetStatTotalCoinsObtained() {
        return this.statTotalCoinsObtained;
    }

    public void SetStatTotalCoinsObtained(int number) {
        this.statTotalCoinsObtained = number;
    }

    public int GetStatMaxMoneyRecord() {
        return this.statMaxMoneyRecord;
    }

    public void SetStatMaxMoneyRecord(int number) {
        this.statMaxMoneyRecord = number;
    }

    public int GetStatMoneySpent() {
        return this.statMoneySpent;
    }

    public void SetStatMoneySpent(int number) {
        this.statMoneySpent = number;
    }

    public int GetStatNbChestOpened() {
        return this.statNbChestOpened;
    }

    public void SetStatNbChestOpened(int number) {
        this.statNbChestOpened = number;
    }

    public int GetStatNbTradeMade() {
        return this.statNbTradeMade;
    }

    public void SetStatNbTradeMade(int number) {
        this.statNbTradeMade = number;
    }

    public int GetStatNbEventEncountered() {
        return this.statNbEventEncountered;
    }

    public void SetStatNbEventEncountered(int number) {
        this.statNbEventEncountered = number;
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

    public float GetRunTimer()
    {
        return this.runTimer;
    }

    public void SetRunTimer(float time)
    {
        this.runTimer = time;
    }
    public bool GetRunTimerIsActive()
    {
        return this.runTimerIsActive;
    }

    public void SetRunTimerIsActive(bool timerIsActive)
    {
        this.runTimerIsActive = timerIsActive;
    }

    #endregion

    #region Combat Management

    public void EnterCombat()
    {
        outOfCombatTimer = outOfCombatTimerDuration;

        if (!inCombat)
        {
            inCombat = true;
        }
    }

    public void ExitCombat()
    {
        inCombat = false;
    }

    #endregion

    public void PauseGame(bool paused)
    {
        GameIsPaused = paused;
        Time.timeScale = paused ? 0f : 1f; //Met en pause le temps dans le jeu
        //Quand on met en pause, on coupe aussi le son ( quand il y en aura )   
        AudioListener.pause = paused;
    }

    // ********** PARTIE CARTE/MAP DE RUN ********** //
    #region Fonctions et variables de la map

    private List<Noeud> mapData = new List<Noeud>();
    public int levelMapSizeX = 0;
    public int levelMapSizeY = 0;
    public int levelNumberEnemy = 0;

    public List<Noeud> GetMapData() {
        return mapData;
    }

    public void SetMapData(List<Noeud> data) {
        mapData = data;
    }

    public void ResetMapData() {
        mapData = new List<Noeud>();
    }

    public void SetLevelData(int mapSizeX, int mapSizeY, int numberEnemy) {
        levelMapSizeX = mapSizeX;
        levelMapSizeY = mapSizeY;
        levelNumberEnemy = numberEnemy;
    }

    public int GetLevelMapSizeX() {
        return levelMapSizeX;
    }

    public int GetLevelMapSizeY() {
        return levelMapSizeY;
    }

    public int GetLevelNumberEnemy() {
        return levelNumberEnemy;
    }

    #endregion
}
