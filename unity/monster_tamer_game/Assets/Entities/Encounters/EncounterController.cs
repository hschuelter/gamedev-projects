
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EncounterController : MonoBehaviour
{
    [SerializeField] public List<Encounter> EnconterList = new List<Encounter>();
    [SerializeField] public List<Sprite> BGList;
    [SerializeField] public SpriteRenderer battleBackground;

    public int encounterNumber = 0;


    public List<Character> NextEncounter()
    {
        Debug.Log($"[EncounterController] encounterNumber = {encounterNumber}");
        battleBackground.sprite = BGList.ElementAt(encounterNumber);
        var currentEncounter = EnconterList.ElementAt(encounterNumber++);
        List<Character> enemies = new List<Character>();

        for (int i = 0; i < currentEncounter.quantity; i++)
        {
            enemies.Add(currentEncounter.possibleEnemies.PickRandom());
        }

        return enemies;
    }

    public bool IsNext()
    {
        return encounterNumber < EnconterList.Count;
    }
}
