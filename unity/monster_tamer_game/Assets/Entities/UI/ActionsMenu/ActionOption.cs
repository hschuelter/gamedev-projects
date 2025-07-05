using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionOption : MonoBehaviour, ISelectHandler
{
    public BattleManager battleManager;
    public SpellData spellData;

    [HideInInspector] public ActionMenu actionMenu;

    public void OnSelect(BaseEventData eventData)
    {
        actionMenu.MoveCursorTo(this);
        if (actionMenu != null)
            actionMenu.lastAction = this;
    }

    public void ConfirmSpell()
    {
        Spell spell = new Spell(spellData);
        var currentStats = battleManager.partyList.First();

        var action = new ActionMagic(currentStats);
        action.SetSpell(spell);

        battleManager.ConfirmAction(action);
    }

}
