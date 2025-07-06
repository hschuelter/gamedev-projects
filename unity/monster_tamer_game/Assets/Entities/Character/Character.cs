using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    public RuntimeAnimatorController animatorController;
    public Sprite characterSprite;
    
    [Header("RPG")]
    public StatsData statusData;
    public List<SpellData> magicList = new List<SpellData>();
    
    private Stats stats;

    private void Awake()
    {
        Debug.Log($"[Character] - Awake");
        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = characterSprite;

        var animator = gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = animatorController;

    }

    void Start()
    {
        Debug.Log($"[Character] - Start");
        //stats = GetComponent<Stats>();
        //stats.UpdateStats(statusData);
    }

}
