using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
    [SerializeField] public List<DamageResistance> resistances = new List<DamageResistance>();
    public bool isCharge { get; set; }
    public bool isReckless { get; set; }

    public StatsData statsData;

    public bool isGuarding = false;
    public int currentExp = 0;
    public PartyMemberHUDManager characterHUDManager;
    public Animator animatorController;
    public TargetSelectionManager targetSelectionManager;
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
        this.resistances = statusData.resistances;

        if (animatorController != null)
        {
            animatorController.SetFloat("healthPercentage", currentHealth / maxHealth);
            animatorController.SetInteger("healthValue", (int)Mathf.Floor(currentHealth));
        }
    }

    public void GainExp(int expGained)
    {
        Debug.Log($"Implement the EXP gained in here");
    }

    public void LevelUp()
    {
        this.level++;
        this.currentHealth += 2;
        this.maxHealth += 2;
        this.currentMana += 2;
        this.maxMana += 2;
        this.attack += 2;
        this.defense += 2;
        this.magicAttack += 2;
        this.magicDefense += 2;
        this.speed += 2;
    }
    public void UpdateExp(int exp)
    {
        currentExp = exp;
    }

    public void Damage(float damage, bool isCritical = false, bool isMiss = false)
    {
        if (isMiss)
        {
            StartCoroutine(SetMiss(0.40f));
            return;
        }
        currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth);
        AnimateDamageTaken(damage, 0.40f, isCritical, isMiss);
    }
    public void Heal(float value, bool isCritical = false)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0f, maxHealth);
        AnimateHeal(value, isCritical);
    }

    public void Attack(Stats target, GameObject vfxPrefab, ActionType actionType)
    {
        int baseDamage = 1;
        var damageType = DamageType.Physical;
        Damage damage = DamageManager.Instance.CalculateDamage(this, target, baseDamage, damageType, isMagic: false, actionType: actionType);
        target.Damage(damage.value, damage.isCritical, damage.isMiss);

        if (animatorController != null)
            animatorController.SetBool("isAttack", true);

        StartCoroutine(ShowVFX(vfxPrefab, target.transform.position, 0.40f));
    }

    public void Magic(Stats target, Spell spell, ActionType actionType)
    {
        spell.Cast(this, target, actionType);
        currentMana -= spell.manaCost;

        if (animatorController != null)
            animatorController.SetBool("isMagic", true);

        StartCoroutine(ShowVFX(spell.vfxPrefab, target.transform.position, 0.40f));
    }

    public void Guard()
    {
        this.isGuarding = true;

        if (animatorController != null)
            animatorController.SetBool("isGuard", true);
    }

    public void Rest()
    {
        this.isReckless = false;
        this.isCharge = false;
    }

    public void UseItem(Stats target, Item item)
    {
        item.Use(target);
        InventoryManager.Instance.UseItem(item);
        StartCoroutine(ShowVFX(item.vfxPrefab, target.transform.position, 0.40f));

        if (animatorController != null) animatorController.SetBool("isItem", true);
    }

    /* Animations */

    public void SetWalking(bool value)
    {
        animatorController.SetBool("isWalking", value);
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
        //Debug.Log($"{this.name} -> Move front");
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

    public IEnumerator SetMiss(float time)
    {
        StartCoroutine(ShowDamage(0, 0.40f, isMiss: true));
        yield return delay(time);
        MoveBack();
        yield return delay(time);
        MoveFront();
    }

    public void AnimateDamageTaken(float damage, float time, bool isCritical = false, bool isMiss = false)
    {
        StartCoroutine(SetHit(true, 0.32f));
        StartCoroutine(SetHit(false, time + 0.40f));

        StartCoroutine(ShowDamage(damage, 0.40f, isCritical, isMiss));
    }
    public void AnimateHeal(float damage, bool isCritical = false)
    {
        StartCoroutine(ShowDamage(-damage, 0.40f, isCritical));
    }

    IEnumerator delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
    IEnumerator ShowDamage(float damage, float seconds, bool isCritical = false, bool isMiss = false)
    {
        yield return new WaitForSeconds(seconds);
        DamageNumbersManager.Instance.ShowDamage(damage, this.transform.position, isCritical, isMiss);
    }
    IEnumerator ShowVFX(GameObject vfxPrefab, Vector3 position, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        VFXManager.Instance.ShowVFX(vfxPrefab, position);
    }

    void OnMouseOver()
    {
        if (currentHealth == 0) return;
        targetSelectionManager.SelectOnMouseHover(gameObject.name);
    }

    void OnMouseExit()
    {
        targetSelectionManager.MouseExit();
    }
}
