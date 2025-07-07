using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    private string[] _itemList = { "Potion" };
    private void Awake()
    {
        Instance = this;
    }

    public string[] GetItems()
    {
        return _itemList;
    }

}
