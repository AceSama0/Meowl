using UnityEngine;

public class PuzzleControlerActivater : MonoBehaviour
{
    [SerializeField] GameObject puzzleController;
    void Awake()
    {
        puzzleController.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(puzzleController != null) puzzleController.SetActive(true);
        }
    }
    
}
