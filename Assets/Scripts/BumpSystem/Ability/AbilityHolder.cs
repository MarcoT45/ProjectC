using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityHolder : MonoBehaviour
{
    //Liste des Slots d'abilities (trinkets)
    public AbilitySlot[] abilitySlots;

    //Emplacement pour le trinket
    [Serializable]
    public class AbilitySlot
    {
        public Ability ability;
        //Index pour identifier le trinket (1, 2, 3...) ControlsManager.Instance.Trinket{trinketIndex}Pressed
        public int trinketIndex;
    }

    public void OnEnable()
    {
        EquipmentController.onEquipmentChanged += HandleEquipmentChanged;
    }

    public void OnDisable()
    {
        EquipmentController.onEquipmentChanged -= HandleEquipmentChanged;
    }

    private void Start()
    {
        //Initialise les slots d'abilities
        abilitySlots = new AbilitySlot[2];
    }

    private void HandleEquipmentChanged(ItemData newItem, ItemData oldItem)
    {
        //Vérifie si l'item équipé est un trinket avec une ability
        if (newItem != null && newItem.GetItemType() == ItemType.Accessoire)
        {
            Ability newAbility = newItem.activeAbility;
            if (newAbility != null)
            {
                for(int i = (int)ItemType.Accessoire; i <= (int)ItemType.Accessoire + 1; i++)
                {
                    if(newItem.GetNumero() == EquipmentController.Instance.GetCurrentEquipement()[i].GetNumero())
                    {
                        //Ajoute l'ability au slot correspondant
                        abilitySlots[i - (int)ItemType.Accessoire] = new AbilitySlot { ability = newAbility, trinketIndex = i - (int)ItemType.Accessoire + 1 };
                        Debug.Log("Ability added to slot " + (i - (int)ItemType.Accessoire));
                        return;
                    }
                }
            }
        }
    }

    private void Update()
    {
        //if(abilitySlots.Count == 0) return;

        foreach (AbilitySlot slot in abilitySlots)
        {
            if(slot == null || slot.ability == null) continue;

            UpdateAbilityState(slot);
        }

    }

    private void UpdateAbilityState(AbilitySlot slot)
    {
        Ability ability = slot.ability;
        switch (slot.ability.stateAbility)
        {
            case Ability.AbilityState.ready:
                if (GetTrinketPressed(slot.trinketIndex))
                {
                    Debug.Log("Ability activated");
                    ability.Activate(this.gameObject);
                    ability.stateAbility = Ability.AbilityState.active;
                    ability.activeTimeRemaining = ability.activeTime;
                }
                break;

            case Ability.AbilityState.active:

                if (ability.activeTimeRemaining > 0)
                {
                    Debug.Log("Ability active time remaining: " + ability.activeTime);
                    ability.activeTimeRemaining -= Time.deltaTime;
                }
                else
                {
                    Debug.Log("Ability cooldown started");
                    ability.BeginCooldown(this.gameObject);
                    ability.stateAbility = Ability.AbilityState.cooldown;
                    ability.cooldownTimeRemaining = ability.cooldownTime;
                }
                break;

            case Ability.AbilityState.cooldown:

                if (ability.cooldownTimeRemaining > 0)
                {
                    ability.cooldownTimeRemaining -= Time.deltaTime;
                }
                else
                {
                    ability.stateAbility = Ability.AbilityState.ready;
                }
                break;
        }
    }

    // Essaye de lire ControlsManager.Instance.Trinket{index}Pressed via reflection (propriété ou champ).
    // Retourne false si ControlsManager absent ou la propriété/field non trouvée.
    private bool GetTrinketPressed(int index)
    {
        var cm = ControlsManager.Instance;
        if (cm == null) return false;

        string name = $"Trinket{index}Pressed";

        var type = cm.GetType();

        var prop = type.GetProperty(name);
        if (prop != null && prop.PropertyType == typeof(bool))
        {
            try
            {
                var val = prop.GetValue(cm);
                return (val is bool b && b);
            }
            catch { return false; }
        }

        var field = type.GetField(name);
        if (field != null && field.FieldType == typeof(bool))
        {
            try
            {
                var val = field.GetValue(cm);
                return (val is bool b && b);
            }
            catch { return false; }
        }

        // fallback : rien trouvé
        return false;
    }
}
