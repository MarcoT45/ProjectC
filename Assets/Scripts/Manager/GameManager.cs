using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance = null;
    public static GameManager Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
  
    }
    #endregion

    // En haut le singleton 
    // En bas la partie jeu

    [SerializeField] int startCoins;
    private int playerCoins;
    [SerializeField] private Catalog catalog;
    [SerializeField] private Bestiary bestiary;
    public GameObject player {  get; private set; }

    public void Start()
    {
        playerCoins = startCoins;
        player = GameObject.FindWithTag("Player");
    }

    public List<ItemData> GetAllItems()
    {
        return this.catalog.GetAllItems();
    }

    public List<MonsterData> GetAllMonsters()
    {
        return this.bestiary.GetAllMonsters();
    }

    public int GetPlayerCoins()
    {
        return playerCoins;
    }

    public void SetPlayerCoins(int coins)
    {
        playerCoins = coins;
    }

    public void AddCoins(int number)
    {
        this.playerCoins += number;
    }

    public void ResetCoins(int number)
    {
        this.playerCoins = 0;
    }

    public void NewRun()
    {
    }
}
