using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SubActionOption : ActionOption
{
    public override void SetInfo()
    {
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
        Spell spell = new Spell(spellData);
        var stats = battleManager.partyList.First();

        if (stats.currentMana < spell.manaCost) return;

        var action = new ActionMagic(stats);
        action.SetSpell(spell);

        battleManager.ConfirmAction(action);
    }
}
