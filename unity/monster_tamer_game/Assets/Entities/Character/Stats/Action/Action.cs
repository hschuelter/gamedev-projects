using UnityEngine;

public class Action
{
    public Stats user { get; set; }
    public Stats target { get; set; }
    public string actionName { get; set; }
    public string description { get; set; }

    public Action(Stats user, Stats target)
    {
        this.user = user;
        this.target = target;
        this.actionName = "Dont know!";
        this.description = $"{user.nickname} did something to {target.nickname}...";
    }
    public Action(Stats user)
    {
        this.user = user;
        this.actionName = "Dont know!";
    }

    public virtual void SetTarget(Stats target)
    {
        this.target = target;
        this.description = $"{this.user.nickname} did something to {target.nickname}...";
    }

    public virtual void Execute()
    {
        Debug.Log($"Override me...");
    }
}
