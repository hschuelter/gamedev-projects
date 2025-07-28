using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardsController : MonoBehaviour
{
    public List<Character> possibleCharacters;

    public Character GetRandomCharacter(List<Character> currentParty)
    {
        return possibleCharacters.Where(character => !currentParty.Any(cur => character.statsData.nickname == cur.statsData.nickname)).PickRandom();
    }
}
