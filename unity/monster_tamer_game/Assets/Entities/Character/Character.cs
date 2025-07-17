using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    public RuntimeAnimatorController animatorController;
    public Sprite characterSprite;
    
    [Header("RPG")]
    public StatsData statsData;
    public List<SpellData> magicList = new List<SpellData>();
    
    [HideInInspector] public Stats stats;

    private void Awake()
    {
        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = characterSprite;

        var animator = gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = animatorController;

    }

    void Start()
    {
        //stats = GetComponent<Stats>();
        //stats.UpdateStats(statusData);
    }

}
