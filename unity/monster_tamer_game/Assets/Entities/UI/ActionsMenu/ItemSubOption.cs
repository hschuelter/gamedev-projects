using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSubOption : ActionOption
{
    public override void SetInfo()
    {
        description = commandData.description;
        var texts = GetComponentsInChildren<TMP_Text>();

        foreach (var t in texts)
        {
            if (t.name == "Label")
                t.text = commandData.title;
        }
    }

    public void SetInfo(string title)
    {
        var texts = GetComponentsInChildren<TMP_Text>();
        foreach (var t in texts)
        {
            if (t.name == "Label")
                t.text = title;
        }
    }

    public void ConfirmItem()
    {
        //Spell spell = new Spell(spellData);
        var stats = battleManager.partyList.First();

        //if (stats.currentMana < spell.manaCost) return;
        battleManager.ConfirmAction(new ActionItem(stats), battleManager.playerParty);
    }
}
