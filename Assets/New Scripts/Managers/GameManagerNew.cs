using UnityEngine;

public class GameManagerNew : MonoBehaviour
{
    Player player;
    [SerializeField] Vector3 playerPosition;
    void Awake()
    {
        player = FindAnyObjectByType<Player>();
    }
    void Start()
    {
        player.transform.position = playerPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
