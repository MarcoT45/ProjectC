using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public CharacterData baseStats; // Stats de base du personnage ( ScriptableObject )   

    [Header("Current Stats")]
    public CharacterStats totalStats; // Stats totales (base + équipement)

    public void OnEnable()
    {
        EquipmentController.onStatsChanged += RecalculateTotalStats;
    }

    public void OnDisable()
    {
        EquipmentController.onStatsChanged -= RecalculateTotalStats;
    }

    private void Start()
    {
        RecalculateTotalStats();
    }

    public void RecalculateTotalStats()
    {
        Debug.Log("Recalcule Total Stats");
        totalStats.Clear();

        // Ajouter les stats de base
        totalStats = baseStats.stats;


        // Ajouter les stats de l'équipement
        totalStats.Add(EquipmentController.Instance.GetTotalStats());


    }

    public CharacterStats GetTotalStats()
    {
        return totalStats;
    }

}
