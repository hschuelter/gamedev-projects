using UnityEngine;

public class Item
{
    public ItemData itemData;

    [HideInInspector] public int healValue;
    [HideInInspector] public string title;
    [HideInInspector] public bool isTargetParty;
    [HideInInspector] public GameObject vfxPrefab;

    public Item(ItemData itemData)
    {
        this.itemData = itemData;
        this.healValue = itemData.healValue;
        this.title = itemData.title;
        this.isTargetParty = itemData.isTargetParty;
        this.vfxPrefab = itemData.vfxPrefab;
    }

    public virtual void Use(Stats target)
    {
        Debug.Log($"[ITEM] override me...");
    }
}
