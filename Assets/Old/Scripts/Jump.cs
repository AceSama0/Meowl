using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float jumpForce = 6f;
    [SerializeField] float doubleJumpForce = 6f;
    // [SerializeField] Vector2 wallJumpForce = new Vector2(4f, 8f);
    [SerializeField] float playerHalfHeight;
    [SerializeField] float playerHalfWidth;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float offset;
    [SerializeField] bool canDoubleJump;
    //private bool isGrounded;

    void Start()
    {
        playerHalfHeight = spriteRenderer.bounds.extents.y;
        playerHalfWidth = spriteRenderer.bounds.extents.x;
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            CheckJumpType();
        }
        // if (Input.GetButtonDown("Jump") && GetisGrounded())
        // {
        //     playerJump(jumpForce);
        // }
        // else if (Input.GetButtonDown("Jump") && !GetisGrounded() && canDoubleJump)
        // {
        //     rb.linearVelocity = Vector2.zero;
        //     rb.angularVelocity = 0;
        //     playerJump(doubleJumpForce);
        //     canDoubleJump = false;
        // }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        GetisGrounded();
    }

    void CheckJumpType()
    {
        bool isGrounded = GetisGrounded();
        if (isGrounded)
        {
            playerJump(jumpForce);
        }
        else
        {
            // int direction = isİtWall();
            // if (direction == 0)
            // {
                DoubleJump();
            // }
            // else if (direction != 0)
            // {
            //     // WallJump(direction);
            // }
        }
    }

    bool GetisGrounded()
    {
        bool hit = Physics2D.Raycast(transform.position, Vector2.down, playerHalfHeight + offset, LayerMask.GetMask("Ground"));
        if (hit)
        {
            canDoubleJump = true;
        }
        return hit;
    }

    // int isİtWall()
    // {
    //     if (Physics2D.Raycast(transform.position, Vector2.right, playerHalfWidth + offset, LayerMask.GetMask("Ground")))
    //     {
    //         return -1;
    //     }
    //     if (Physics2D.Raycast(transform.position, Vector2.left, playerHalfWidth + offset, LayerMask.GetMask("Ground")))
    //     {
    //         return 1;
    //     }
    //     return 0;
    // }

    void playerJump(float force)
    {
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    void DoubleJump()
    {
        if (canDoubleJump)
        {
            canDoubleJump = false;
            
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            playerJump(doubleJumpForce);
        }
    }
    // void WallJump(int direction)
    // {
    //     Vector2 force = wallJumpForce;
    //     force.x *= direction;
    //     rb.linearVelocity = Vector2.zero;
    //     rb.angularVelocity = 0f;
    //     rb.AddForce(force, ForceMode2D.Impulse);
    // }
    
}
