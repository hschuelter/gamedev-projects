using System.Data;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Sprite characterSprite;
    public StatsData statusData;
    public RuntimeAnimatorController animatorController;
    
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

    //void Update()
    //{

    //}
}
