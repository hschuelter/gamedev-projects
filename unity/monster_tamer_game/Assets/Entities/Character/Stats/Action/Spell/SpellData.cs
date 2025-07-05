using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "Scriptable Objects/SpellData")]
public class SpellData : ScriptableObject
{
    [SerializeField] public int baseDamage;
    [SerializeField] public int manaCost;
    [SerializeField] public string title;
}
