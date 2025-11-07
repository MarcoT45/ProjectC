using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoostOutOfCombatPassive", menuName = "ScriptableObjects/Items/Passives/SpeedBoostOutOfCombat")]
public class SpeedBoostOutOfCombatPassive : EquipmentPassive
{
    public SpriteRenderer icon;

    [TextArea]
    public string description;

    [Range(0f, 10f)] public float speedMultiplier = 1f;

    public override SpriteRenderer GetIcon()
    {
        return icon;
    }

    public override string GetDescription()
    {
        return description;
    }

    public override void ApplyEffect(GameObject user, EquipmentTriggerType trigger, GameObject context, float value = 0)
    {
        // Augmente la vitesse du joueur lorsqu'il n'est pas en combat
        if (trigger == EquipmentTriggerType.OnExitCombat)
        {
            PlayerStats playerStats = user.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.totalStats.spd *= speedMultiplier;
                Debug.Log($"SpeedBoostOutOfCombatPassive: Increased speed by {speedMultiplier} out of combat.");
            }
        }

        // Restaure la vitesse du joueur lorsqu'il entre en combat
        if (trigger == EquipmentTriggerType.OnEnterCombat)
        {
            PlayerStats playerStats = user.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.totalStats.spd /= speedMultiplier;
                Debug.Log($"SpeedBoostOutOfCombatPassive: Increased speed by {speedMultiplier} out of combat.");
            }
        }
    }

}
