using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class BasicAI : EnemyAI
{
    public Action ChooseAction(Stats user, Stats target)
    {
        var action = new ActionAttack(user, target);
        action.actionType = ActionType.Normal;

        return action;
    }

    public Stats ChooseTarget(List<Stats> targetList)
    {
        return targetList.PickRandom();

    }
}
