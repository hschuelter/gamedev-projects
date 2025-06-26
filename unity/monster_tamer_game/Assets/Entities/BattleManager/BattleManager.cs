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
    [SerializeField] private Stats playerStats;
    [SerializeField] private Stats enemyStats;
    
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
            string description = $"{stats.nickname} attacked {enemyStats.nickname}!";
            roundQueue.Add(new Action(stats, enemyStats, "Attack", description));

        }
        StartCoroutine(ExecuteBattleRound());

    }

    public void ConfirmGuardAction()
    {
        foreach (var player in playerParty.partyMembers)
        {
            var stats = player.GetComponent<Stats>();
            string description = $"{stats.nickname} is guarding!";
            roundQueue.Add(new Action(stats, enemyStats, "Guard", description));
        }
        StartCoroutine(ExecuteBattleRound());
    }

    public void ConfirmMagicAction()
    {
        foreach (var player in playerParty.partyMembers)
        {
            var stats = player.GetComponent<Stats>();
            string description = $"{stats.nickname} casted Fire!";
            roundQueue.Add(new Action(stats, enemyStats, "Magic", description));
        }
        StartCoroutine(ExecuteBattleRound());
    }

    public IEnumerator ExecuteBattleRound()
    {
        hudManager.HideActionMenu();

        var enemyTarget = playerParty.partyMembers.First().GetComponent<Stats>();

        roundQueue.Add(new Action(enemyStats, enemyTarget, "Attack", $"{enemyStats.nickname} attacked {enemyTarget.nickname}!"));
        roundQueue = roundQueue.OrderByDescending(action => action.actionName == "Guard").ThenByDescending(action => action.user.speed).ToList();
        
        roundQueue.RemoveAll(action => action.user.currentHealth <= 0);

        while (roundQueue.Count > 0)
        {
            var currAction = roundQueue.First();
            roundQueue.RemoveAt(0);
            var user = currAction.user;
            var target = currAction.target;
            hudManager.UpdateDescription(currAction.description);
            hudManager.ShowDescription();

            if (currAction.actionName == "Attack")
            {
                user.Attack(target);
            }

            if (currAction.actionName == "Magic")
            {
                user.Magic(target);
            }

            if (currAction.actionName == "Guard")
            {
                user.Guard();
            }


            if (target.characterHUDManager != null)
                hudManager.UpdateHUD(target.characterHUDManager);

            if (user.characterHUDManager != null)
                hudManager.UpdateHUD(user.characterHUDManager);

            roundQueue.RemoveAll(action => action.user.currentHealth <= 0);
            user.ActionAnimation();
            yield return delay(ANIMATION_DELAY_TIME);
            user.Stepback();

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
