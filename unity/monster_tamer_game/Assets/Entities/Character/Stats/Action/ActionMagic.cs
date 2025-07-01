using UnityEngine;

public class ActionMagic : Action
{
    public ActionMagic(Stats user, Stats target) : base(user, target)
    {
        this.user = user;
        this.target = target;
        this.actionName = "Magic";
        this.description = $"{user.nickname} casted Fire!";
    }

    public ActionMagic(Stats user) : base(user)
    {
        this.user = user;
        this.actionName = "Magic!";
    }

    public override void SetTarget(Stats target)
    {
        this.target = target;
        this.description = $"{user.nickname} casted Fire!";
    }

    public override void Execute()
    {
        user.Magic(target);
    }
}
