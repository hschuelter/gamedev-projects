using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExpertAI : EnemyAI
{
    public Action ChooseAction(Stats user, Stats target)
    {
        Damage damageNormal = DamageManager.Instance.CalculateDamage(user, target, 1, DamageType.Physical, isMagic: false, actionType: ActionType.Normal);
        Damage damageCharged = DamageManager.Instance.CalculateDamage(user, target, 1, DamageType.Physical, isMagic: false, actionType: ActionType.Charge);
        Damage damageReckless = DamageManager.Instance.CalculateDamage(user, target, 1, DamageType.Physical, isMagic: false, actionType: ActionType.Reckless);

        var action = new ActionAttack(user, target);
        action.actionType = ActionType.Normal;

        if (damageNormal.value >= target.currentHealth) return action;

        if (damageCharged.value >= target.currentHealth)
        {
            action.actionType = ActionType.Charge;
            return action;
        }

        if (damageReckless.value >= target.currentHealth)
        {
            action.actionType = ActionType.Reckless;
            return action;
        }

        return action;
    }

    public Stats ChooseTarget(List<Stats> targetList)
    {
        return targetList.OrderBy(t => t.currentHealth).First();
    }
}
