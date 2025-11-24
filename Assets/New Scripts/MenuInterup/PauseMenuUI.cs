using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public bool isGamePause = false;

    [SerializeField] GameObject PauseUI;
    InventoryScript inventoryScript;

    void Awake()
    {
        inventoryScript = FindAnyObjectByType<InventoryScript>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !inventoryScript.isAnotherScreenOpened)
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
