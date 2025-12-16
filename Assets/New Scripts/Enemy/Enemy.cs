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

    [Header("Dash")]
    public bool canDash = true;
    [SerializeField] bool isDashing;
    [SerializeField] float dashingDuration = 0.1f;
    [SerializeField] float dashingSpeed;
    [SerializeField] float dashCoolDown = 0.1f;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        target = FindAnyObjectByType<Player>().transform;
        originalSpeed = movementSpeed;
    }

    void Update()
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
            Vector3 direction = (target.transform.position - transform.position).normalized;
            moveDirection = direction;
        }
        if (Vector2.Distance(target.position, transform.position) < 10f && canDash && gameObject.name == "Daughter")
        {
            StartCoroutine(Dash());
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
        isDashing = true;
        yield return new WaitForSeconds(1);
        rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * dashingSpeed;
        yield return new WaitForSeconds(dashingDuration);
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        
    }
}
