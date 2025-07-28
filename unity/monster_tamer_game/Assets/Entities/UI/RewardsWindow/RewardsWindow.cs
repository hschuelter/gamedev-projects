using UnityEngine;

public class RewardsWindow : MonoBehaviour
{
    [SerializeField] private RewardsController rewardsController;
    [SerializeField] private BattleManager battleManager;

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

    public void HideWindow()
    {
        gameObject.SetActive(false);
    }
}
