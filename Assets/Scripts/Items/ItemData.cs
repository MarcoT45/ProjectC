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
    private float attack;
    [SerializeField]
    private float defense;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float luck;
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

    public int GetNumero()
    {
        return numero;
    } 

    public string GetName()
    {
        return itemName;
    } 

    public string GetDescription()
    {
        return description;
    }

    public bool GetDiscovered()
    {
        return discovered;
    }

    public void SetDiscovered(bool d)
    {
        this.discovered = d;
    }

    public float GetAttack() {
        return this.attack;
    }

    public float GetDefense() {
        return this.defense;
    }

    public float GetSpeed() {
        return this.speed;
    }

    public float GetLuck() {
        return this.luck;
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