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

    private int coinCount;
    [SerializeField]
    private Catalog catalog;

    public List<ItemData> GetAllItems()
    {
        return this.catalog.GetAllItems();
    }

    public void AddCoins(int number)
    {
        this.coinCount += number;
        Debug.Log(coinCount);
    }

    public void ResetCoins(int number)
    {
        this.coinCount = 0;
    }

    public void NewRun()
    {
    }
}
