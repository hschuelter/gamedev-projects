using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "Scriptable Objects/SpellData")]
public class SpellData : ScriptableObject
{
    [SerializeField] public string title;
    [SerializeField] public int baseDamage;
    [SerializeField] public int manaCost;
    [SerializeField] public bool isTargetParty;
    [SerializeField] public DamageType damageType;
    [SerializeField] public GameObject vfxPrefab;
}
