using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] GameObject descriptionWindow;
    [SerializeField] Color healthyColor;
    [SerializeField] Color attentionColor;
    [SerializeField] Color dangerColor;

    [SerializeField] ActionMenu actionMenu;
    [SerializeField] ActionMenuCursor actionCursor;

    [SerializeField] ActionMenu subActionMenu;
    [SerializeField] ActionMenuCursor subActionCursor;

    [SerializeField] ResultsWindow resultsWindow;
    [SerializeField] GameObject partyHUDWindow;

    private TMP_Text descriptionText;

    private void Start()
    {
        descriptionText = descriptionWindow.GetComponentInChildren<TMP_Text>();
        descriptionWindow.SetActive(false);
        ShowActionMenu(true);
    }
    public void UpdateHUD(PartyMemberHUDManager characterHUDManager) 
    {
        characterHUDManager.UpdateHealth();
        characterHUDManager.UpdateMana();
    }

    public void UpdateDescription(string description)
    {
        descriptionText.text = description;
    }
    public void ShowDescription(bool value)
    {
        descriptionWindow.SetActive(value);
    }
    public void ShowActionMenu(bool value)
    {
        actionMenu.ShowMenu(value);
    }

    public void ShowSubActionMenu(bool value)
    {
        subActionMenu.ShowMenu(value);
        if (value) subActionMenu.SelectFirstOption();
    }

    public void EnableActionMenu()
    {
        actionMenu.EnableAllButtons();
    }

    public void DisableActionMenu()
    {
        actionMenu.DisableAllButtons();
    }

    public void ShowResultsWindow(bool value)
    {
        resultsWindow.ShowWindow(value);

        ShowActionMenu(false);
        partyHUDWindow.SetActive(false);
    }
    public void ShowBattleStart()
    {
        resultsWindow.HideWindow();
        ShowActionMenu(true);
        partyHUDWindow.SetActive(true);
    }

}
