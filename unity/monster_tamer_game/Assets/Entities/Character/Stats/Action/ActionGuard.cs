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

    public override void Execute()
    {
        user.Guard();
    }
}
