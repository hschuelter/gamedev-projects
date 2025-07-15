using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class CommandSubMenu : ActionMenu
{
    public Button buttonPrefab;
    public void LoadMagicOptions(List<SpellData> spells)
    {
        if (actionsList.Count == 0) Setup();

        foreach (var magicCommand in actionsList)
        {
            magicCommand.gameObject.SetActive(true);
            var commandOption = magicCommand.GetComponent<SubActionOption>();
            var spellData = commandOption.spellData;
            if (!spells.Any(spell => spell.title == spellData.title))
                magicCommand.gameObject.SetActive(false);

        }

        GetComponent<VerticalLayoutGroup>().enabled = true;
        var firstButton = actionsList.Where(action => action.gameObject.activeSelf).FirstOrDefault();
        if (firstButton != null) firstButton.GetComponent<Button>().Select();
    }
    public override void EnableAllButtons()
    {
        foreach (var item in actionsList)
        {
            item.GetComponent<Button>().interactable = true;
        }
        if (!lastAction.gameObject.activeSelf) lastAction = actionsList.Where(action => action.gameObject.activeSelf).FirstOrDefault();
        lastAction.GetComponent<Button>().Select();
        actionCursor.Activate();
        MoveCursorTo(actionsList.First());
    }
}
