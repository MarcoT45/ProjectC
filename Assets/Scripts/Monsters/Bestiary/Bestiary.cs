using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bestiary", menuName = "ScriptableObjects/Monsters/Bestiary")]
public class Bestiary : ScriptableObject
{
    [SerializeField]
    private List<MonsterData> monsters;

    public List<MonsterData> GetAllMonsters()
    {
        return this.monsters;
    }
}
