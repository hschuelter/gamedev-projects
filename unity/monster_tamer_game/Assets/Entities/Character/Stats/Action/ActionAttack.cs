using System.Linq;
using UnityEngine;

public class ActionAttack : Action
{
    public ActionAttack(Stats user, Stats target) : base(user, target)
    {
        this.user = user;
        this.target = target;
        this.actionName = "Attack";
        this.description = $"{user.nickname} attacked {target.nickname}!";
    }

    public ActionAttack(Stats user) : base(user)
    {
        this.user = user;
        this.actionName = "Attack";
    }

    public override void SetTarget(Stats target)
    {
        this.target = target;
        this.description = $"{user.nickname} attacked {target.nickname}!";
    }

    public override void Execute()
    {
        if (target.currentHealth == 0)
        {
            if (targetParty.Where(t => t.currentHealth > 0).ToList().Count == 0) return;

            target = this.targetParty.Where(t => t.currentHealth > 0).PickRandom();
        }

        var vfxPrefab = VFXManager.Instance.vfxHitPrefab;
        user.Attack(target, vfxPrefab, actionType);
        ApplyNegativeEffects(actionType);
    }
}
