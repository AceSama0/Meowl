using UnityEngine;
using System.IO;
using Unity.Cinemachine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController InventoryController;
    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "savaData.json");
        Debug.Log("" + saveLocation);
        InventoryController = FindAnyObjectByType<InventoryController>();
        LoadGame();

    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBoundary = FindAnyObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name,
            InventorySaveData = InventoryController.GetInventoryItems()
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
            InventoryController.SetIventortyItems(saveData.InventorySaveData);  
        }
        else
        {
            SaveGame();
        }
    }
}
