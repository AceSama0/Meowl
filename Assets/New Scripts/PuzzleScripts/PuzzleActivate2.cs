using System.Collections;
using UnityEngine;

public class PuzzleActivate2 : MonoBehaviour
{
    public GameObject PuzzleName;
    InventoryScript inventoryScript;
    PauseMenuUI pause;
    Player player;
    public bool puzzleOpened = false;

    void Start()
    {
        player = FindAnyObjectByType<Player>();
        pause = FindAnyObjectByType<PauseMenuUI>();
        PuzzleName.SetActive(false);
        inventoryScript = FindAnyObjectByType<InventoryScript>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PuzzleName != null)
            {
                PuzzleName.SetActive(true);
            }
            puzzleOpened = true;    
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        puzzleOpened = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pause.isGamePause)
        {
            inventoryScript.isAnotherScreenOpened = false;
            player.canMove = true;
            PuzzleName.SetActive(false);
            return;
        }
    }

    
}
