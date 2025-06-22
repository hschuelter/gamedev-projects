using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PartyHUDManager : MonoBehaviour
{
    public Party party;
    public PartyMemberHUDManager partyMemberHUDPrefab;

    private List<PartyMemberHUDManager> partyMembersHUD;

    public void DrawHUD()
    {
        partyMembersHUD = new List<PartyMemberHUDManager>();
        int i = 0;
        foreach (MonsterStats partyMember in party.partyMembers)
        {
            partyMemberHUDPrefab.GetComponent<PartyMemberHUDManager>().partyMember = partyMember.gameObject;

            var memberHUD = Instantiate(partyMemberHUDPrefab, this.transform);
            partyMember.GetComponent<MonsterStats>();
            memberHUD.name = $"PartyMemberHUD-{partyMember.nickname}-{i++}";

            partyMembersHUD.Add(memberHUD);
            memberHUD.Draw();
        }
    }
}
