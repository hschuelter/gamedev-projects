using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Party playerParty;
    [SerializeField] private Party enemyParty;

    [SerializeField] public HUDManager hudManager;
    [SerializeField] private TargetSelectionManager targetSelectionManager;
    //[SerializeField] private Button atkButton;
    //[SerializeField] private ActionMenu actionMenu;

    float ANIMATION_DELAY_TIME = 1f;
    float SHORT_DELAY_TIME = 0.5f;

    private List<Action> roundQueue = new List<Action>();
    private List<Stats> partyList = new List<Stats>();
    private Action currentAction;

    void Start()
    {
        playerParty.CreateParty();
        enemyParty.CreateParty(false);

        roundQueue = new List<Action>();
        partyList = new List<Stats>();
        ResetPartyState();
    }

    public void ConfirmAction(Action action)
    {
        currentAction = action;
        targetSelectionManager.isSelectingEnemy = true;
        targetSelectionManager.SetTargets(enemyParty.partyMembers.Where(c => c.currentHealth > 0).ToList());
        hudManager.DisableActionMenu();
    }

    public void ConfirmTarget(Stats target)
    {
        currentAction.SetTarget(target);
        roundQueue.Add(currentAction);

        Debug.Log($"Round Queue: {roundQueue.Count} x {playerParty.partyMembers.Where(c => c.currentHealth > 0).ToList().Count}");
        if (roundQueue.Count == playerParty.partyMembers.Where(c => c.currentHealth > 0).ToList().Count)
            StartCoroutine(ExecuteBattleRound());
    }

    public void ConfirmAttackAction()
    {
        // Depois tirar
        var enemyStats = enemyParty.partyMembers.PickRandom();

        var currentStats = partyList.First();
        partyList.RemoveAt(0);

        ConfirmAction(new ActionAttack(currentStats));
    }

    public void ConfirmGuardAction()
    {
        // Depois tirar
        var enemyStats = enemyParty.partyMembers.PickRandom();

        var currentStats = partyList.First();
        partyList.RemoveAt(0);

        ConfirmAction(new ActionGuard(currentStats));
    }

    public void ConfirmMagicAction()
    {
        // Depois tirar
        var enemyStats = enemyParty.partyMembers.PickRandom();

        var currentStats = partyList.First();
        partyList.RemoveAt(0);

        ConfirmAction(new ActionMagic(currentStats));
    }

    public void ConfirmItemAction()
    {
        // Depois tirar
        var enemyStats = enemyParty.partyMembers.PickRandom();

        var currentStats = partyList.First();
        partyList.RemoveAt(0);

        ConfirmAction(new ActionItem(currentStats));
    }

    public IEnumerator ExecuteBattleRound()
    {
        hudManager.HideActionMenu();

        foreach (var enemy in enemyParty.partyMembers)
        {
            if (enemy.currentHealth == 0) continue;
            var partyCopy = playerParty.partyMembers;
            //var playerStats = partyCopy.Where(p => p.currentHealth > 0).PickRandom();
            var playerStats = partyCopy.Where(p => p.currentHealth > 0).First();

            if (enemy.currentHealth <= 0) continue;
            roundQueue.Add(new ActionAttack(enemy, playerStats));
        }

        roundQueue = roundQueue.OrderByDescending(action => action.actionName == "Guard").ThenByDescending(action => action.user.speed).ToList();
        roundQueue.RemoveAll(action => action.user.currentHealth <= 0);

        while (roundQueue.Count > 0)
        {
            var currAction = roundQueue.First();
            roundQueue.RemoveAt(0);

            hudManager.UpdateDescription(currAction.description);
            hudManager.ShowDescription();

            currAction.Execute();

            if (currAction.target.characterHUDManager != null)
                hudManager.UpdateHUD(currAction.target.characterHUDManager);

            if (currAction.user.characterHUDManager != null)
                hudManager.UpdateHUD(currAction.user.characterHUDManager);

            roundQueue.RemoveAll(action => action.user.currentHealth <= 0);
            currAction.user.ActionAnimation();

            // Add specific delay times for each type of action
            yield return delay(ANIMATION_DELAY_TIME);
            currAction.user.Stepback();

            hudManager.HideDescription();
            yield return delay(SHORT_DELAY_TIME);
        }

        ResetPartyState();
        hudManager.ShowActionMenu();
    }

    public void ResetPartyState() 
    {
        partyList = new List<Stats>();
        foreach (var player in playerParty.partyMembers)
        {
            player.ResetGuard();

            if (player.currentHealth == 0) continue;
            partyList.Add(player);
        }
    }

    IEnumerator delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
