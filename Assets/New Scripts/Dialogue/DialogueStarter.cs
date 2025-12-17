using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] GameObject dialogue;
    Player player;

    void Awake()
    {
        player = FindAnyObjectByType<Player>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.canMove = false;
            dialogue.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        player.canMove = true;
        Destroy(gameObject);
    }

    void Update()
    {
        if (!dialogue.activeInHierarchy)
        {
            player.canMove = true;
            
        }
    }
}
