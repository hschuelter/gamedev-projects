using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionOption : MonoBehaviour, ISelectHandler
{
    public BattleManager battleManager;
    public SpellData spellData;
    public CommandData commandData;

    [HideInInspector] public ActionMenu actionMenu;
    [HideInInspector] public string description;

    public virtual void SetInfo() {
        description = commandData.description;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (description == "") SetInfo();
        battleManager.UpdateCommandDescription(description);

        actionMenu.MoveCursorTo(this);
        if (actionMenu != null)
            actionMenu.lastAction = this;
    }

}
