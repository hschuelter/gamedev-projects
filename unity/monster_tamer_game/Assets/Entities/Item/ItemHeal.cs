using UnityEngine;

public class ItemHeal : Item
{

    public ItemHeal(ItemData itemData) : base(itemData)
    {
        this.itemData = itemData;
        this.healValue = itemData.healValue;
        this.title = itemData.title;
        this.isTargetParty = itemData.isTargetParty;
        this.vfxPrefab = itemData.vfxPrefab;
    }

    public override void Use(Stats target)
    {
        target.Heal(healValue, this.sfx);
    }
}
