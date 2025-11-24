using UnityEngine;

public class FindDoor : MonoBehaviour
{
    Player player;
    Vector3 moveDirection;
    Rigidbody2D rb;

    [SerializeField] float movementSpeed = 2f;

    void Awake()
    {
        player = FindAnyObjectByType<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        moveDirection = direction;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
            rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * movementSpeed;
        
    }
}
