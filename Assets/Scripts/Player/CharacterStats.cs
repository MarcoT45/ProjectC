using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CharacterStats : IEnumerable<KeyValuePair<string, int>> // Ajout de l'implémentation de l'interface IEnumerable
{

    [Header("Stats visibles")]
    public int pv;
    public int atk;
    public int def;
    public int spd;
    public int crit;
    public int poids;
    public int luck; // crit pour pièces en %

    //Amené à évoluer => passif fait différement
  /*  [Header("Stats invisibles / passifs")]
    public int magicResist;
    public int vampirism;
    public int deflectDamage;
    public int bonusCoins; // bonus pièces fixe 
    public int spdOutOfCombat; // bonus spd hors combat*/

    public void Clear()
    {
        pv = 0;
        atk = 0;
        def = 0;
        spd = 0;
        crit = 0;
        poids = 0;
        luck = 0;
    }

    public void Add(CharacterStats statsToAdd)
    {
        pv += statsToAdd.pv;
        atk += statsToAdd.atk;
        def += statsToAdd.def;
        spd += statsToAdd.spd;
        crit += statsToAdd.crit;
        poids += statsToAdd.poids;
        luck += statsToAdd.luck;
    }

    public static readonly string[] StatNames = { "HP", "ATK", "DEF", "SPD", "CRIT", "WGHT", "LUCK" };

    public int Count => StatNames.Length;

    public int this[int index]
    {
        get
        {
            return index switch
            {
                0 => pv,
                1 => atk,
                2 => def,
                3 => spd,
                4 => crit,
                5 => poids,
                6 => luck,
                _ => throw new IndexOutOfRangeException("Invalid stat index"),
            };
        }
        set
        {
            switch (index)
            {
                case 0:
                    pv = value;
                    break;
                case 1:
                    atk = value;
                    break;
                case 2:
                    def = value;
                    break;
                case 3:
                    spd = value;
                    break;
                case 4:
                    crit = value;
                    break;
                case 5:
                    poids = value;
                    break;
                case 6:
                    luck = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("Invalid stat index");
            }
        }
    }

    // Permet: foreach (var kv in stats) { var name = kv.Key; var value = kv.Value; }
    public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return new KeyValuePair<string, int>(StatNames[i], this[i]);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int[] ToArray()
    {
        var a = new int[Count];
        for (int i = 0; i < Count; i++)
        {
            a[i] = this[i];
        }
        return a;
    }

}