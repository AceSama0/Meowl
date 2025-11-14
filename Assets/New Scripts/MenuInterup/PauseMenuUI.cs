using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public static bool isGamePause = false;

    [SerializeField] GameObject PauseUI;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
    void Pause()
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
