using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public class Stats : MonoBehaviour
{
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth;
    [SerializeField] public float currentMana;
    [SerializeField] public float maxMana;
    [SerializeField] public float attack;
    [SerializeField] public float defense;
    [SerializeField] public float magicAttack;
    [SerializeField] public float magicDefense;
    [SerializeField] public float speed;
    [SerializeField] public string nickname;
    [SerializeField] public int level;

    public StatsData statusData;

    public bool isGuarding = false;
    public PartyMemberHUDManager characterHUDManager;
    public Animator animatorController;
    public void UpdateStats(StatsData statusData)
    {
        this.currentHealth = statusData.currentHealth;
        this.maxHealth = statusData.maxHealth;
        this.currentMana = statusData.currentMana;
        this.maxMana = statusData.maxMana;
        this.attack = statusData.attack;
        this.defense = statusData.defense;
        this.magicAttack = statusData.magicAttack;
        this.magicDefense = statusData.magicDefense;
        this.speed = statusData.speed;
        this.nickname = statusData.nickname;
        this.level = statusData.level;

        if (animatorController != null)
        {
            animatorController.SetFloat("healthPercentage", currentHealth / maxHealth);
            animatorController.SetInteger("healthValue", (int)Mathf.Floor(currentHealth));
        }
    }

    public void Damage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth);

        if (animatorController != null)
        {
            AnimateDamageTaken(damage, 0.40f);
        }
    }

    public void Attack(Stats target)
    {
        int baseDamage = 1;
        int damage = CalculateDamage(target, baseDamage, isMagic: false);
        //Debug.Log($"[ATK] {nickname} -> {target.nickname}: {damage} dmg");
        target.Damage(damage);

        if (animatorController != null)
            animatorController.SetBool("isAttack", true);
    }

    public void Magic(Stats target, Spell spell)
    {
        int baseDamage = spell.baseDamage;
        int damage = CalculateDamage(target, baseDamage, isMagic: true);
        //Debug.Log($"[MAG] {nickname} -> {target.nickname}: {damage} dmg");
        target.Damage(damage);
        currentMana -= spell.manaCost;

        if (animatorController != null)
            animatorController.SetBool("isMagic", true);
    }

    public void Guard()
    {
        this.isGuarding = true;

        if (animatorController != null)
            animatorController.SetBool("isGuard", true);
    }

    // UseItem(Item item)
    public void UseItem(Stats target)
    {
        // item.use(this);
        //target.currentHealth = Mathf.Clamp(target.currentHealth + 20, 0f, target.maxHealth);
        target.Damage(-20);

        if (animatorController != null) animatorController.SetBool("isItem", true);

        //if (target.animatorController != null)
        //{
        //    target.animatorController.SetFloat("healthPercentage", target.currentHealth / target.maxHealth);
        //    target.animatorController.SetInteger("healthValue", (int)Mathf.Floor(target.currentHealth));
        //}
    }

    public void ActionAnimation()
    {
        var pos = gameObject.transform.position;
        var scale = gameObject.transform.localScale;
        gameObject.transform.position = new Vector3(pos.x + 0.4f * scale.x, pos.y, pos.z);

    }
    public void Stepback()
    {
        var pos = gameObject.transform.position;
        var scale = gameObject.transform.localScale;
        gameObject.transform.position = new Vector3(pos.x - 0.4f * scale.x, pos.y, pos.z);

        if (animatorController != null)
            ResetAnimator();
    }

    public void MoveFront()
    {
        var pos = gameObject.transform.position;
        var scale = gameObject.transform.localScale;
        gameObject.transform.position = new Vector3(pos.x + 0.2f * scale.x, pos.y, pos.z);
    }
    public void MoveBack()
    {
        var pos = gameObject.transform.position;
        var scale = gameObject.transform.localScale;
        gameObject.transform.position = new Vector3(pos.x - 0.2f * scale.x, pos.y, pos.z);
    }

    private void ResetAnimator()
    {
        animatorController.SetBool("isAttack", false);
        animatorController.SetBool("isMagic", false);
        animatorController.SetBool("isItem", false);
    }

    public void ResetGuard()
    {
        animatorController.SetBool("isGuard", false);
    }

    private int CalculateDamage(Stats target, int baseDamage, bool isMagic = false)
    {
        float moveMultiplier = 1f;
        /* 
         * DAMAGE = Base Damage × Stat Difference × Level Difference × Move Multiplier
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

        float offensiveStat = isMagic ? magicAttack : attack;
        float defensiveStat = isMagic ? target.magicDefense : target.defense;

        float statDifference = offensiveStat - defensiveStat;
        statDifference = Mathf.Clamp(statDifference, 1, statDifference);

        float levelDifference = (10 + this.level - target.level) / 10;

        float damage =  baseDamage * statDifference * levelDifference * moveMultiplier;
        if (target.isGuarding) damage = damage / 2;

        return (int) Math.Ceiling(damage);
    }

    public IEnumerator SetHit(bool value, float time)
    {
        yield return delay(time);
        animatorController.SetBool("isHit", value);
        if (!value)
        {
            animatorController.SetFloat("healthPercentage", currentHealth / maxHealth);
            animatorController.SetInteger("healthValue", (int)Mathf.Floor(currentHealth));
        }
    }

    public void AnimateDamageTaken(float damage, float time)
    {
        StartCoroutine(SetHit(true, 0.32f));
        StartCoroutine(SetHit(false, time + 0.40f));

        /* Damage */
        StartCoroutine(ShowDamage(damage, 0.40f));
        
    }

    IEnumerator delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
    IEnumerator ShowDamage(float damage, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DamageNumbersManager.Instance.ShowDamage(damage, this.transform.position);
    }
}
