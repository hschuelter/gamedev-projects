using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandSubMenu : ActionMenu
{
    public Button buttonPrefab;

    private void Awake()
    {
        Setup();
    }
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
        var commandOptions = actionsList.Where(action => action.gameObject.activeSelf).ToList();
        var firstButton = commandOptions.FirstOrDefault();
        firstButton?.GetComponent<Button>().Select();
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
