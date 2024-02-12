using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
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

    public Sprite GetSprite()
    {
        return sprite;
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