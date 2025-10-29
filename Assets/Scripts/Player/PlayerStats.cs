using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public CharacterData baseStats; // Stats de base du personnage ( ScriptableObject )   

    [Header("Current Stats")]
    public CharacterStats totalStats; // Stats totales (base + équipement)

    private EquipmentController equipmentController;

    public int currentHealth;


    public void OnEnable()
    {
        EquipmentController.onStatsChanged += RecalculateTotalStats;
    }

    public void OnDisable()
    {
        EquipmentController.onStatsChanged -= RecalculateTotalStats;
    }

    private void Awake()
    {
        equipmentController = GetComponent<EquipmentController>();
        RecalculateTotalStats();
    }

    public void RecalculateTotalStats()
    {
        totalStats.Clear();

        // Ajouter les stats de base
        totalStats = baseStats.stats;


        // Ajouter les stats de l'équipement
        if (equipmentController != null && equipmentController.GetCurrentEquipement() != null)
        {
            foreach (var item in equipmentController.GetCurrentEquipement())
            {
                if (item != null)
                {
                    totalStats.Add(item.stats);
                }
            }
        }
    }

    public CharacterStats GetTotalStats()
    {
        return totalStats;
    }

    public void Heal(float amount)
    {
        totalStats.pv += (int)amount;
        // Optionally clamp to max health if you have a max health stat
        // totalStats.pv = Mathf.Min(totalStats.pv, maxHealth);
    }
}
