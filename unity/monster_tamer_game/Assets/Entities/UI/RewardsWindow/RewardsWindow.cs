using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardsWindow : MonoBehaviour
{
    [SerializeField] private RewardsController rewardsController;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private GameObject _itemRewardsWindow;
    [SerializeField] private TMP_Text _itemLabel;

    public void NextBattle()
    {
        HideWindow();
        battleManager.ShowMapWindow();
    }

    public void AddCharacter()
    {
        battleManager.AddPartyMember(rewardsController);
        NextBattle();
    }

    public void DoubleExp()
    {
        battleManager.DoubleExp();
        NextBattle();
    }

    public void GainItems()
    {
        int amount = Random.Range(1, 5);
        InventoryManager.Instance.AddPotions(amount);
        EnableAllButtons(false);
        _itemRewardsWindow.SetActive(true);
        _itemLabel.text = $"You gained x{amount} Potions";
    }

    public void EnableAllButtons(bool value)
    {
        foreach (var button in this.GetComponentsInChildren<Button>())
        {
            button.interactable = value;
        }
        var firstReward = GetComponentsInChildren<Button>().First();
        if (battleManager.playerParty.partyMembers.Count == 4) firstReward.interactable = false;
    }
    public void ShowWindow()
    {
        EnableAllButtons(true);
        gameObject.SetActive(true);
        _itemRewardsWindow.SetActive(false);
    }

    public void HideWindow()
    {
        gameObject.SetActive(false);
        _itemRewardsWindow.SetActive(false);
    }
}
