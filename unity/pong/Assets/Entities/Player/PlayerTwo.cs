using System.Drawing;
using UnityEngine;

public class PlayerTwo : Player
{
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerController = new PlayerControllerTwo();
        sizeGrowth = 1.2f;
        spriteRenderer.color = color;
    }
    
}
