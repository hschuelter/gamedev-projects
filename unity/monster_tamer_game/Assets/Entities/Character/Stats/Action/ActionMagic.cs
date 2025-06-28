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

    public override void Execute()
    {
        user.Magic(target);
    }
}
