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
    public List<Character> partyCharacters;
    public GameObject shadowPrefab;
    public TargetSelectionManager targetSelectionManager;

    [HideInInspector] public List<Stats> partyMembers;
    
    private int sortingOrder = 1;
    public void CreateParty(bool isPlayer = true)
    {
        sortingOrder = 1;
        ClearChildren();
        partyMembers = new List<Stats>();

        int i = 0;
        int positionMod = isPlayer ? 1 : -1;
        foreach (Character character in partyCharacters)
        {
            var cur = CreateCharacter(character, isPlayer);
            float x_offset = -0.15f * i * positionMod;
            float y_offset = -0.18f * i;

            cur.transform.position = new Vector3(this.transform.position.x + x_offset, this.transform.position.y + y_offset, 0);
            cur.transform.localScale = new Vector3(cur.transform.localScale.x * positionMod, cur.transform.localScale.y, cur.transform.localScale.z);
            i++;
        }

        partyMembers = gameObject.GetComponentsInChildren<Stats>().ToList();

        if (partyHUD == null) return;
        partyHUD.DrawHUD();
    }

    public GameObject CreateCharacter(Character character, bool isPlayer)
    {
        GameObject characterObj = new GameObject($"PartyMemberHUD-{character.statsData.nickname}-{sortingOrder}");

        GameObject shadowObject = Instantiate(shadowPrefab);
        shadowObject.transform.parent = characterObj.transform;
        
        var spriteRenderer = characterObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = character.characterSprite;
        spriteRenderer.sortingOrder = sortingOrder++;

        var animator = characterObj.AddComponent<Animator>();
        animator.runtimeAnimatorController = character.animatorController;

        var stats = characterObj.AddComponent<Stats>();
        stats.animatorController = animator;
        stats.UpdateStats(character.statsData);
        character.stats = stats;

        if (!isPlayer) { 
            var collider2D = characterObj.AddComponent<BoxCollider2D>();
            collider2D.size = new Vector2(0.33f, 0.3f);
            stats.targetSelectionManager = this.targetSelectionManager;
        }

        characterObj.transform.parent = this.transform;
        return characterObj;
    }

    public Character GetCharacter(Stats stats)
    {
        return partyCharacters.Find(c => c.stats.name == stats.name);
    }


    public void ClearChildren()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
