using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CharacterStats : IEnumerable<KeyValuePair<string, float>> // Ajout de l'implémentation de l'interface IEnumerable
{

    [Header("Stats")]
    public float pv;
    public float atk;
    public float def;
    public float spd;
    public float crit;
    public float poids;
    public float luck; // crit pour pièces en %

    //A voir si on garde le bouclier commme stat visible ou pas
    public float shield;

    public void Clear()
    {
        pv = 0;
        atk = 0;
        def = 0;
        spd = 0;
        crit = 0;
        poids = 0;
        luck = 0;
        shield = 0;
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
        shield += statsToAdd.shield;
    }

    public static readonly string[] StatNames = { "HP", "ATK", "DEF", "SPD", "CRIT", "WGHT", "LUCK", "SHLD" };

    public int Count => StatNames.Length;

    public float this[int index]
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
                7 => shield,    
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
                case 7:
                    shield = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("Invalid stat index");
            }
        }
    }

    // Permet: foreach (var kv in stats) { var name = kv.Key; var value = kv.Value; }
    public IEnumerator<KeyValuePair<string, float>> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return new KeyValuePair<string, float>(StatNames[i], this[i]);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public float[] ToArray()
    {
        var a = new float[Count];
        for (int i = 0; i < Count; i++)
        {
            a[i] = this[i];
        }
        return a;
    }

}