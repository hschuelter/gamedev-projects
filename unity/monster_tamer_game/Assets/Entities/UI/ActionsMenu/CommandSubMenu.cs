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
        Debug.Log($"LoadMagicOptions");
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
        Debug.Log($"firstButton >{firstButton}<");
        if (firstButton != null)
        {
            /* TODO: Please fix this */
            firstButton.GetComponent<Button>().Select();
            firstButton.GetComponent<Button>().Select();
        }
    }
    public override void EnableAllButtons()
    {
        foreach (var item in actionsList)
        {
            item.GetComponent<Button>().interactable = true;
        }

        actionCursor.Activate();
        MoveCursorTo(actionsList.First());

        var availableCommands = actionsList.Where(action => action.gameObject.activeSelf).ToList();
        if (availableCommands.Count == 0) return;

        if (!lastAction.gameObject.activeSelf) lastAction = availableCommands.First();
        lastAction.GetComponent<Button>().Select();
        MoveCursorTo(actionsList.First());
    }
}
