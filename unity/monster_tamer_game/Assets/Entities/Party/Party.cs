using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Party : MonoBehaviour
{
    public PartyHUDManager partyHUD;
    public List<GameObject> partyMembersPrefab;
    [HideInInspector] public List<MonsterStats> partyMembers;

    void Start()
    {
        partyMembers = new List<MonsterStats>();
        ResetParty();

        int i = 0;
        foreach (var member in partyMembersPrefab)
        {
            var cur = Instantiate(member, this.transform);
            float y_offset = -0.20f * i;
            float x_offset = -0.15f * i;
            cur.transform.position = new Vector3(this.transform.position.x + x_offset, this.transform.position.y + y_offset, 0);
            i++;
        }
        partyMembers = gameObject.GetComponentsInChildren<MonsterStats>().ToList();

        if (partyHUD == null) return;
        partyHUD.DrawHUD();
    }
    public void ResetParty()
    {
        foreach (var member in partyMembersPrefab)
        {
            MonsterStats stats = member.GetComponent<MonsterStats>();
            stats.currentHealth = stats.maxHealth;
            stats.currentMana = stats.maxMana;
        }
    }
}
