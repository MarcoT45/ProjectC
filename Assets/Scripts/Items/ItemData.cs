using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemData", menuName = "ScriptableObjects/Items/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField]
    private int numero;
    [SerializeField]
    private string itemName;
    [SerializeField]
    [TextArea]
    private string description;
    [SerializeField]
    private bool discovered;
    [SerializeField]
    private float pv;
    [SerializeField]
    private float atk;
    [SerializeField]
    private float def;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float critical;
    [SerializeField]
    [TextArea]
    private string descriptionEffect;
    [SerializeField]
    private int price;
    [SerializeField]
    private int rarity;
    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private ItemType type;


    public ItemData()
    {
        numero = 0;
        itemName = "Test";
    }

    public string GetName()
    {
        return itemName;
    } 

    public string GetDescription()
    {
        return description;
    }

    public int GetPrice()
    {
        return price;
    }

    public int GetRarity()
    {
        return rarity;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public ItemType GetItemType()
    {
        return type;
    }
}

public enum ItemType
{
    Casque,
    Torse,
    Bottes,
    Accessoire,
    Arme
}