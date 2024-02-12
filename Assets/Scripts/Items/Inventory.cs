using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<ItemData> items;

    #region  Singleton
    private static Inventory instance = null;
    public static Inventory Instance => instance;

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

    public void Start()
    {
        items = new List<ItemData> ();

        foreach (ItemData item in GameManager.Instance.GetAllItems())
        {
            items.Add(item);
        }

        Debug.Log(items.Count);
    }

    public List<ItemData> GetItemList()
    {
        return items;
    }

    public void AddItem(ItemData item){ items.Add(item); }

    public void RemoveItem(ItemData item) { items.Remove(item); }

    public void ClearItem() { items.Clear(); }
}
