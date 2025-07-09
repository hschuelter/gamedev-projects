using UnityEngine;

[CreateAssetMenu(fileName = "CommandData", menuName = "Scriptable Objects/CommandData")]
public class CommandData : ScriptableObject
{
    public string description;
    public int baseDamage;
    public int manaCost;
    public string title;

    public GameObject vfxPrefab;

}
