using UnityEngine;

public class PuzzleActivateCode : MonoBehaviour
{
    public GameObject PuzzleName;
    [SerializeField] private PuzzleSetSlots puzzleSetSlots;
    InventoryScript inventoryScript;

    void Start()
    {
        PuzzleName.SetActive(false);
        inventoryScript = FindAnyObjectByType<InventoryScript>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PuzzleName.SetActive(true);
            OnPuzzlePanelOpen();
            inventoryScript.isAnotherScreenOpened = true;
            Pause();
        }
    }

    
    
    void OnPuzzlePanelOpen()
    {
        // Null kontrolü ekleyin
        if (puzzleSetSlots == null)
        {
            puzzleSetSlots = FindAnyObjectByType<PuzzleSetSlots>();
            Debug.LogWarning("PuzzleSetSlots Inspector'dan atanmamış, FindAnyObjectByType ile bulundu");
        }
        
        if (puzzleSetSlots != null)
        {
            puzzleSetSlots.RefreshPuzzleDisplay();
        }
        else
        {
            Debug.LogError("PuzzleSetSlots bulunamadı!");
        }
    }

    void Pause()
    {
        Time.timeScale = 0f;
    }
    void Resume()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PuzzleName.SetActive(false);
            inventoryScript.isAnotherScreenOpened = false;
            Resume();
        }
    }
    //Çalıştırıldığında diğer yerlere haber ver
}
