using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Party : MonoBehaviour
{
    public List<GameObject> partyMembers;
    //public GameObject prefab1;
    //public GameObject prefab2;
    //public GameObject prefab3;
    //public GameObject prefab4;
    public PartyHUDManager partyHUD;

    void Start()
    {
        ResetParty();

        if (partyHUD == null) return;
        partyHUD.DrawHUD();

        int i = 0;
        foreach (var member in partyMembers)
        {
            var cur = Instantiate(member, this.transform);
            float y_offset = -0.20f * i;
            float x_offset = -0.15f * i;
            cur.transform.position = new Vector3(this.transform.position.x + x_offset, this.transform.position.y + y_offset, 0);
            i++;
        }
    }
    public void ResetParty()
    {
        foreach (var member in partyMembers)
        {
            MonsterStats stats = member.GetComponent<MonsterStats>();
            stats.currentHealth = stats.maxHealth;
            stats.currentMana = stats.maxMana;
        }
    }
}
