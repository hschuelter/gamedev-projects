using UnityEngine;

public class Action
{
    public Stats user;
    public Stats target;
    public string actionName;
    public string description;

    public Action(Stats user, Stats target, string actionName, string description = "")
    {
        this.user = user;
        this.target = target;
        this.actionName = actionName;
        this.description = description;
    }
}
