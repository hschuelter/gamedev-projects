using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : Entity
{
    [SerializeField] private float moveSpeed = 1.8f;
    [SerializeField] public Color color;

    public Vector2 velocity;
    public IPlayerController playerController;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerController = new PlayerControllerOne();
        sizeGrowth = 1.2f;
        spriteRenderer.color = color;
    }

    void Update()
    {
        var input = playerController.GetInput();

        velocity.y = input * moveSpeed;
        body.linearVelocity = velocity;

    }

}
