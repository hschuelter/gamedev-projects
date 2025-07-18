using System;
using System.Collections;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance { get; private set; }

    private float criticalHitRatio = 2f;

    private void Awake()
    {
        Instance = this;
    }

    public Damage CalculateDamage(Stats user, Stats target, int baseDamage, bool isMagic = false)
    {
        float moveMultiplier = 1f;
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
         *      * Move Multiplier =
         *          * Normal: 1
         *          * Weak: 1.5
         *          * Resists: 0.5
         *          * Null: 0
         *          * Absorbs: -1
         */

        float offensiveStat = isMagic ? user.magicAttack : user.attack;
        float defensiveStat = isMagic ? target.magicDefense : target.defense;

        float statDifference = offensiveStat - defensiveStat;
        statDifference = Mathf.Clamp(statDifference, 1, statDifference);

        float levelDifference = (10 + user.level - target.level) / 10;

        float damage = baseDamage * statDifference * levelDifference * moveMultiplier;
        if (target.isGuarding && baseDamage > 0) damage = damage / 2;

        var hitRoll = HitRoll();
        if (hitRoll == 20) damage *= criticalHitRatio;
        if (hitRoll == 1)  damage = 0;

        return new Damage((int) Math.Ceiling(damage), hitRoll == 20);
    }

    private int HitRoll() {
        return UnityEngine.Random.Range(1, 20);
    }
}
