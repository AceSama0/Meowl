using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float movementSpeed = 2f;
    Vector2 moveDirection;
    Transform target;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        target = FindAnyObjectByType<Player>().transform;
    }

    void Update()
    {
        if (target)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            moveDirection = direction;
        }
    }
    void FixedUpdate()
    {
        if(target)
        {
            rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * movementSpeed;
        }
    }
}
