using UnityEngine;

public class ActionItem: Action
{
    public Item item;
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

    public void SetItem(Item item)
    {
        this.item = item;
        this.description = $"{user.nickname} used {item.title}!";
    }

    public override void SetTarget(Stats target)
    {
        this.target = target;
        this.description = $"{user.nickname} used Potion!";
    }

    public override void Execute()
    {
        var vfxPrefab = VFXManager.Instance.vfxHealPrefab;
        var sfx = SoundEffectsController.Instance.sfxHeal;
        user.UseItem(target, item);
    }
}
