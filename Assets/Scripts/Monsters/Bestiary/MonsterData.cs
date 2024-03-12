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
    
    public float pv;
    
    public float atk;
    
    public float def;
    
    public float speed;
    
    public Sprite sprite;


    public Sprite GetSprite()
    {
        return sprite;
    }
}
