
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EncounterController : MonoBehaviour
{
    [SerializeField] public List<Encounter> EnconterList = new List<Encounter>();
    private int encounterNumber = 0;


    public List<Character> NextEncounter()
    {
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
