using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Party playerParty;
    [SerializeField] private Party enemyParty;

    [SerializeField] public HUDManager hudManager;
    [SerializeField] private TargetSelectionManager targetSelectionManager;

    [SerializeField] private SpellData fireSpellData;

    float ANIMATION_DELAY_TIME = 1f;
    float SHORT_DELAY_TIME = 0.5f;

    [HideInInspector] public Action currentAction;
    [HideInInspector] public List<Stats> partyList = new List<Stats>();
    private List<Action> roundQueue = new List<Action>();
    private bool isGameOver = false;

    void Start()
    {
        playerParty.CreateParty();
        enemyParty.CreateParty(false);

        roundQueue = new List<Action>();
        partyList = new List<Stats>();
        ResetPartyState();
        hudManager.ShowBattleStart();
    }

    public void ConfirmAction(Action action)
    {
        partyList.RemoveAt(0);
        currentAction = action;
        targetSelectionManager.isSelectingEnemy = true;
        targetSelectionManager.SetTargets(enemyParty.partyMembers.Where(c => c.currentHealth > 0).ToList());
        hudManager.ShowSubActionMenu(false);
        hudManager.DisableActionMenu();
    }
    public void ConfirmAction(Action action, Party party)
    {
        partyList.RemoveAt(0);
        currentAction = action;
        targetSelectionManager.isSelectingEnemy = true;
        targetSelectionManager.SetTargetsAlly(party.partyMembers.Where(c => c.currentHealth > 0).ToList());
        hudManager.ShowSubActionMenu(false);
        hudManager.DisableActionMenu();
    }
    public void ConfirmAction(Action action, Stats target)
    {
        partyList.RemoveAt(0);
        currentAction = action;
        currentAction.SetTarget(target);
        roundQueue.Add(currentAction);
        if (roundQueue.Count == playerParty.partyMembers.Where(c => c.currentHealth > 0).ToList().Count)
            StartCoroutine(ExecuteBattleRound());
    }

    public void ConfirmTarget(List<Stats> targetParty, int targetPosition)
    {
        currentAction.SetTarget(targetParty.ElementAt(targetPosition));
        currentAction.SetTargetParty(targetParty);
        roundQueue.Add(currentAction);

        if (roundQueue.Count == playerParty.partyMembers.Where(c => c.currentHealth > 0).ToList().Count)
            StartCoroutine(ExecuteBattleRound());
    }

    public void ConfirmAttackAction()
    {
        var currentStats = partyList.First();
        hudManager.ShowSubActionMenu(false);

        ConfirmAction(new ActionAttack(currentStats));
    }

    public void ConfirmGuardAction()
    {
        var currentStats = partyList.First();
        ConfirmAction(new ActionGuard(currentStats), currentStats);
    }

    public void ConfirmMagicAction()
    {
        var currentStats = partyList.First();
        hudManager.ShowSubActionMenu(true);

        var action = new ActionMagic(currentStats);
        currentAction = action;
        hudManager.DisableActionMenu();
    }

    public void ConfirmItemAction()
    {
        var currentStats = partyList.First();

        ConfirmAction(new ActionItem(currentStats), playerParty);
    }

    public IEnumerator ExecuteBattleRound()
    {
        hudManager.ShowActionMenu(false);
        AddEnemyActions();
        SortRoundQueue();

        bool playersAlive = false, enemiesAlive = false;

        while (roundQueue.Count > 0 && !isGameOver)
        {
            var currAction = roundQueue.First();
            roundQueue.RemoveAt(0);

            hudManager.UpdateDescription(currAction.description);
            hudManager.ShowDescription(true);

            currAction.Execute();

            if (currAction.target.characterHUDManager != null)
                hudManager.UpdateHUD(currAction.target.characterHUDManager);

            if (currAction.user.characterHUDManager != null)
                hudManager.UpdateHUD(currAction.user.characterHUDManager);

            roundQueue.RemoveAll(action => action.user.currentHealth <= 0);
            
            /* ANIMATIONS */
            currAction.user.ActionAnimation();
            yield return delay(ANIMATION_DELAY_TIME);
            currAction.user.Stepback();
            hudManager.ShowDescription(false);
            yield return delay(SHORT_DELAY_TIME);

            playersAlive = (playerParty.partyMembers.Where(pm => pm.currentHealth > 0).ToList().Count > 0);
            enemiesAlive = (enemyParty.partyMembers.Where(pm => pm.currentHealth > 0).ToList().Count > 0);
            isGameOver = !playersAlive || !enemiesAlive;
        }
        if (isGameOver) hudManager.ShowResultsWindow(playersAlive);

        else
        {
            ResetPartyState();
            hudManager.ShowActionMenu(true);
        }
    }
    private void AddEnemyActions()
    {
        var partyCopy = playerParty.partyMembers;
        foreach (var enemy in enemyParty.partyMembers)
        {
            if (enemy.currentHealth == 0) continue;

            /* Enemy AI */
            //var playerStats = partyCopy.Where(p => p.currentHealth > 0).PickRandom();
            var playerStats = partyCopy.Where(p => p.currentHealth > 0).First();
            var attackAction = new ActionAttack(enemy, playerStats);
            attackAction.SetTargetParty(partyCopy);

            roundQueue.Add(attackAction);
        }
    }
    private void SortRoundQueue()
    {
        roundQueue = roundQueue.OrderByDescending(action => action.actionName == "Guard").ThenByDescending(action => action.user.speed).ToList();
        roundQueue.RemoveAll(action => action.user.currentHealth <= 0);
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
