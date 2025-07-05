using Unity.VisualScripting;
using UnityEngine;

public class Spell
{
    public SpellData spellData;

    [HideInInspector] public int baseDamage;
    [HideInInspector] public int manaCost;
    [HideInInspector] public string title;

    public Spell(SpellData spellData)
    {
        this.spellData = spellData;
        this.baseDamage = spellData.baseDamage;
        this.manaCost = spellData.manaCost;
        this.title = spellData.title;
    }

}
