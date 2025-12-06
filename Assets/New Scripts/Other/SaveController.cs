using UnityEngine;
using System.IO;
using Unity.Cinemachine;
using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    [SerializeField] GameObject saveUI;

    private InventoryController InventoryController;
    void Awake()
    {
        InventoryController = FindAnyObjectByType<InventoryController>();

    }
    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "savaData.json");
        // Debug.Log("" + saveLocation);
    }
    public void NewGame()
    {
        if (File.Exists(saveLocation))
        {
            File.Delete(saveLocation);
        }
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }


    public void SaveGame()
    {
        StartCoroutine(ShowSaving());
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
            NewGame();
        }
    }

    IEnumerator ShowSaving()
    {
        saveUI.SetActive(true);
        yield return new WaitForSeconds(2);
        saveUI.SetActive(false);
    }
}
