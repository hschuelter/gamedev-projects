using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float sizeGrowth = 3.0f;

    public Rigidbody2D body;
    public ParticleSystem particles;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }
    void Awake()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void OnBallCollision(Vector3 position)
    {
        StartCoroutine(ActivateParticles(position, 0.5f));
        StartCoroutine(ActivateEntityAnimation(0.1f));
    }
    private IEnumerator ActivateParticles(Vector3 position, float time)
    {
        particles.transform.position = position;
        particles.Play();
        yield return new WaitForSeconds(time);
        particles.Stop();
    }
    private IEnumerator ActivateEntityAnimation(float time)
    {
        Vector3 spriteScale = spriteRenderer.transform.localScale;

        spriteRenderer.transform.localScale = spriteScale * sizeGrowth;
        yield return new WaitForSeconds(time);
        spriteRenderer.transform.localScale = spriteScale;
    }
}
