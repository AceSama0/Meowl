using System;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    void Awake()
    {
        instance = this;
        string sceneName = SceneManagerX.Instance.SceneName;
        if (SceneManagerX.Instance != null)
        {
            Debug.Log(sceneName);
        }
        else
        {
            Debug.Log("null");
        }
    }
}
