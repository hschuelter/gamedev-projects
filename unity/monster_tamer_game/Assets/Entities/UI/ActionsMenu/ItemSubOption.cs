using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ItemSubOption : ActionOption
{
    public ItemData itemData;
    public override void SetInfo()
    {
        description = commandData.description;
        var texts = GetComponentsInChildren<TMP_Text>();

        foreach (var t in texts)
        {
            if (t.name == "Label")
                t.text = commandData.title;
        }
    }

    public void SetInfo(Item item, int amount)
    {
        var texts = GetComponentsInChildren<TMP_Text>();
        foreach (var t in texts)
        {
            if (t.name == "Mana Cost")
                t.text = $"x{amount}";
            if (t.name == "Label")
                t.text = item.title;
        }
    }

    public void ConfirmItem()
    {
        Item item;

        if (itemData.healValue > 0) item = new ItemHeal(itemData);
        else item = new Item(itemData);

        int itemAmount = InventoryManager.Instance.GetAmountOf(item);
        var amountInRoundQueue = InventoryManager.Instance.AmountInRoundQueue(item);
        if (itemAmount - amountInRoundQueue == 0) return;

        var stats = battleManager.partyList.First();
        var action = new ActionItem(stats);
        action.SetItem(item);

        Party party = item.isTargetParty ? battleManager.playerParty : battleManager.enemyParty;
        battleManager.ConfirmAction(action, party, item.isTargetParty);
    }
}
