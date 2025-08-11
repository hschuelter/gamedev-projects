using System.Collections.Generic;
using UnityEngine;

public class BasicAI : EnemyAI
{
    public Action ChooseAction(Stats user, Stats target)
    {
        return new ActionAttack(user, target);
    }

    public Stats ChooseTarget(List<Stats> targetList)
    {
        return targetList.PickRandom();

    }
}
