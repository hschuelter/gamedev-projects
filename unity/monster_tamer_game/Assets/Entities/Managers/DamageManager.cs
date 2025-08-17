using System;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance { get; private set; }

    [Header("Exp Multiplier")]
    public float LastModifier = 1f;
    public int LastHitRoll = 1;

    [Header("Value Modifiers")]
    [SerializeField] private float _criticalHitRatio = 2f;
    [SerializeField] private float _healRatio = 2f;
    [SerializeField] private float _recklessDamageRatio = 1.5f;
    [SerializeField] private float _guardDamageRatio = 2f;

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

        float statDifference = LinearDiff(offensiveStat, defensiveStat); 
        //float statMultiplier = SquareRootDiff(offensiveStat, defensiveStat);
        float statMultiplier = LogarithmicDiff(offensiveStat, defensiveStat);

        float levelDifference = (10f + user.level - target.level) / 10f;
        levelDifference = Mathf.Clamp(levelDifference, 0.1f, 2.0f);
        float resistanceMultiplier = CalculateResistances(target, damageType);

        float damage = 2.5f * baseDamage * statMultiplier * levelDifference * resistanceMultiplier;
        //float damage = baseDamage * statDifference * levelDifference * resistanceMultiplier;


        // Action Type
        damage = damage * ((int)actionType) * 0.5f;

        // If target used a Reckless move before
        if (target.isReckless) damage = damage * _recklessDamageRatio;

        if (damageType == DamageType.Heal) damage = damage * _healRatio;

        if (target.isGuarding && baseDamage > 0 && damageType != DamageType.Heal) damage = damage / _guardDamageRatio;

        var hitRoll = HitRoll();
        if (hitRoll == 20) damage *= _criticalHitRatio;
        if (hitRoll == 1)  damage = 0;

        LastHitRoll = hitRoll;

        // Damage Rolls
        float damageRoll = UnityEngine.Random.Range(0.8f, 1.0f);

        Debug.Log($"*************************");
        Debug.Log($"User: {user.nickname} | Target: {target.nickname}");
        Debug.Log($"baseDamage: {baseDamage}");
        Debug.Log($"statDifference: {statDifference} | statMultiplier: {statMultiplier}");
        Debug.Log($"levelDifference: {levelDifference}");
        Debug.Log($"resistanceMultiplier: {resistanceMultiplier}");
        Debug.Log($"target.isReckless: {target.isReckless}");
        Debug.Log($"actionType: {actionType}");
        Debug.Log($"damageRoll: {damageRoll}");
        Debug.Log($"damage: {damage}");
        Debug.Log($"Total Damage: {(int)Math.Ceiling(damage * damageRoll)}");
        Debug.Log($"*************************");


        return new Damage((int) Math.Ceiling(damage * damageRoll), hitRoll == 20, hitRoll == 1);
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

    private float LinearDiff(float atk, float def)
    {
        return Mathf.Clamp(atk - def, 1, atk - def);
    }

    private float SquareRootDiff(float atk, float def)
    {
        float statRatio = Mathf.Clamp(atk - def, 1, atk - def);
        float statMultiplier = Mathf.Pow(statRatio, 0.5f);
        return Mathf.Clamp(statMultiplier, 0.3f, 2.5f);
    }

    private float LogarithmicDiff(float atk, float def)
    {
        float statRatio = Mathf.Clamp(atk - def, 1, atk - def);
        float statMultiplier =  Mathf.Log(1.0f + statRatio) / Mathf.Log(2.0f);
        return Mathf.Clamp(statMultiplier, 0.3f, 2.5f);
    }
}
