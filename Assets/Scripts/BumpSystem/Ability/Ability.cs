using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public enum AbilityState
    {
        ready,
        active,
        cooldown
    }

    public string abilityName;
    public float cooldownTime;
    public float activeTime;

    public float CooldownTimeRemaining { get; set; }
    public float ActiveTimeRemaining { get; set; }
    public AbilityState StateAbility { get; set; } = AbilityState.ready;

    public abstract void Activate(GameObject parent);
    public abstract void BeginCooldown(GameObject parent);
}
