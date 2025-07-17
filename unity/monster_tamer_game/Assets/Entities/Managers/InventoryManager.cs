using Mono.Cecil.Cil;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    private List<InventoryItem> _itemList;

    public ItemData potionItemData;
    public BattleManager battleManager;
    private void Awake()
    {
        Instance = this;

        _itemList = new List<InventoryItem>();
        var _item = new ItemHeal(potionItemData);
        AddItem(_item, 1);
    }

    public void AddItem(Item item, int amount)
    {
        if(_itemList.Any(i => i.item.title == item.title))
        {
            var _item = _itemList.Where(i => i.item.title == item.title).First();
            _item.amount += amount;
        }
        else
        {
            var inventoryItem = new InventoryItem(item, amount);
            _itemList.Add(inventoryItem);
        }
    }

    public List<InventoryItem> GetItems()
    {
        return _itemList;
    }

    public int GetAmountOf(Item item)
    {
        return _itemList.Where(i => i.item.title == item.title).First().amount;
    }

    public void UseItem(Item item)
    {
        var _item = _itemList.Where(i => i.item.title == item.title).First();
        _item.amount--;
        PrintAllItems();
    }

    public void PrintAllItems()
    {
        foreach (var inventoryItem in _itemList)
        {
            Debug.Log($"{inventoryItem.item.title} x{inventoryItem.amount}");
        }
    }

    public int AmountInRoundQueue(Item item)
    {
        var actionItems = battleManager.roundQueue.OfType<ActionItem>().ToList();
        return actionItems.Where(a => a.item.title == item.title).ToList().Count;
    }
}
