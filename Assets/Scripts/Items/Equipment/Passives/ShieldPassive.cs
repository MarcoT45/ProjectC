using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldPassive", menuName = "ScriptableObjects/Items/Passives/Shield")]
public class ShieldPassive : EquipmentPassive
{
    public SpriteRenderer icon;
    [TextArea]
    public string description;
    public float shieldAmount = 10f;
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
        return;
    }
}
