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
    [SerializeField] private MonsterStats playerStats;
    [SerializeField] private MonsterStats enemyStats;
    [SerializeField] private HUDManager hudManager;
    [SerializeField] private Button atkButton;
    [SerializeField] private GameObject actionsMenu;

    float ANIMATION_DELAY_TIME = 1f;
    float SHORT_DELAY_TIME = 0.5f;

    private List<Action> roundQueue = new List<Action>();

    void Start()
    {
        Debug.Log("Battle Start!");
        //hudManager.UpdateHUD(playerStats, enemyStats);
        atkButton.Select();
    }

    public void ConfirmAttackAction()
    {
        string description = $"{playerStats.nickname} attacked {enemyStats.nickname}!";
        roundQueue.Add(new Action(playerStats, enemyStats, "Attack", description));
        StartCoroutine(ExecuteBattleRound());

    }

    public void ConfirmGuardAction()
    {
        string description = $"{playerStats.nickname} is guarding!";
        roundQueue.Add(new Action(playerStats, enemyStats, "Guard", description));
        StartCoroutine(ExecuteBattleRound());
    }

    public void ConfirmMagicAction()
    {
        string description = $"{playerStats.nickname} casted Fire!";
        roundQueue.Add(new Action(playerStats, enemyStats, "Magic", description));
        StartCoroutine(ExecuteBattleRound());
    }

    public IEnumerator ExecuteBattleRound()
    {
        actionsMenu.SetActive(false);

        roundQueue.Add(new Action(enemyStats, playerStats, "Attack", $"{enemyStats.nickname} attacked {playerStats.nickname}!"));
        roundQueue = roundQueue.OrderByDescending(action => action.actionName == "Guard").ThenByDescending(action => action.user.speed).ToList();

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

            roundQueue.RemoveAll(action => action.user.currentHealth == 0);
            user.ActionAnimation();
            yield return delay(ANIMATION_DELAY_TIME);
            user.Stepback();

            hudManager.HideDescription();
            yield return delay(SHORT_DELAY_TIME);
        }

        actionsMenu.SetActive(true);
    }

    IEnumerator delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
