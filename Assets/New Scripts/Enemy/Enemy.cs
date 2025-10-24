using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public static Enemy instance { get; private set; }
    [SerializeField] float speed;
    float enemySpeed;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    Player player;
    [SerializeField] CapsuleCollider2D capsuleCollider2D;
    [SerializeField] Transform spwanPoint;
    float distance;
    [SerializeField] float delay;

    Vector2 moveDirection;
    bool isMoving;
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        enemySpeed = speed;
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        player = Player.instance;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position, 8f, LayerMask.GetMask("Player"));

        if (Physics2D.OverlapCircle(transform.position, 10f, LayerMask.GetMask("Player")))
        {
            SetDirection();
            // Flee();
            if (hit.distance < 5f && player.lightTime >= 0) // coroutineden çıkıyor if düzenlenmeli
            {
                StartCoroutine(FleeCor());
            }
        }
        else
        {
            SetDirection();
        }

        FlipObject();
        if (isMoving)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
    void FixedUpdate()
    {
        if (Physics2D.OverlapCircle(transform.position, 10f, LayerMask.GetMask("Player")))
        {
            Move();
            isMoving = true;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            isMoving = false;
        }
    }

    void SetDirection()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        moveDirection = direction;
        speed = enemySpeed;
        
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * speed;
    }
    void FlipObject()
    {
        if (player.transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
    
    IEnumerator FleeCor()
    { 
        spriteRenderer.enabled = false;
        capsuleCollider2D.enabled = false;
        speed = 0;
        transform.position = spwanPoint.position;
        yield return new WaitForSeconds(delay);

        spriteRenderer.enabled = true;
        capsuleCollider2D.enabled = true;
        // animator.SetBool("isWalking", false);

        yield return new WaitForSeconds(delay);
        // animator.SetBool("isWalking", true);
        speed = enemySpeed;
    }
}
