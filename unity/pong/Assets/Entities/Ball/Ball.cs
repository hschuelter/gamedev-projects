using System;
using UnityEngine;
using UnityEngine.Windows;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    [SerializeField] private float SPEED = 1.0f;
    [SerializeField] private float MAX_SPEED = 5f;
    public float moveSpeed;

    public float maximumPitch = 2f;
    public float pitchIncrease = 0.05f;
    
    private Rigidbody2D body;
    private Collider2D col;
    private TrailRenderer trailRenderer;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private Vector2 velocity;
    private Vector3 startingPosition;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 1f;

        startingPosition = new Vector3(0, 0, 0);
        Respawn();
    }
    void Awake()
    {
        trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    void Update()
    {
        velocity = velocity.normalized * moveSpeed;
        body.linearVelocity = velocity;
    }

    private void Respawn()
    {
        float angle = Random.Range(0, 360);
        velocity = new Vector2(1, 0);
        velocity = Quaternion.Euler(0f, 0f, angle) * velocity;

        transform.position = startingPosition;
        moveSpeed = SPEED;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "ScoreZone") return;

        Vector2 normal = other.contacts[0].normal;
        Vector3 position = this.transform.position;

        float angle = Vector2.Angle(velocity, velocity.x > 0 ? Vector2.right : Vector2.left);
        if (angle > 70 && normal.y != 0)
        {
            velocity += velocity.x > 0 ? Vector2.right : Vector2.left;
        }

        Entity entity = other.gameObject.GetComponent<Entity>();

        if (normal.x != 0) velocity.x *= -1;
        if (normal.y != 0) velocity.y *= -1;

        if (other.gameObject.tag == "Player")
        {
            entity.OnBallCollision(new Vector2(entity.transform.position.x, position.y));
            spriteRenderer.color = entity.spriteRenderer.color;
            trailRenderer.startColor = entity.spriteRenderer.color;

            moveSpeed = Mathf.Min(moveSpeed * 1.1f, MAX_SPEED);
            Vector2 playerSpeed = other.gameObject.GetComponent<Rigidbody2D>().linearVelocity;
            velocity = velocity + (playerSpeed / 3);

            audioSource.pitch = Mathf.Min(audioSource.pitch + pitchIncrease, maximumPitch);
        }
        else
        {
            entity.OnBallCollision(new Vector2(position.x, entity.transform.position.y));
        }


        velocity = velocity.normalized;
        audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<ScoreZone>().Score();
        Destroy(this);
    }
}
