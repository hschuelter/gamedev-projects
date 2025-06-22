using System;
using UnityEngine;

public class MonsterStats : MonoBehaviour
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

    public bool isGuarding = false;
    public PartyMemberHUDManager characterHUDManager;
    private Animator spriteAnimator;

    private void Start()
    {
        spriteAnimator = GetComponent<Animator>();
    }

    public void Damage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth);
    }

    public void Attack(MonsterStats target)
    {
        int movePower = 40;
        int damage = CalculateDamage(target, movePower);
        target.Damage(damage);
    }

    public void Magic(MonsterStats target)
    {
        int movePower = 60;
        int damage = CalculateDamage(target, movePower);
        target.Damage(damage);
        currentMana--;
    }

    public void Guard()
    {
        this.isGuarding = true;
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
    }

    private int CalculateDamage(MonsterStats target, int movePower)
    {
        float damageConstant = ((2 * level / 5) + 2) / 50;

        float damage = damageConstant  * movePower * (attack / target.defense) + 2;
        if (target.isGuarding) damage = damage / 2;

        return (int)Math.Ceiling(damage);
    }
}
