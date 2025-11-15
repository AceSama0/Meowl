using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] GameObject InventoryUI;
    void Start()
    {
        InventoryUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryUI.SetActive(!InventoryUI.activeSelf);
        }
    }
}
