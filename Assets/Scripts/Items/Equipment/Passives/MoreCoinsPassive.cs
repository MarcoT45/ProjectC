using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoreCoinsPassive", menuName = "ScriptableObjects/Items/Passives/MoreCoins")]
public class MoreCoinsPassive : EquipmentPassive
{
    public SpriteRenderer icon;

    [TextArea]
    public string description;

    [Range(0, 10)] public int coinsToAdd = 1;

    public override void ApplyEffect(GameObject user, EquipmentTriggerType trigger, GameObject context, float value = 0)
    {
        if (trigger == EquipmentTriggerType.OnPickup && context != null)
        {
            GameManager.Instance.AddCoinsToRunPlayerCoins(coinsToAdd);

            Debug.Log($"MoreCoinsPassive: Added {coinsToAdd} coins.");
        }
    }

    public override SpriteRenderer GetIcon()
    {
        return icon;
    }

    public override string GetDescription()
    {
        return description;
    }
}
