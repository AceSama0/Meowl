using UnityEngine;

public class PuzzleActivateCode : MonoBehaviour
{
    public GameObject PuzzleName;
    [SerializeField] private PuzzleSetSlots puzzleSetSlots; // Inspector'dan atayın

    void Start()
    {
        PuzzleName.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PuzzleName.SetActive(true);
            OnPuzzlePanelOpen();
            // Pause();
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


    public void Resume()
    {
        Time.timeScale = 1f;
    }
    void Pause()
    {
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PuzzleName.SetActive(false);
            // Resume();
        }
    }
}
