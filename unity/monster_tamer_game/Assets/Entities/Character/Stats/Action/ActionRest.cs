using UnityEngine;

public class ActionRest : Action
{
    public ActionRest(Stats user, Stats target) : base(user, target)
    {
        this.user = user;
        this.target = target;
        this.actionName = "Rest";
        this.description = $"{user.nickname} is resting!";
    }

    public ActionRest(Stats user) : base(user)
    {
        this.user = user;
        this.actionName = "Rest";
    }

    public override void SetTarget(Stats target)
    {
        this.target = target;
        this.description = $"{user.nickname} can't move";
    }

    public override void Execute()
    {
        user.Rest();
    }
}
