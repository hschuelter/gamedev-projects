using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] public string title;
    [SerializeField] public int healValue;
    [SerializeField] public bool isTargetParty;
    [SerializeField] public GameObject vfxPrefab;
    [SerializeField] public AudioClip sfx;
}
