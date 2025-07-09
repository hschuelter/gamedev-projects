using System.Linq;
using UnityEngine;

public class ActionMagic : Action
{
    private Spell spell;
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

    public void SetSpell(Spell spell)
    {
        this.spell = spell;
        this.description = $"{user.nickname} casted {spell.title}!";
    }

    public override void SetTarget(Stats target)
    {
        this.target = target;
    }

    public override void Execute()
    {
        if (target.currentHealth == 0)
            target = this.targetParty.Where(t => t.currentHealth > 0).PickRandom();
        
        var vfxPrefab = VFXManager.Instance.vfxHitPrefab;
        user.Magic(target, spell);
    }
}
