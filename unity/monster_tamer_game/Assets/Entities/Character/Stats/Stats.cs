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
        AnimateDamageTaken(damage, 0.40f);
    }
    public void Heal(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0f, maxHealth);
        AnimateHeal(value);
    }

    public void Attack(Stats target, GameObject vfxPrefab)
    {

        int baseDamage = 1;
        int damage = DamageManager.Instance.CalculateDamage(this, target, baseDamage, isMagic: false);
        target.Damage(damage);

        if (animatorController != null)
            animatorController.SetBool("isAttack", true);

        StartCoroutine(ShowVFX(vfxPrefab, target.transform.position, 0.40f));
    }

    public void Magic(Stats target, Spell spell)
    {
        spell.Cast(this, target);
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

    public void UseItem(Stats target, Item item)
    {
        item.Use(target);
        InventoryManager.Instance.UseItem(item);
        StartCoroutine(ShowVFX(item.vfxPrefab, target.transform.position, 0.40f));

        if (animatorController != null) animatorController.SetBool("isItem", true);
    }

    /* Animations */

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

        StartCoroutine(ShowDamage(damage, 0.40f));
    }
    public void AnimateHeal(float damage)
    {
        StartCoroutine(ShowDamage(-damage, 0.40f));
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
    IEnumerator ShowVFX(GameObject vfxPrefab, Vector3 position, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        VFXManager.Instance.ShowVFX(vfxPrefab, position);
    }
}
