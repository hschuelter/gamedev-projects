using UnityEngine;

[CreateAssetMenu(fileName = "StatsData", menuName = "Scriptable Objects/StatsData")]
public class StatsData : ScriptableObject
{
    public float currentHealth;
    public float maxHealth;
    public float currentMana;
    public float maxMana;
    public float attack;
    public float defense;
    public float magicAttack;
    public float magicDefense;
    public float speed;
    public string nickname;
    public int level;
    public int expGranted;
}
