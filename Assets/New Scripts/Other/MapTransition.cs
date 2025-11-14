using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
public class MapTransition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundry;
    private CinemachineConfiner2D confiner;
    [SerializeField]Direction direction;
    enum Direction{Up,Down,Left,Right}
    void Awake()
    {
        confiner = FindAnyObjectByType<CinemachineConfiner2D>();    
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            confiner.BoundingShape2D = mapBoundry;
            UpdatePlayerPosition(collision.gameObject);
        }
    }

    void UpdatePlayerPosition(GameObject player)
    {
        Vector3 playerPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                playerPos.y += 2.5f;
                break;
            case Direction.Down:
                playerPos.y -= 2.5f;
                break;
            case Direction.Left:
                playerPos.x -= 2.5f;
                break;
            case Direction.Right:
                playerPos.x += 2.5f;
                break;
        }

        player.transform.position = playerPos;
    }

}
