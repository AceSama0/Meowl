using UnityEngine;
using UnityEngine.SceneManagement;


public class MenüyeDönüş : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
