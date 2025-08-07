using System;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance { get; private set; }

    private float criticalHitRatio = 2f;

    public float LastModifier = 1f;
    public int LastHitRoll = 1;

    private void Awake()
    {
        Instance = this;
    }

    public Damage CalculateDamage(Stats user, Stats target, int baseDamage, DamageType damageType, bool isMagic = false, ActionType actionType = ActionType.Normal)
    {
        /* 
         * DAMAGE = Base Damage x Stat Difference x Level Difference x Move Multiplier
         *      * Base Damage = Weapon or Spell Rank
         *          * Min: 1
         *          * Max: 2
         *      * Stat Difference = Attack - Defense
         *          * Min: 1
         *          * Max: (M) Attack - (M) Defense
         *      * Level Difference = 
         *          * Each level above -> +10% damage
         *          * Each level below -> +10% damage
         *      * Resistance Multiplier =
         *          * Normal: 1
         *          * Weak: 1.5
         *          * Resists: 0.5
         *          * Null: 0
         *          * Absorbs: -1 // Not implemented yet
         */

        float offensiveStat = isMagic ? user.magicAttack : user.attack;
        float defensiveStat = isMagic ? target.magicDefense : target.defense;
        float statDifference = Mathf.Clamp(offensiveStat - defensiveStat, 1, offensiveStat - defensiveStat);
        float levelDifference = (10 + user.level - target.level) / 10;
        levelDifference = Mathf.Clamp(levelDifference, 0.1f, 2.0f);
        float resistanceMultiplier = CalculateResistances(target, damageType);

        float damage = baseDamage * statDifference * levelDifference * resistanceMultiplier ;
        
        // Action Type
        damage = damage * ((int)actionType) * 0.5f;

        // If target used a Reckless move
        if (target.isReckless) damage = damage * 1.5f;

        if (target.isGuarding && baseDamage > 0) damage = damage / 2;

        var hitRoll = HitRoll();
        if (hitRoll == 20) damage *= criticalHitRatio;
        if (hitRoll == 1)  damage = 0;

        LastHitRoll = hitRoll;

        return new Damage((int) Math.Ceiling(damage), hitRoll == 20, hitRoll == 1);
    }

    public float CalculateResistances(Stats target, DamageType damageType)
    {
        var resistance = target.resistances.Where(res => res.damageType == damageType).FirstOrDefault();
        LastModifier = resistance?.GetMultiplier() ?? 1f;
        return resistance?.GetMultiplier() ?? 1f;
    }

    private int HitRoll() {
        return UnityEngine.Random.Range(1, 21);
    }
}
