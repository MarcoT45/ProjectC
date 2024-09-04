using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.Text;

public class SaveManager : MonoBehaviour {

    private static SaveManager instance = null;
    public static SaveManager Instance => instance;

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
    // En bas la partie gestion

    public GameState gameState = new GameState();

    private void GetData() {
        this.gameState.startCoins = GameManager.Instance.GetStartCoins();
        this.gameState.statNbRunMade = GameManager.Instance.GetStatNbRunMade();
        this.gameState.statNbRunWon = GameManager.Instance.GetStatNbRunWon();
        this.gameState.statNbBossDefeated = GameManager.Instance.GetStatNbBossDefeated();
        this.gameState.statNbEnnemiesDefeated = GameManager.Instance.GetStatNbEnnemiesDefeated();
        this.gameState.statNbNormalBattleWon = GameManager.Instance.GetStatNbNormalBattleWon();
        this.gameState.statNbEliteBattleWon = GameManager.Instance.GetStatNbEliteBattleWon();
        this.gameState.statTotalCoinsObtained = GameManager.Instance.GetStatTotalCoinsObtained();
        this.gameState.statMaxMoneyRecord = GameManager.Instance.GetStatMaxMoneyRecord();
        this.gameState.statMoneySpent = GameManager.Instance.GetStatMoneySpent();
        this.gameState.statNbChestOpened = GameManager.Instance.GetStatNbChestOpened();
        this.gameState.statNbTradeMade = GameManager.Instance.GetStatNbTradeMade();
        this.gameState.statNbEventEncountered = GameManager.Instance.GetStatNbEventEncountered();

        this.gameState.statShortestWinTimeHours = GameManager.Instance.GetStatShortestWinTime().Hours;
        this.gameState.statShortestWinTimeMinutes = GameManager.Instance.GetStatShortestWinTime().Minutes;
        this.gameState.statShortestWinTimeSeconds = GameManager.Instance.GetStatShortestWinTime().Seconds;
        this.gameState.statShortestWinTimeMilliseconds = GameManager.Instance.GetStatShortestWinTime().Milliseconds;

        this.gameState.statLongestWinTimeHours = GameManager.Instance.GetStatLongestWinTime().Hours;
        this.gameState.statLongestWinTimeMinutes = GameManager.Instance.GetStatLongestWinTime().Minutes;
        this.gameState.statLongestWinTimeSeconds = GameManager.Instance.GetStatLongestWinTime().Seconds;
        this.gameState.statLongestWinTimeMilliseconds = GameManager.Instance.GetStatLongestWinTime().Milliseconds;

        List<ItemData> listItem = GameManager.Instance.GetAllItems();
        foreach (var i in listItem) {
            if (i.GetDiscovered()) {
                this.gameState.catalogItemObtained.Add(i.GetNumero());
            }
        }

        List<MonsterData> listMonster = GameManager.Instance.GetAllMonsters();
        foreach (var m in listMonster) {
            if (m.GetDiscovered()) {
                this.gameState.bestiaryMonsterSeen.Add(m.GetNumero());
            }
        }
    }

    private void SendData() {
        try {
            GameManager.Instance.SetStartCoins(this.gameState.startCoins);
            GameManager.Instance.SetStatNbRunMade(this.gameState.statNbRunMade);
            GameManager.Instance.SetStatNbRunWon(this.gameState.statNbRunWon);
            GameManager.Instance.SetStatNbBossDefeated(this.gameState.statNbBossDefeated);
            GameManager.Instance.SetStatNbEnnemiesDefeated(this.gameState.statNbEnnemiesDefeated);
            GameManager.Instance.SetStatNbNormalBattleWon(this.gameState.statNbNormalBattleWon);
            GameManager.Instance.SetStatNbEliteBattleWon(this.gameState.statNbEliteBattleWon);
            GameManager.Instance.SetStatTotalCoinsObtained(this.gameState.statTotalCoinsObtained);
            GameManager.Instance.SetStatMaxMoneyRecord(this.gameState.statMaxMoneyRecord);
            GameManager.Instance.SetStatMoneySpent(this.gameState.statMoneySpent);
            GameManager.Instance.SetStatNbChestOpened(this.gameState.statNbChestOpened);
            GameManager.Instance.SetStatNbTradeMade(this.gameState.statNbTradeMade);
            GameManager.Instance.SetStatNbEventEncountered(this.gameState.statNbEventEncountered);

            TimeSpan st = new TimeSpan (0, this.gameState.statShortestWinTimeHours, this.gameState.statShortestWinTimeMinutes, this.gameState.statShortestWinTimeSeconds, this.gameState.statShortestWinTimeMilliseconds);
            GameManager.Instance.SetStatShortestWinTime(st);

            TimeSpan lt = new TimeSpan (0, this.gameState.statLongestWinTimeHours, this.gameState.statLongestWinTimeMinutes, this.gameState.statLongestWinTimeSeconds, this.gameState.statLongestWinTimeMilliseconds);
            GameManager.Instance.SetStatLongestWinTime(lt);

            GameManager.Instance.UpdateAllItemDiscovered(this.gameState.catalogItemObtained);
            GameManager.Instance.UpdateAllMonsterDiscovered(this.gameState.bestiaryMonsterSeen);
        } catch (System.Exception) {
            isOpening = true;
            this.gameState = new GameState();
            SendData();
            SaveGame();
        }
    }

    public void SaveGame() {
        GetData();
        string gameStateData = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonUtility.ToJson(this.gameState)));
        string filePath = Application.persistentDataPath + "/ConeyCatchingSaveData.json";
        System.IO.File.WriteAllText(filePath, gameStateData);
    }

    public void LoadGame() {
        string filePath = Application.persistentDataPath + "/ConeyCatchingSaveData.json";
        try {
            string gameStateData = Encoding.UTF8.GetString(Convert.FromBase64String(System.IO.File.ReadAllText(filePath)));
            if (IsValidJson(gameStateData)) {
                this.gameState = JsonUtility.FromJson<GameState>(gameStateData);
                SendData();
            } else {
                isOpening = true;
                this.gameState = new GameState();
                SendData();
                SaveGame();
            }
        } catch (System.Exception) {
            isOpening = true;
            this.gameState = new GameState();
            SendData();
            SaveGame();
        }
    }

    private bool IsValidJson(string jsonString) {
        try {
            JsonUtility.FromJsonOverwrite(jsonString, new object());
            return true;
        } catch (System.Exception) {
            return false;
        }
    }

    // Pour ouvrir la petite pop-up qui notifie le joueur que sa sauvegarde est corrompue
    public GameObject popUpCorrupted;
    private bool isOpening = false;

    private void FixedUpdate() {

        if(isOpening) {
            Vector3 targetAngle = new Vector3(0, 0, 0);

            if (Vector3.Distance(popUpCorrupted.transform.eulerAngles, targetAngle) > 0.01f) {
                popUpCorrupted.transform.Rotate(2.5f, 0, 0);
            } else {
                popUpCorrupted.transform.eulerAngles = targetAngle;
                isOpening = false;
            }
        }
    }

    // Sert uniquement pour la fonction reset des données du joueur sur l'écran titre dans les options
    public void ResetData() {
        this.gameState = new GameState();
        SendData();
        SaveGame();
    }

}

public class GameState {
    public int startCoins = 0;
    public List<int> catalogItemObtained = new List<int>();
    public List<int> bestiaryMonsterSeen = new List<int>();
    public int statNbRunMade = 0;
    public int statNbRunWon = 0;
    public int statShortestWinTimeHours = 23;
    public int statShortestWinTimeMinutes = 59;
    public int statShortestWinTimeSeconds = 59;
    public int statShortestWinTimeMilliseconds = 999;
    public int statLongestWinTimeHours = 0;
    public int statLongestWinTimeMinutes = 0;
    public int statLongestWinTimeSeconds = 0;
    public int statLongestWinTimeMilliseconds = 0;
    public int statNbBossDefeated = 0;
    public int statNbEnnemiesDefeated = 0;
    public int statNbNormalBattleWon = 0;
    public int statNbEliteBattleWon = 0;
    public int statTotalCoinsObtained = 0;
    public int statMaxMoneyRecord = 0;
    public int statMoneySpent = 0;
    public int statNbChestOpened = 0;
    public int statNbTradeMade = 0;
    public int statNbEventEncountered = 0;
}    