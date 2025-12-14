using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    
    void Start()
    {
        MusicManager.PlayBackgroundMusic(true);
    }
    public void StartGame()
    {
        
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void CreditsScene()
    {
        SceneManager.LoadScene("Credits");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit");
    }
}
