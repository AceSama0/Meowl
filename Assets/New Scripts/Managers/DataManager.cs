using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    private List<string> scenesList = new List<string>();
    Player player;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        player = FindAnyObjectByType<Player>();
    }

    void Start()
    {
        if (SceneManagerX.Instance.SceneName == "Kitchen")
        {
            player.transform.position = new Vector3(0, 0, 0);
        }
    }

    
}
