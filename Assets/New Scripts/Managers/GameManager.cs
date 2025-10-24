using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    LevelManager levelManager;
    Player player;
    //public bool canvas;
    public bool doorBool = false;

    [SerializeField] GameObject canvasObject;


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
    }
    void Start()
    {
        
    }
    void Update()
    {
        // if (canvas)
        // {
        //     canvasObject.SetActive(true);
        // }
        // else
        // {
        //     canvasObject.SetActive(false);
        // }
    }

    void RestatrtScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    public void PlayerPos()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (player != null)
        {
            if (sceneName == "Room1")
            {
                player.transform.position = new Vector3(1.15f, -11f, 0);
            }
            else if (sceneName == "Corridor")
            {
                player.transform.position = new Vector3(0, -4f, 0);
            }
            else if (sceneName == "Room2")
            {
                player.transform.position = new Vector3(0, -3f, 0);
            }
            else if (sceneName == "Room3")
            {

            }
            else if (sceneName == "Room4")
            {

            }
            else if (sceneName == "Room5")
            {

            }

        }
    }
}
