using UnityEngine;
using UnityEngine.SceneManagement;


public class MenüyeDönüş : MonoBehaviour
{
    [SerializeField] AudioClip creditsSong;
    void Start()
    {
        MusicManager.PlayBackgroundMusic(false,creditsSong);
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
