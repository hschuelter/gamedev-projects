using UnityEngine;

public class SpellHeal : Spell
{
    public SpellHeal(SpellData spellData) : base(spellData)
    {
        this.spellData = spellData;
        this.baseDamage = spellData.baseDamage;
        this.manaCost = spellData.manaCost;
        this.title = spellData.title;
        this.isTargetParty = spellData.isTargetParty;
        this.vfxPrefab = spellData.vfxPrefab;
    }

    public override void Cast(Stats user, Stats target)
    {
        int value = DamageManager.Instance.CalculateDamage(user, target, -baseDamage, isMagic: true);
        target.Heal(value);
    }
}
