using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] int startDirection = 1;
    [SerializeField] float halfWidth;
    [SerializeField] int currentDirection;
    [SerializeField] float offset = 0.1f;
    [SerializeField] float speed = 6f;
    [SerializeField] Vector2 movement;

    void Start()
    {
        currentDirection = startDirection;
        spriteRenderer = GetComponent<SpriteRenderer>();
        halfWidth = spriteRenderer.bounds.extents.y;
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("Oyuncu tespit edildi sonraki sahneye geçiliyor");
        }
    }
    void FixedUpdate()
    {
        movement.x = speed * currentDirection;
        movement.y = rb.linearVelocity.y;
        rb.linearVelocity = movement;
        EnemyRaycast();
    }
    void EnemyRaycast()
    {
        if (Physics2D.Raycast(transform.position, Vector2.right, halfWidth + offset, LayerMask.GetMask("Ground")) && rb.linearVelocity.x > 0)
        {
            currentDirection *= -1;
            spriteRenderer.flipX = true;
        }
        else if (Physics2D.Raycast(transform.position, Vector2.left, halfWidth + offset, LayerMask.GetMask("Ground")) && rb.linearVelocity.x < 0)
        {
            currentDirection *= -1;
            spriteRenderer.flipX = false;
        }
    }
}
