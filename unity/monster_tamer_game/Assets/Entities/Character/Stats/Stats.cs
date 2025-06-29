using System;
using UnityEngine;

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
    }

    public void Damage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth);
    }

    public void Attack(Stats target)
    {
        int movePower = 40;
        int damage = CalculateDamage(target, movePower);
        Debug.Log($"[ATK] {nickname} -> {target.nickname}: {damage} dmg");
        target.Damage(damage);

        if (animatorController != null)
            animatorController.SetBool("isAttack", true);
    }

    public void Magic(Stats target)
    {
        int movePower = 60;
        int damage = CalculateDamage(target, movePower);
        Debug.Log($"[MAG] {nickname} -> {target.nickname}: {damage} dmg");
        target.Damage(damage);
        currentMana--;

        if (animatorController != null)
            animatorController.SetBool("isMagic", true);
    }

    public void Guard()
    {
        this.isGuarding = true;
    }

    // UseItem(Item item)
    public void UseItem()
    {
        // item.use(this);
        currentHealth = Mathf.Clamp(currentHealth + 20, 0f, maxHealth);
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
    }

    private int CalculateDamage(Stats target, int movePower)
    {
        float damageConstant = ((2 * level / 5) + 2) / 50;

        float damage = damageConstant  * movePower * (attack / target.defense) + 2;
        if (target.isGuarding) damage = damage / 2;

        return (int)Math.Ceiling(damage);
    }
}
