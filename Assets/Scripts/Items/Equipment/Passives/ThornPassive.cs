using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ThornPassive", menuName = "ScriptableObjects/Items/Passives/Thorn")]
public class ThornPassive : EquipmentPassive
{
    public SpriteRenderer icon;

    [TextArea]
    public string description;

    [Range(0f, 1f)] public float thornPercentage = 0.1f;

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
        if (trigger == EquipmentTriggerType.OnHitTaken && context != null)
        {
            if(context != null) return;

            EnemyAI enemy = context.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                float thornDamage = value * thornPercentage;
                //Arrondir au nombre entier au-dessus
                thornDamage = Mathf.Round(thornDamage);
                enemy.Damage( (int)thornDamage );
                Debug.Log($"ThornPassive: Dealt {thornDamage} thorn damage. Damaged {value}");
            }
        }
    }
}
