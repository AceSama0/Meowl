using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public static PauseMenuUI instance { get; private set; }
    public bool isGamePause = false;

    [SerializeField] GameObject PauseUI;
    InventoryScript inventoryScript;
    PuzzleActivate2 puzzleActivate2;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        inventoryScript = FindAnyObjectByType<InventoryScript>();
        puzzleActivate2 = FindAnyObjectByType<PuzzleActivate2>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !puzzleActivate2.puzzleOpened && !inventoryScript.isAnotherScreenOpened)
        {
            if (isGamePause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }


    public void Resume()
    {
        inventoryScript.isAnotherScreenOpened = false;
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePause = false;
    }
    public void Pause()
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePause = true;

    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
