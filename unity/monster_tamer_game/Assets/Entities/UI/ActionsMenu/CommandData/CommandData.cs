using UnityEngine;

[CreateAssetMenu(fileName = "CommandData", menuName = "Scriptable Objects/CommandData")]
public class CommandData : ScriptableObject
{
    public string title;
    public string description;
    public int baseDamage;
    public int manaCost;

    public GameObject vfxPrefab;

}
