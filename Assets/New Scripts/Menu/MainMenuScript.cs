using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void StartGame()
    {
        MusicManager.PlayBackgroundMusic(true);
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
