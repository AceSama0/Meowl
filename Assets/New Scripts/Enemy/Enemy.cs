using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    public bool canMove = true;
    public float movementSpeed = 2f;
    Vector2 moveDirection;
    Transform target;
    public float originalSpeed { get; private set; }
    public Animator animator;

    [Header("Dash")]
    public bool canDash = true;
    [SerializeField] public bool isDashing;
    [SerializeField] float dashingDuration = 0.1f;
    [SerializeField] float dashingSpeed;
    [SerializeField] float dashCoolDown = 0.1f;



    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        target = FindAnyObjectByType<Player>().transform;
        originalSpeed = movementSpeed;
    }

    void Update()
    {
        if (canMove)
        {
            if (isDashing)
            {
                animator.SetBool("Jump", true);
                return;
            }
            if (target)
            {
                animator.SetBool("Jump", false);
                Vector3 direction = (target.transform.position - transform.position).normalized;
                moveDirection = direction;
            }
            if (Vector2.Distance(target.position, transform.position) < 10f && canDash && gameObject.name == "Daughter")
            {
                StartCoroutine(Dash());
            }
        }


    }
    void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }
        if (isDashing)
        {
            return;
        }
        if (target)
        {
            rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * movementSpeed;
        }
    }
    IEnumerator Dash()
    {
        canDash = false;
        if (gameObject.name == "Daughter") animator.SetBool("Jump", true);
        isDashing = true;
        rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * dashingSpeed;
        yield return new WaitForSeconds(dashingDuration);
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        if (gameObject.name == "Daughter") animator.SetBool("Jump", false);

    }
}
