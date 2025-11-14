using UnityEngine;
using System.IO;
using Unity.Cinemachine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "savaData.json");
        LoadGame();

    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBoundary = FindAnyObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));

    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerTransform;
            FindAnyObjectByType<CinemachineConfiner2D>().BoundingShape2D = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();
        }
        else
        {
            SaveGame();
        }
    }
}
