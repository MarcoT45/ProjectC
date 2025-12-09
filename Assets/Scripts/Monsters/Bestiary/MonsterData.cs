using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MonsterData", menuName = "ScriptableObjects/Monsters/MonsterData")]
public class MonsterData : ScriptableObject
{
    
    public int numero;
    
    public string monsterName;
    
    [TextArea]
    public string description;
    
    public bool discovered;
    
    public int pv;
    
    public int atk;
    
    public int def;
    
    public float speed;
    
    public Sprite sprite;

    public int GetNumero()
    {
        return numero;
    }

    public bool GetDiscovered()
    {
        return discovered;
    }

    public void SetDiscovered(bool d)
    {
        this.discovered = d;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }
}
