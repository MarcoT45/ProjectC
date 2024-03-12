using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Catalog", menuName = "ScriptableObjects/Items/Catalog")]
public class Catalog : ScriptableObject
{
    [SerializeField]
    private List<ItemData> items;

    public List<ItemData> GetAllItems()
    {
        return this.items;
    }
}
