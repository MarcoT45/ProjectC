using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Catalog : ScriptableObject
{
    [SerializeField]
    private List<ItemData> items;

    public List<ItemData> GetAllItems()
    {
        return this.items;
    }
}
