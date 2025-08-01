using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterExpHUDManager : MonoBehaviour
{
    public Party party;
    public CharacterExpHUD characterExpHUDPrefab;

    private List<CharacterExpHUD> hudList;

    public void DrawHUD(int expGained)
    {
        int i = 0;
        hudList = new List<CharacterExpHUD>();
        foreach (Stats character in party.partyMembers)
        {
            var memberHUD = Instantiate(characterExpHUDPrefab, this.transform);
            memberHUD.characterStats = character;
            memberHUD.name = $"ExpGainedHUD-{character.nickname}-{i++}";
            memberHUD.Draw(expGained);
            hudList.Add(memberHUD);
        }
    }

    public void StartExp()
    {
        hudList.All(item => item.isReady = true);
    }

    public bool isOK()
    {
        return hudList.All(item => item.isOk);
    }
    public void ClearChildren()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
