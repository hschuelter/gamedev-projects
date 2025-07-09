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

    [SerializeField] ActionMenu subActionMenu;
    [SerializeField] ActionMenuCursor subActionCursor;

    [SerializeField] public ActionMenu itemSubMenu;
    [SerializeField] ActionMenuCursor itemsubCursor;

    [SerializeField] ResultsWindow resultsWindow;
    [SerializeField] GameObject partyHUDWindow;

    private void Start()
    {
        descriptionWindow.Setup();
        descriptionWindow.ShowWindow(false);
        ShowActionMenu(true);
        ShowSubActionMenu(false);
        ShowItemSubMenu(false);
    }
    public void UpdateHUD(PartyMemberHUDManager characterHUDManager) 
    {
        characterHUDManager.UpdateHealth();
        characterHUDManager.UpdateMana();
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
    }

    public void ShowSubActionMenu(bool value)
    {
        subActionMenu.ShowMenu(value);
        if (value) subActionMenu.SelectFirstOption();

        if (value) EnableButtons(subActionMenu);
        else DisableButtons(subActionMenu);
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
        subActionMenu.EnableAllButtons();
    }

    public void DisableSubActionMenu()
    {
        subActionMenu.DisableAllButtons();
    }

    public void ShowResultsWindow(bool value)
    {
        resultsWindow.ShowWindow(value);

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
        ShowActionMenu(true);
        partyHUDWindow.SetActive(true);
    }

}
