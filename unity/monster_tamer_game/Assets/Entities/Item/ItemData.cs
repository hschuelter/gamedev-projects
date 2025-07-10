using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] public int healValue;
    [SerializeField] public string title;
    [SerializeField] public bool isTargetParty;
    [SerializeField] public GameObject vfxPrefab;

}
