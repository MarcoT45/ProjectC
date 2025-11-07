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

    public float cooldownTimeRemaining;
    public float activeTimeRemaining;
    public AbilityState stateAbility = AbilityState.ready;

    public abstract void Activate(GameObject parent);
    public abstract void BeginCooldown(GameObject parent);
}
