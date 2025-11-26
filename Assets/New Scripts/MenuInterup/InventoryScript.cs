using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] GameObject InventoryUI;
    public bool isAnotherScreenOpened = false;
    [SerializeField]PauseMenuUI pauseMenuUI;
    void Start()
    {
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
