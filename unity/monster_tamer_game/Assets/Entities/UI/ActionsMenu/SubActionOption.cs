using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SubActionOption : ActionOption
{
    public override void SetInfo()
    {
        description = commandData.description;
        var texts = GetComponentsInChildren<TMP_Text>();

        foreach (var t in texts)
        {
            if (t.name == "Mana Cost") 
                t.text = spellData.manaCost.ToString();
            if (t.name == "Label")
                t.text = spellData.title;

        }
    }
    public void ConfirmSpell()
    {
        Spell spell;
        if (spellData.baseDamage > 0) spell = new SpellDamage(spellData);
        else if (spellData.baseDamage < 0) spell = new SpellHeal(spellData);
        else spell = new SpellHeal(spellData); // SpellBuff

        var stats = battleManager.partyList.First();

        if (stats.currentMana < spell.manaCost) return;

        var action = new ActionMagic(stats);
        action.SetSpell(spell);

        Party party = spell.isTargetParty ? battleManager.playerParty : battleManager.enemyParty;

        battleManager.ConfirmAction(action, party, spell.isTargetParty);
    }
}
