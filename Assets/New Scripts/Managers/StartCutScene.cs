using UnityEngine;

public class StartCutScene : MonoBehaviour
{
    Player player;
    [SerializeField] GameObject cutScene1;
    bool cutScenehappened = false;
    
    void Awake()
    {
        player = FindAnyObjectByType<Player>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !cutScenehappened)
        {
            player.canMove = false;
            player.GetComponent<SpriteRenderer>().enabled = false;
            cutScene1.SetActive(true);
            cutScenehappened = true;
        }
    }
}
