using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string abilityName;
    public float cooldownTime;
    public float activeTime;

    public abstract void Activate(GameObject parent);
    public abstract void BeginCooldown(GameObject parent);
}
