using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentPassive : ScriptableObject
{
    public abstract SpriteRenderer GetIcon();
    public abstract string GetDescription();
    public abstract void ApplyEffect(GameObject user, EquipmentTriggerType trigger, GameObject context, float value = 0);
}

public enum EquipmentTriggerType
{
    OnPickup, //Quand le joueur ramasse une pièce    
    OnHit, //Quand le joueur touche un ennemi
    OnHitTaken, //Quand le joueur se fait toucher par un ennemi
    OnCrit, //Quand le joueur fait un coup critique
    OnKill, //Quand le joueur tue un ennemi
    OnDeath, //Quand le joueur meurt
    OnEnterCombat, //Quand le joueur entre en combat
    OnExitCombat, //Quand le joueur sort de combat
    OnMove, //Quand le joueur se déplace
    AlwaysActive //Passif toujours actif
}