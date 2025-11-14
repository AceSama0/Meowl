using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerX : MonoBehaviour
{
    public static SceneManagerX Instance { get; private set;}
    public string SceneName;
    void Awake()
    {
        Instance = this;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneName = currentScene.name;
    }
}
