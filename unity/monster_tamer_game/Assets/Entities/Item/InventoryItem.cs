using UnityEngine;

public class InventoryItem
{
    public Item item;
    public int amount;
    public InventoryItem(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
    public InventoryItem(ItemData itemData, int amount)
    {
        this.item = new Item(itemData);
        this.amount = amount;
    }
}
