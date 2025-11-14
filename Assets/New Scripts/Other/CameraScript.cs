using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Player player;
    float minY = 0.4f;
    float followSpeed = 5f;
    void Awake()
    {
        player = FindAnyObjectByType<Player>();
    }
    
    void Update()
    {
        float targetY = player.transform.position.y;

        float clampedYMin = Mathf.Max(targetY, minY);

        Vector2 targetPosition = new Vector2(transform.position.x, clampedYMin);

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
