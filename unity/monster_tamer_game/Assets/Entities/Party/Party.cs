using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Party : MonoBehaviour
{
    public List<GameObject> partyMembers;
    public GameObject prefab;
    public PartyHUDManager partyHUD;

    void Start()
    {
        CreateParty();

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

    void Update()
    {
        
    }

    public void CreateParty()
    {
        partyMembers = new List<GameObject>();
        partyMembers.Add(prefab);
        partyMembers.Add(prefab);
        partyMembers.Add(prefab);
        //partyMembers.Add(prefab);

        if (partyHUD == null) return;
        partyHUD.DrawHUD();
    }
}
