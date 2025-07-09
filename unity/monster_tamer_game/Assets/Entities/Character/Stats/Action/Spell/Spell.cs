using Unity.VisualScripting;
using UnityEngine;

public class Spell
{
    public SpellData spellData;

    [HideInInspector] public int baseDamage;
    [HideInInspector] public int manaCost;
    [HideInInspector] public string title;
    [HideInInspector] public bool isTargetParty;
    [HideInInspector] public GameObject vfxPrefab;

    public Spell(SpellData spellData)
    {
        this.spellData = spellData;
        this.baseDamage = spellData.baseDamage;
        this.manaCost = spellData.manaCost;
        this.title = spellData.title;
        this.isTargetParty = spellData.isTargetParty;
        this.vfxPrefab = spellData.vfxPrefab;
    }

    public virtual void Cast(Stats user, Stats target)
    {
        Debug.Log($"override me...");
    }

}
