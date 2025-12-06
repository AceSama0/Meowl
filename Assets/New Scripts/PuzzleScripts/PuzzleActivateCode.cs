using UnityEngine;

public class PuzzleActivateCode : MonoBehaviour
{
    public GameObject PuzzleName;
    [SerializeField] private PuzzleSetSlots puzzleSetSlots;
    InventoryScript inventoryScript;
    PauseMenuUI pause;

    void Start()
    {
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
            OnPuzzlePanelOpen();
            inventoryScript.isAnotherScreenOpened = true;
        }
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
        
        if (Input.GetKeyDown(KeyCode.Escape) && inventoryScript.isAnotherScreenOpened && !pause.isGamePause)
        {
            PuzzleName.SetActive(false);
            inventoryScript.isAnotherScreenOpened = false;
        }
    }
    //Çalıştırıldığında diğer yerlere haber ver
}
