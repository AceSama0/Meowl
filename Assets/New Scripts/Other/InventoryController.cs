using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class InventoryController : MonoBehaviour
{
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] int slotCount;
    [SerializeField] GameObject[] itemPrefabs;

    void Start()
    {
        for (int i = 0; i < slotCount;i++)
        {
            SlotScripts slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<SlotScripts>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slotPrefab.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentImage = item;
            }
        }
    }
}
