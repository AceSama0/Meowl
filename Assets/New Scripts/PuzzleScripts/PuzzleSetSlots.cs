using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class PuzzleSetSlots : MonoBehaviour
{
    [SerializeField] GameObject PuzzlePieces;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] int slotCount;
    [SerializeField] GameObject[] itemPrefabs;
    void Start()
    {
        SetSlots();
    }

    private void SetSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            InventorySlot slot = Instantiate(slotPrefab, PuzzlePieces.transform).GetComponent<InventorySlot>();

            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
        }
    }
}
