using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public enum DamageType
{
    Physical,
    Fire,
    Thunder,
    Ice,
    Heal
}

[Serializable]
public enum ResistanceLevel
{
    Immune = 0,      // 0% damage
    Resistant = 1,   // 50% damage  
    Normal = 2,      // 100% damage
    Weak = 3         // 150% damage
}

[Serializable]
public class DamageResistance
{
    public DamageType damageType;
    public ResistanceLevel resistanceLevel = ResistanceLevel.Normal;
    public float GetMultiplier()
    {
        return (int)resistanceLevel * 0.5f;
    }


}
