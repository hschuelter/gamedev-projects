using UnityEngine;

public class ActionGuard : Action
{
    public ActionGuard(Stats user, Stats target) : base(user, target)
    {
        this.user = user;
        this.target = target;
        this.actionName = "Guard";
        this.description = $"{user.nickname} is guarding!";
    }

    public ActionGuard(Stats user) : base(user)
    {
        this.user = user;
        this.actionName = "Guard";
    }

    public override void SetTarget(Stats target)
    {
        this.target = target;
        this.description = $"{user.nickname} is guarding!";
    }

    public override void Execute()
    {
        user.Guard();
    }
}
