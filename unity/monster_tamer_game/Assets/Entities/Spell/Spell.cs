using Unity.VisualScripting;
using UnityEngine;

public class Spell
{
    public SpellData spellData;

    [HideInInspector] public int baseDamage;
    [HideInInspector] public int manaCost;
    [HideInInspector] public string title;
    [HideInInspector] public bool isTargetParty;
    [HideInInspector] public DamageType damageType;
    [HideInInspector] public GameObject vfxPrefab;
    [HideInInspector] public AudioClip sfx;

    public Spell(SpellData spellData)
    {
        this.spellData = spellData;
        this.baseDamage = spellData.baseDamage;
        this.manaCost = spellData.manaCost;
        this.title = spellData.title;
        this.isTargetParty = spellData.isTargetParty;
        this.damageType = spellData.damageType;
        this.vfxPrefab = spellData.vfxPrefab;
        this.sfx = spellData.sfx;
    }

    public virtual void Cast(Stats user, Stats target, ActionType actionType, AudioClip spellSfx)
    {
        Debug.Log($"[Spell] override me...");
    }
}
