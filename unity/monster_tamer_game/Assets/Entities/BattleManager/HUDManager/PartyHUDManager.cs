using UnityEngine;

public class PartyHUDManager : MonoBehaviour
{
    public Party party;
    public GameObject partyMemberPrefab;

    public void DrawHUD()
    {
        foreach(GameObject partyMember in party.partyMembers)
        {
            partyMemberPrefab.GetComponent<PartyMemberHUDManager>().partyMember = partyMember;
            var member = Instantiate(partyMemberPrefab, this.transform);
            partyMember.GetComponent<MonsterStats>().characterHUDManager = member.GetComponent<PartyMemberHUDManager>();

        }
    }
}
