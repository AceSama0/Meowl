using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] SpriteRenderer spriteRenderer;
    private UnityEngine.Vector2 movement;
    private UnityEngine.Vector2 screenBounds;
    private float playerHalfWidth;
    [SerializeField] float xPosLastFrame;
    [SerializeField] Animator animator;
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new UnityEngine.Vector2(Screen.width, Screen.height));
        playerHalfWidth = spriteRenderer.bounds.extents.x;
        
    }

    void Update()
    {
        playerMovement();
        //ClampX();
        FlipcharacterX();

    }
    void playerMovement()
    {
        float input = Input.GetAxisRaw("Horizontal");
        movement.x = input * speed * Time.deltaTime;
        transform.Translate(movement);
        if (input != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void ClampX()
    {
        float clampX = Mathf.Clamp(transform.position.x, -screenBounds.x + playerHalfWidth, screenBounds.x - playerHalfWidth);
        UnityEngine.Vector2 pos = transform.position;
        pos.x = clampX;
        transform.position = pos;
    }
    void FlipcharacterX()
    {
        float input = Input.GetAxis("Horizontal");
        if (input > 0 && transform.position.x > xPosLastFrame)
        {
            spriteRenderer.flipX = false;
        }
        else if (input < 0 && transform.position.x < xPosLastFrame)
        {
            spriteRenderer.flipX = true;
        }
        xPosLastFrame = transform.position.x;
    }
    
}
