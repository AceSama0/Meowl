using System.Runtime.InteropServices;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{    
    public static DontDestroy instance { get; private set; }
    private static GameObject[] persistentObjects = new GameObject[3];
    public int objectIndex;
    void Awake()
    {
        if (persistentObjects[objectIndex] == null)
        {
            persistentObjects[objectIndex] = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if (persistentObjects[objectIndex] != gameObject)
        {
            Destroy(gameObject);
        }
    }

}
