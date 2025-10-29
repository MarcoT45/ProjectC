using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LifeStealPassive", menuName = "ScriptableObjects/Items/Passives/LifeSteal")]
public class LifeStealPassive : EquipmentPassive
{
    public SpriteRenderer icon;

    [TextArea]
    public string description;

    [Range(0f, 1f)] public float lifeStealPercentage = 0.1f;

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
        if (trigger == EquipmentTriggerType.OnHit && context != null)
        {
            float lifeToHeal = value * lifeStealPercentage;
            //Arrondir au nombre entier au-dessus
            lifeToHeal = Mathf.Round(lifeToHeal);
            user.GetComponent<PlayerStats>()?.Heal(lifeToHeal);

            Debug.Log($"LifeStealPassive: Healed {lifeToHeal} health. Damaged {value}");
        }
    }

}
