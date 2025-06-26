using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;

public class Party : MonoBehaviour
{
    public PartyHUDManager partyHUD;
    public List<GameObject> partyMembersPrefab;
    public List<Character> partyCharacters;

    //[HideInInspector] 
    public List<Stats> partyMembers { get; set; }

    void Start()
    {
        partyMembers = new List<Stats>();
        ResetParty();

        int i = 0;
        foreach (Character character in partyCharacters)
        {
            var cur = CreateCharacter(character);
            float x_offset = -0.15f * i;
            float y_offset = -0.20f * i;

            cur.transform.position = new Vector3(this.transform.position.x + x_offset, this.transform.position.y + y_offset, 0);
            i++;
        }

        partyMembers = gameObject.GetComponentsInChildren<Stats>().ToList();

        if (partyHUD == null) return;
        partyHUD.DrawHUD();
    }
    public void ResetParty()
    {
        foreach (var member in partyMembersPrefab)
        {
            Stats stats = member.GetComponent<Stats>();
            stats.currentHealth = stats.maxHealth;
            stats.currentMana = stats.maxMana;
        }
    }

    public GameObject CreateCharacter(Character character)
    {
        GameObject characterObj = new GameObject($"PartyMemberHUD-{character.statusData.nickname}");


        var spriteRenderer = characterObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = character.characterSprite;
        spriteRenderer.sortingOrder = 1;

        var animator = characterObj.AddComponent<Animator>();
        animator.runtimeAnimatorController = character.animatorController;

        var stats = characterObj.AddComponent<Stats>();
        stats.UpdateStats(character.statusData);

        characterObj.transform.parent = this.transform;
        return characterObj;
    }
}
