
using UnityEngine;

public class PuzzleActivateCode : MonoBehaviour
{
    public GameObject PuzzleName;
    [SerializeField] private PuzzleSetSlots puzzleSetSlots;

    PauseMenuUI pause;
    Player player;
    // public bool activatedPuzzle;
    PuzzleActivate2 puzzleActivate2;
    void Start()
    {
        puzzleActivate2 = FindAnyObjectByType<PuzzleActivate2>();
        player = FindAnyObjectByType<Player>();
        pause = FindAnyObjectByType<PauseMenuUI>();
        PuzzleName.SetActive(false);

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PuzzleName != null)
            {
                PuzzleName.SetActive(true);
            }
            SoundEffectManager.Play("Puzzle");
            OnPuzzlePanelOpen();
            player.canMove = false;
            // activatedPuzzle = true;
            puzzleActivate2.puzzleOpened = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        puzzleActivate2.puzzleOpened = false;
    }
    void OnPuzzlePanelOpen()
    {
        if (puzzleSetSlots == null)
        {
            puzzleSetSlots = FindAnyObjectByType<PuzzleSetSlots>();
        }

        if (puzzleSetSlots != null)
        {
            puzzleSetSlots.RefreshPuzzleDisplay();
        }
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !pause.isGamePause)
        {
            player.canMove = true;
            PuzzleName.SetActive(false);
            return;
        }
    }

}
