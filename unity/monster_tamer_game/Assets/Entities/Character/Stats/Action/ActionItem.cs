using UnityEngine;

public class ActionItem: Action
{
    public ActionItem(Stats user, Stats target) : base(user, target)
    {
        this.user = user;
        this.target = target;
        this.actionName = "Item";
        this.description = $"{user.nickname} used Potion!";
    }

    public ActionItem(Stats user) : base(user)
    {
        this.user = user;
        this.actionName = "Item";
    }

    public override void SetTarget(Stats target)
    {
        this.target = target;
        this.description = $"{user.nickname} used Potion!";
    }

    public override void Execute()
    {
        user.UseItem(target);
    }
}
