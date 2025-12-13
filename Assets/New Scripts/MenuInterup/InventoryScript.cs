using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] GameObject InventoryUI;
    PauseMenuUI pauseMenuUI;
    public bool isAnotherScreenOpened;
    void Start()
    {
        pauseMenuUI = FindAnyObjectByType<PauseMenuUI>();
        InventoryUI.SetActive(false);
    }

    void Update()
    {
        if (pauseMenuUI.isGamePause)
        {
            InventoryUI.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && !isAnotherScreenOpened && !pauseMenuUI.isGamePause)
        {
            InventoryUI.SetActive(!InventoryUI.activeSelf);
        }
    }
}
