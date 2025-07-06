using NUnit.Framework;
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

    private List<Action> roundQueue { get; set; }
    private int partyIterator;
    private bool isGameOver = false;
    private bool isSubmenu = false;

    void Start()
    {
        playerParty.CreateParty();
        enemyParty.CreateParty(false);


        Debug.Log($"{playerParty.partyMembers.Count} -> {playerParty.partyMembers}");

        partyIterator = 0;
        partyList = new List<Stats>();
        roundQueue = new List<Action>();
        ResetPartyState();
        hudManager.ShowBattleStart();
    }

    private void Update()
    {
        HandleCancelButton();
    }

    private void HandleCancelButton()
    {
        if (!Input.GetButtonDown("Cancel")) return;

        if (!targetSelectionManager.isSelectingEnemy && !isSubmenu && roundQueue.Count > 0)
        {
            var command = roundQueue.Last();
            var currentCharacter = partyList.First();
            var lastCharacter = command.user;

            partyList.Insert(0, lastCharacter);
            roundQueue.RemoveAt(roundQueue.Count - 1);

            currentCharacter.MoveBack();
            lastCharacter.MoveFront();
        }

        else if (!targetSelectionManager.isSelectingEnemy && isSubmenu)
        {
            hudManager.ShowSubActionMenu(false);
            hudManager.EnableActionMenu();
            isSubmenu = false;
        }

        else if (targetSelectionManager.isSelectingEnemy)
        {
            targetSelectionManager.DisableComponent();

            if (isSubmenu) hudManager.EnableSubActionMenu();
            else hudManager.EnableActionMenu();

            partyList.Insert(0, currentAction.user);
        }
    }

    private void EnableTargetSelection(Party party)
    {
        targetSelectionManager.Enable();
        targetSelectionManager.SetTargets(party.partyMembers.Where(c => c.currentHealth > 0).ToList());
        //hudManager.ShowSubActionMenu(false);
        hudManager.DisableActionMenu();
        hudManager.DisableSubActionMenu();
    }

    public void ConfirmAction(Action action)
    {
        currentAction = action;

        partyIterator++;
        partyList.RemoveAt(0);

        EnableTargetSelection(enemyParty);
    }
    public void ConfirmAction(Action action, Party party)
    {
        /* Item Command -> can target party */
        currentAction = action;

        partyIterator++;
        partyList.RemoveAt(0);
        
        EnableTargetSelection(party);
    }
    public void ConfirmAction(Action action, Stats target)
    {
        /* Guard Command -> no need for targetting */
        currentAction = action;
        currentAction.user.MoveBack();

        partyIterator++;
        partyList.RemoveAt(0);

        currentAction.SetTarget(target);
        roundQueue.Add(currentAction);
        if (roundQueue.Count == playerParty.partyMembers.Where(c => c.currentHealth > 0).ToList().Count)
            StartCoroutine(ExecuteBattleRound());
        else
            partyList.First().MoveFront();
    }

    public void ConfirmTarget(List<Stats> targetParty, int targetPosition)
    {
        currentAction.user.MoveBack();

        currentAction.SetTarget(targetParty.ElementAt(targetPosition));
        currentAction.SetTargetParty(targetParty);

        hudManager.EnableActionMenu();
        //hudManager.DisableActionMenu();
        hudManager.ShowSubActionMenu(false);
        isSubmenu = false;

        roundQueue.Add(currentAction);

        if (roundQueue.Count == playerParty.partyMembers.Where(c => c.currentHealth > 0).ToList().Count)
            StartCoroutine(ExecuteBattleRound());
        else
            partyList.First().MoveFront();
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
        isSubmenu = true;

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
        partyIterator = 0;
        partyList = new List<Stats>();
        foreach (var player in playerParty.partyMembers)
        {
            player.ResetGuard();

            if (player.currentHealth == 0) continue;
            partyList.Add(player);
        }
        partyList.First().MoveFront();
    }
    IEnumerator delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
