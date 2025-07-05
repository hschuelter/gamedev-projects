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
        int movePower = 40;
        int damage = CalculateDamage(target, movePower);
        //Debug.Log($"[ATK] {nickname} -> {target.nickname}: {damage} dmg");
        target.Damage(damage);

        if (animatorController != null)
            animatorController.SetBool("isAttack", true);
    }

    public void Magic(Stats target, Spell spell)
    {
        int baseDamage = spell.baseDamage;
        int damage = CalculateDamage(target, baseDamage);
        //Debug.Log($"[MAG] {nickname} -> {target.nickname}: {damage} dmg");
        target.Damage(damage);
        currentMana--;

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
    public void UseItem()
    {
        // item.use(this);
        currentHealth = Mathf.Clamp(currentHealth + 20, 0f, maxHealth);

        if (animatorController != null)
        {
            animatorController.SetBool("isItem", true);
            animatorController.SetFloat("healthPercentage", currentHealth / maxHealth);
            animatorController.SetInteger("healthValue", (int)Mathf.Floor(currentHealth));

        }
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

    private int CalculateDamage(Stats target, int movePower)
    {
        //float damageConstant = ((2 * level / 5) + 2) / 50;

        float damage = movePower * (attack / target.defense);
        if (target.isGuarding) damage = damage / 2;

        return (int)Math.Ceiling(damage);
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
