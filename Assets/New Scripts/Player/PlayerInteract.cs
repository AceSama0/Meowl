using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private RaycastHit2D hit;
    [SerializeField] private float interactDistance;

    public enum FacingDirection { Right, Left, Up, Down }
    public FacingDirection currentDirection = FacingDirection.Right;
    

    void Update()
    {
        SetCurrentDirection();
        Vector2 direction = GetDirection();
        hit = Physics2D.Raycast(transform.position, direction , interactDistance , LayerMask.GetMask("Collectable"));
        // Debug.DrawRay()
        if (hit.collider != null)
        {
            GameObject item = hit.collider.gameObject;

            if (Input.GetKeyUp(KeyCode.E))
            {
                Debug.Log(item.name);
                Destroy(item);
            }
        }
    }

    void SetCurrentDirection()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentDirection = FacingDirection.Up;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentDirection = FacingDirection.Down;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentDirection = FacingDirection.Left;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentDirection = FacingDirection.Right;
        }
    }

    Vector2 GetDirection()
    {
        switch (currentDirection)
        {
            case FacingDirection.Right: return Vector2.right;
            case FacingDirection.Left: return Vector2.left;
            case FacingDirection.Up: return Vector2.up;
            case FacingDirection.Down: return Vector2.down;
            default: return Vector2.right;
        }
    }

}
