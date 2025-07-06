using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.8f;

    private Rigidbody2D body;
    private Vector2 velocity;

    public Transform movePoint;
    public LayerMask collisionLayer;
    public LayerMask encounterLayer;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bool playerArrived = Vector3.Distance(transform.position, movePoint.position) <= 0.001f;
        Move();

        if (playerArrived)
        {
            var h_input = Input.GetAxisRaw("Horizontal");
            var v_input = Input.GetAxisRaw("Vertical");

            //velocity = new Vector2(h_input, v_input);
            //body.linearVelocity = velocity;
            //if (h_input != 0) v_input = 0f;

            if (h_input != 0)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(h_input, 0f, 0f) * 0.32f, .01f, collisionLayer)) {
                    movePoint.position += new Vector3(h_input, 0f, 0f) * 0.32f;
                }
            }

            else if (v_input != 0f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, v_input, 0f) * 0.32f, .01f, collisionLayer)) {
                    movePoint.position += new Vector3(0f, v_input, 0f) * 0.32f; 
                }
            }


        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;
        bool isEncounterLayer = (encounterLayer & (1 << layer)) != 0;
        
        if (isEncounterLayer)
        {
            float rng = Random.Range(0f, 1f);
            //Debug.Log(rng);
            if (rng < 0.2f) Debug.Log("Encounter found!");

        }


    }
}
