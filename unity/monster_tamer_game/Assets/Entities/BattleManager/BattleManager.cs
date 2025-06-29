using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Party playerParty;
    [SerializeField] private Party enemyParty;

    [SerializeField] private HUDManager hudManager;
    [SerializeField] private Button atkButton;
    [SerializeField] private ActionMenu actionMenu;

    float ANIMATION_DELAY_TIME = 1f;
    float SHORT_DELAY_TIME = 0.5f;

    private List<Action> roundQueue = new List<Action>();

    void Start()
    {
        playerParty.CreateParty();
        enemyParty.CreateParty(false);
        //Debug.Log("Battle Start!");
        //hudManager.UpdateHUD(playerStats, enemyStats);
        //actionMenu.actionsList.First().GetComponent<Button>().Select();
        //hudManager.ShowActionMenu();
    }

    public void ConfirmAttackAction()
    {
        foreach (var player in playerParty.partyMembers)
        {
            var stats = player.GetComponent<Stats>();
            var enemyStats = enemyParty.partyMembers.PickRandom();

            roundQueue.Add(new ActionAttack(stats, enemyStats));

        }
        StartCoroutine(ExecuteBattleRound());

    }

    public void ConfirmGuardAction()
    {
        foreach (var player in playerParty.partyMembers)
        {
            var stats = player.GetComponent<Stats>();
            var enemyStats = enemyParty.partyMembers.PickRandom();

            roundQueue.Add(new ActionGuard(stats, enemyStats));
        }
        StartCoroutine(ExecuteBattleRound());
    }

    public void ConfirmMagicAction()
    {
        foreach (var player in playerParty.partyMembers)
        {
            var stats = player.GetComponent<Stats>();
            var enemyStats = enemyParty.partyMembers.PickRandom();

            roundQueue.Add(new ActionMagic(stats, enemyStats));
        }
        StartCoroutine(ExecuteBattleRound());
    }
    public void ConfirmItemAction()
    {
        foreach (var player in playerParty.partyMembers)
        {
            var stats = player.GetComponent<Stats>();
            var enemyStats = enemyParty.partyMembers.PickRandom();

            roundQueue.Add(new ActionItem(stats, enemyStats));
        }
        StartCoroutine(ExecuteBattleRound());
    }

    public IEnumerator ExecuteBattleRound()
    {
        hudManager.HideActionMenu();

        //var enemyTarget = playerParty.partyMembers.First().GetComponent<Stats>();
        foreach (var enemy in enemyParty.partyMembers)
        {
            foreach (var player in playerParty.partyMembers)
            {
                Debug.Log($"\t{player.nickname}");
            }

            var playerStats = playerParty.partyMembers.PickRandom();
            Debug.Log($"[EnemyTarget]: {playerStats.nickname}");

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

        hudManager.ShowActionMenu();
    }

    IEnumerator delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
