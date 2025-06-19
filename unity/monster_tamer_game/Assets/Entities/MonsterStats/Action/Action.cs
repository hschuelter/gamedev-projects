using UnityEngine;

public class Action
{
    public MonsterStats user;
    public MonsterStats target;
    public string actionName;
    public string description;

    public Action(MonsterStats user, MonsterStats target, string actionName, string description = "")
    {
        this.user = user;
        this.target = target;
        this.actionName = actionName;
        this.description = description;
    }
}
