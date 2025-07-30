using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] Color healthyColor;
    [SerializeField] Color attentionColor;
    [SerializeField] Color dangerColor;

    [SerializeField] CommandDescriptionWindow descriptionWindow;

    [SerializeField] ActionMenu actionMenu;
    [SerializeField] ActionMenuCursor actionCursor;

    [SerializeField] CommandSubMenu magicSubMenu;
    [SerializeField] ActionMenuCursor magicSubCursor;

    [SerializeField] public ActionMenu itemSubMenu;
    [SerializeField] ActionMenuCursor itemSubCursor;

    [SerializeField] ResultsWindow resultsWindow;
    [SerializeField] RewardsWindow rewardsWindow;
    [SerializeField] MapWindow mapWindow;
    [SerializeField] GameObject partyHUDWindow;
    
    [SerializeField] BattleManager battleManager;


    private void Start()
    {
        descriptionWindow.Setup();
        descriptionWindow.ShowWindow(false);
        ShowActionMenu(true);
        ShowMagicSubMenu(false);
        ShowItemSubMenu(false);
    }
    public void UpdateHUD(PartyMemberHUDManager characterHUDManager) 
    {
        characterHUDManager.UpdateHealth();
        characterHUDManager.UpdateMana();
    }

    public void UpdateHUDAll(PartyHUDManager partyHUDManager)
    {
        partyHUDManager.UpdateHUDAll();
    }

    public void UpdateDescription(string description)
    {
        descriptionWindow.UpdateText(description);
    }
    public void ShowDescription(bool value)
    {
        descriptionWindow.ShowWindow(value);
    }
    public void ShowActionMenu(bool value)
    {
        actionMenu.ShowMenu(value);
        if (value) actionMenu.SelectFirstOption();
    }

    public void ShowMagicSubMenu(bool value)
    {
        magicSubMenu.ShowMenu(value);
        if (value) magicSubMenu.SelectFirstOption();

        if (value) EnableButtons(magicSubMenu);
        else DisableButtons(magicSubMenu);
    }

    public void ShowItemSubMenu(bool value)
    {
        itemSubMenu.ShowMenu(value);
        if (value) itemSubMenu.SelectFirstOption();

        if (value) EnableButtons(itemSubMenu);
        else DisableButtons(itemSubMenu);
    }

    public void EnableButtons(ActionMenu menu)
    {
        menu.EnableAllButtons();
    }

    public void DisableButtons(ActionMenu menu)
    {
        menu.DisableAllButtons();
    }

    public void EnableActionMenu()
    {
        actionMenu.EnableAllButtons();
    }

    public void DisableActionMenu()
    {
        actionMenu.DisableAllButtons();
    }

    public void EnableSubActionMenu()
    {
        magicSubMenu.EnableAllButtons();
    }

    public void DisableSubActionMenu()
    {
        magicSubMenu.DisableAllButtons();
    }

    public void ShowResultsWindow(bool value, float expGained)
    {
        resultsWindow.ShowWindow(value, Mathf.CeilToInt(expGained));
        ShowActionMenu(false);
        partyHUDWindow.SetActive(false);
    }
    public void LoadOptions()
    {
        itemSubMenu.LoadItems(InventoryManager.Instance.GetItems());
    }

    public void ShowBattleStart()
    {
        resultsWindow.HideWindow();
        rewardsWindow.HideWindow();
        mapWindow.HideWindow();
        ShowActionMenu(false);
    }

    public void ShowBattleUI()
    {
        ShowActionMenu(true);
        partyHUDWindow.SetActive(true);
    }

    public void ShowMapWindow()
    {
        mapWindow.Load();
        mapWindow.ShowWindow();
    }
    public void UpdateMapWindow(int position)
    {
        mapWindow.position = position;
    }
    public void LoadMagicOptions(List<SpellData> spells)
    {
        magicSubMenu.LoadMagicOptions(spells);
    }
}
