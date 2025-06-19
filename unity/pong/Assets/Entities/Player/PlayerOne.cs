using System.Drawing;
using UnityEngine;

public class PlayerOne : Player
{
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        playerController = new PlayerControllerOne();
        sizeGrowth = 1.2f;
        spriteRenderer.color = color;
    }
}
