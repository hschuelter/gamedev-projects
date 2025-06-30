using UnityEngine;
using System.Collections;

public class ActionAttack : Action
{
    public ActionAttack(Stats user, Stats target) : base(user, target)
    {
        this.user = user;
        this.target = target;
        this.actionName = "Attack";
        this.description = $"{user.nickname} attacked {target.nickname}!";

    }

    public override void Execute()
    {
        user.Attack(target);
    }
}
