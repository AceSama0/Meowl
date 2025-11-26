using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public bool isGamePause = false;

    [SerializeField] GameObject PauseUI;
    //Ölüm ekranından sonra 3 tane buton çıkıcak burada 3 buton çık baştan başla ve menü olucak
    
    public void RestarT()
    {
        SceneManager.LoadScene("House");
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
