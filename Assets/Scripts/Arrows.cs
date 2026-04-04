using UnityEngine;

public class Arrows : MonoBehaviour
{
    [SerializeField] float arrowSpeed = 5f;
    [SerializeField] float arrowExistTime = 1f;

    Rigidbody2D arrowRB;
    PlayerMovement player;
    float xSpeed;
    float direction;


    void Awake()
    {
        arrowRB = GetComponent<Rigidbody2D>();
        player = FindFirstObjectByType<PlayerMovement>();
        InitializeDirection();
        InitializeXSpeed();
    }

    void Update()
    {
        AddVelocityToArrow();
    }

    void InitializeDirection()
    {
        direction = player.transform.localScale.x;
        gameObject.transform.localScale = new Vector2(direction, 1f);
    }

    void InitializeXSpeed()
    {
        xSpeed = direction * arrowSpeed;
    }

    void AddVelocityToArrow()
    {
        arrowRB.linearVelocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, arrowExistTime);
    }
}
