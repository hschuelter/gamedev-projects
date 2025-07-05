using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionOption : MonoBehaviour, ISelectHandler
{
    public BattleManager battleManager;
    public SpellData spellData;

    [HideInInspector] public ActionMenu actionMenu;

    public virtual void SetInfo() { }

    public void OnSelect(BaseEventData eventData)
    {
        actionMenu.MoveCursorTo(this);
        if (actionMenu != null)
            actionMenu.lastAction = this;
    }

}
