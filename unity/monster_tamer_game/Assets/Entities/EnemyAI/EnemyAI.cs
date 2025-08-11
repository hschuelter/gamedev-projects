using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public interface EnemyAI 
{
    public virtual Action ChooseAction(Stats user, Stats target)
    {
        Debug.Log($"Basic AI");
        return new ActionAttack(user, target);
    }

    public virtual Stats ChooseTarget(List<Stats> targetList)
    {
        Debug.Log($"Basic AI");
        return targetList.PickRandom();

    }
}
