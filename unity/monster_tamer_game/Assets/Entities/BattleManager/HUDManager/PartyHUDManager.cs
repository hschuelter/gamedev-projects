using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PartyHUDManager : MonoBehaviour
{
    public Party party;
    public PartyMemberHUDManager partyMemberPrefab;

    private List<PartyMemberHUDManager> partyMembersHUD;

    public void DrawHUD()
    {
        partyMembersHUD = new List<PartyMemberHUDManager>();
        int i = 0;
        foreach (GameObject partyMember in party.partyMembers)
        {
            partyMemberPrefab.GetComponent<PartyMemberHUDManager>().partyMember = partyMember;
            var memberHUD = Instantiate(partyMemberPrefab, this.transform);
            var stats = partyMember.GetComponent<MonsterStats>();
            memberHUD.name = $"PartyMemberHUD-{stats.nickname}-{i++}";
            //partyMember.GetComponent<MonsterStats>().characterHUDManager = member.GetComponent<PartyMemberHUDManager>();

            partyMembersHUD.Add(memberHUD);
            memberHUD.Draw();
        }
    }
}
