using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public Party playerParty;
    public Party enemyParty;

    [SerializeField] public HUDManager hudManager;
    [SerializeField] private TargetSelectionManager targetSelectionManager;
    [SerializeField] private EncounterController encounterController;
    [SerializeField] private ExpMultiplierController expMultiplierController;
    [SerializeField] private VFXManager vfxManager;
    [SerializeField] private ChargeWindow chargeWindow;

    [SerializeField] private SpellData fireSpellData;

    float ANIMATION_DELAY_TIME = 1f;
    float SHORT_DELAY_TIME = 0.5f;

    [HideInInspector] public List<Stats> partyList;
    [HideInInspector] public Action currentAction;
    private Action lastAction = new Action();

    public List<Action> roundQueue { get; set; }
    private int partyIterator;
    private bool isGameOver = false;
    private bool isMagicSubmenu = false;
    private bool isItemSubmenu = false;
    private float _expMultiplier = 1.0f;
    private int _charge = 2;

    void Start()
    {
        partyIterator = 0;
        partyList = new List<Stats>();
        BattleStart();
    }

    public void BattleStart()
    {
        _charge = 2;
        chargeWindow.UpdateUI(_charge);
        playerParty.CreateParty();
        NextBattle();
    }
    public void NextBattle()
    {
        if (!encounterController.IsNext()) return;

        MusicPlayerController.Instance.PlayMusic((int)TrackNumber.BattleTheme);

        ResetPartyState(true);
        expMultiplierController.NotifySingleRound(true);
        roundQueue = new List<Action>();
        CreateEnemyParty(encounterController.NextEncounter());
        hudManager.ShowBattleStart();

        StartCoroutine(BattleStartAnimations());
    }

    public void AddPartyMember(RewardsController _rewardsController)
    {
        if (playerParty.partyMembers.Count == 4) return;

        var character = _rewardsController.GetRandomCharacter(playerParty.partyCharacters);
        playerParty.AddCharacter(character, true);
    }

    public IEnumerator BattleStartAnimations()
    {
        playerParty.transform.position = new Vector3(-1.6f, 0.4f, 0);
        hudManager.UpdateHUDAll(playerParty.partyHUD);
        hudManager.UpdateMapWindow(encounterController.encounterNumber);
        isGameOver = false;
        isMagicSubmenu = false;
        isItemSubmenu = false;

        partyList.ForEach(character => character.SetWalking(true));
        playerParty.transform.DOMoveX(-0.8f, 1);
        yield return delay(1f);
        partyList.ForEach(character => character.SetWalking(false));
        yield return delay(0.25f);
        partyList.First().MoveFront();
        hudManager.ShowBattleUI();
    }

    public void ShowMapWindow()
    {
        hudManager.ShowMapWindow();
    }

    private void CreateEnemyParty(List<Character> enemies)
    {
        enemyParty.partyCharacters = enemies;
        enemyParty.CreateParty(false);
    }

    private void Update()
    {
        HandleCancelButton();
        HandleChargeButtons();
    }

    private void HandleCancelButton()
    {
        if (!Input.GetButtonDown("Cancel") && !Input.GetMouseButtonDown(1)) return;

        if (!targetSelectionManager.isSelectingEnemy && !isMagicSubmenu && !isItemSubmenu && roundQueue.Count > 0)
        {
            var command = roundQueue.Last();
            var currentCharacter = partyList.First();
            var lastCharacter = command.user;

            partyList.Insert(0, lastCharacter);
            roundQueue.RemoveAt(roundQueue.Count - 1);

            currentCharacter.MoveBack();
            lastCharacter.MoveFront();
        }

        else if (!targetSelectionManager.isSelectingEnemy && isMagicSubmenu && !isItemSubmenu)
        {
            hudManager.ShowMagicSubMenu(false);
            hudManager.EnableActionMenu();
            hudManager.ShowActionMenu(true);
            isMagicSubmenu = false;
            isItemSubmenu = false;
        }

        else if (!targetSelectionManager.isSelectingEnemy && !isMagicSubmenu && isItemSubmenu)
        {
            hudManager.ShowItemSubMenu(false);
            hudManager.EnableActionMenu();
            hudManager.ShowActionMenu(true);
            isMagicSubmenu = false;
            isItemSubmenu = false;
        }

        else if (targetSelectionManager.isSelectingEnemy)
        {
            targetSelectionManager.DisableComponent();

            if (isMagicSubmenu) hudManager.EnableSubActionMenu();
            else if (isItemSubmenu) hudManager.EnableButtons(hudManager.itemSubMenu);
            else
            {
                hudManager.EnableActionMenu();
                hudManager.ShowActionMenu(true);
            }

            partyList.Insert(0, currentAction.user);
        }
    }

    private void HandleChargeButtons()
    {
        if (isGameOver || targetSelectionManager.isSelectingEnemy) return;

        if (Input.GetKeyDown(KeyCode.E)) ChargeUp();
        if (Input.GetKeyDown(KeyCode.Q)) ChargeDown();
    }


    private void ChargeUp()
    {
        _charge = Math.Clamp(_charge + 1, 2, 4);
        chargeWindow.UpdateUI(_charge);
    }
    private void ChargeDown()
    {
        _charge = Math.Clamp(_charge - 1, 2, 4);
        chargeWindow.UpdateUI(_charge);
    }
    private void ResetChargeBar()
    {
        _charge = 2;
        chargeWindow.UpdateUI(_charge);
    }

    private void EnableTargetSelection(Party party)
    {
        targetSelectionManager.Enable();
        targetSelectionManager.SetTargets(party.partyMembers.Where(c => c.currentHealth > 0 && !c.isCharge).ToList());
        //hudManager.ShowMagicSubMenu(false);
        hudManager.DisableActionMenu();
        hudManager.DisableSubActionMenu();
        hudManager.DisableButtons(hudManager.itemSubMenu);
    }

    public void ConfirmAction(Action action)
    {
        currentAction = action;

        partyIterator++;
        partyList.RemoveAt(0);

        EnableTargetSelection(enemyParty);
        targetSelectionManager.InvertCursor(false);
    }
    public void ConfirmAction(Action action, Party party, bool invert)
    {
        /* Item & Magic Commands -> can target party */
        currentAction = action;

        partyIterator++;
        partyList.RemoveAt(0);

        EnableTargetSelection(party);
        targetSelectionManager.InvertCursor(invert);
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
        if (partyList.Count == 0)
            StartCoroutine(ExecuteBattleRound());
        else
            partyList.First().MoveFront();
    }

    public void SkipTurn()
    {
        StartCoroutine(ExecuteBattleRound());
    }

    public void ConfirmTarget(List<Stats> targetParty, int targetPosition)
    {
        currentAction.user.MoveBack();

        currentAction.SetTarget(targetParty.ElementAt(targetPosition));
        currentAction.SetTargetParty(targetParty);

        hudManager.EnableActionMenu();
        //hudManager.DisableActionMenu();
        hudManager.ShowMagicSubMenu(false);
        hudManager.ShowItemSubMenu(false);
        isMagicSubmenu = false;
        isItemSubmenu = false;

        roundQueue.Add(currentAction);

        if (partyList.Count == 0)
            StartCoroutine(ExecuteBattleRound());
        else
            partyList.First().MoveFront();

        ResetChargeBar();
    }
    public void ConfirmAttackAction()
    {
        var currentStats = partyList.First();
        hudManager.ShowMagicSubMenu(false);
        var attackAction = new ActionAttack(currentStats);
        attackAction.actionType = (ActionType) _charge;
        ConfirmAction(attackAction);
    }

    public void ConfirmGuardAction()
    {
        var currentStats = partyList.First();
        ConfirmAction(new ActionGuard(currentStats), currentStats);
    }
    public void ConfirmRestAction()
    {
        var currentStats = partyList.First();
        ConfirmAction(new ActionRest(currentStats), currentStats);
    }

    public void ConfirmMagicAction()
    {
        var currentStats = partyList.First();
        var curentCharacter = playerParty.GetCharacter(currentStats);
        hudManager.LoadMagicOptions(curentCharacter.magicList);
        hudManager.ShowMagicSubMenu(true);
        isMagicSubmenu = true;
        isItemSubmenu = false;

        var actionMagic = new ActionMagic(currentStats);
        actionMagic.actionType = (ActionType) _charge;
        currentAction = actionMagic;
        hudManager.DisableActionMenu();
        hudManager.ShowActionMenu(false);
    }

    public void ConfirmItemAction()
    {
        var currentStats = partyList.First();
        hudManager.ShowItemSubMenu(true);
        isMagicSubmenu = false;
        isItemSubmenu = true;

        var actionItem = new ActionItem(currentStats);
        currentAction = actionItem;

        hudManager.DisableActionMenu();
        hudManager.ShowActionMenu(false);
        hudManager.LoadOptions();
        //ConfirmAction(new ActionItem(currentStats), playerParty);
    }

    public IEnumerator ExecuteBattleRound()
    {
        hudManager.ShowDescription(false);
        hudManager.ShowActionMenu(false);
        chargeWindow.ShowWindow(false);
        AddEnemyActions();
        //SortRoundQueue();

        bool playersAlive = false, enemiesAlive = false;

        while (roundQueue.Count > 0 && !isGameOver)
        {
            var currAction = roundQueue.First();
            roundQueue.RemoveAt(0);

            hudManager.UpdateDescription(currAction.description);
            hudManager.ShowDescription(true);

            /* ANIMATIONS */
            currAction.user.ActionAnimation();

            currAction.Execute();

            if (currAction.target.characterHUDManager != null)
            {
                // Enemy Attacking
                hudManager.UpdateHUD(currAction.target.characterHUDManager);
                if (DamageManager.Instance.LastHitRoll > 1 && DamageManager.Instance.LastModifier > 0) expMultiplierController.NotifyUntouched(false);

            }

            if (currAction.user.characterHUDManager != null)
            {
                // Player Attacking
                hudManager.UpdateHUD(currAction.user.characterHUDManager);
                if (currAction.actionName != "Rest") lastAction = currAction;

                if (currAction.actionType == ActionType.Charge) expMultiplierController.NotifyChargedAttack(true);
                if (currAction.actionType == ActionType.Reckless) expMultiplierController.NotifyRecklessAttack(true);
                if (DamageManager.Instance.LastModifier > 1f) expMultiplierController.NotifyWeakness(true);
            }

            roundQueue.RemoveAll(action => action.user.currentHealth <= 0);
            
            /* MORE ANIMATIONS */
            yield return delay(ANIMATION_DELAY_TIME);
            currAction.user.Stepback();
            hudManager.ShowDescription(false);
            yield return delay(SHORT_DELAY_TIME);

            playersAlive = (playerParty.partyMembers.Where(pm => pm.currentHealth > 0).ToList().Count > 0);
            enemiesAlive = (enemyParty.partyMembers.Where(pm => pm.currentHealth > 0).ToList().Count > 0);
            isGameOver = !playersAlive || !enemiesAlive;
        }
        if (isGameOver)
        {
            GameOver();
            MusicPlayerController.Instance.PlayMusic((int)TrackNumber.VictoryTheme);
            RewardExp(playersAlive);
        }

        else
        {
            expMultiplierController.NotifySingleRound(false);
            ResetPartyState();
            hudManager.ShowActionMenu(true);
            hudManager.UpdateDescription(lastAction.actionName);
            chargeWindow.ShowWindow(true);
            if (partyList.Any())
                partyList.First().MoveFront();
            else
                SkipTurn();
        }
    }

    public void GameOver()
    {
        var playersDead = playerParty.partyMembers.Where(pm => pm.currentHealth > 0).Select(pm => pm.nickname).ToList();

        playerParty.RemoveDeadCharacters();
        partyList = partyList.Where(character => character.currentHealth > 0).ToList();
    }

    private void RewardExp(bool playersAlive)
    {
        var expGained = enemyParty.partyCharacters.Select(e => e.statsData.expGranted).Sum();
        hudManager.ShowResultsWindow(playersAlive, expGained * _expMultiplier);
        playerParty.RewardExp(expGained * _expMultiplier);
        _expMultiplier = 1.0f;
    }
    public void DoubleExp() { 
        _expMultiplier = 2.0f;
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
            attackAction.actionType = ActionType.Normal;

            roundQueue.Add(attackAction);
        }
    }
    private void SortRoundQueue()
    {
        roundQueue = roundQueue.OrderByDescending(action => action.actionName == "Guard").ThenByDescending(action => action.user.speed).ToList();
        roundQueue.RemoveAll(action => action.user.currentHealth <= 0);
    }
    public void ResetPartyState(bool isStart = false) 
    {
        partyIterator = 0;
        partyList = new List<Stats>();
        foreach (var player in playerParty.partyMembers)
        {
            player.ResetGuard();
            if (isStart) player.Rest();

            if (player.currentHealth == 0 || player.isCharge) continue;
            partyList.Add(player);
        }

        foreach (var player in playerParty.partyMembers)
        {
            if (player.isCharge)
            {
                var action = new ActionRest(player);
                action.SetTarget(player);
                roundQueue.Add(action);
            }
        }

    }
    public void UpdateCommandDescription(string description)
    {
        hudManager.UpdateDescription(description);
        hudManager.ShowDescription(true);
    }

    IEnumerator delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
