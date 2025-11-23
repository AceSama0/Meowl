using System.Collections.Generic;
using UnityEngine;

public class PuzzleSetSlots : MonoBehaviour
{
    private ItemDictionary itemDictionary;
    private InventoryController inventoryController;
    [SerializeField] GameObject PuzzlePieces;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] int slotCount;
    [SerializeField] int[] requiredItemIDs;

    void Start()
    {
        itemDictionary = FindAnyObjectByType<ItemDictionary>();
        inventoryController = FindAnyObjectByType<InventoryController>();
        CreateEmptySlots();
    }
    
    void OnEnable()
    {
        if (itemDictionary == null)
            itemDictionary = FindAnyObjectByType<ItemDictionary>();
        
        if (inventoryController == null)
            inventoryController = FindAnyObjectByType<InventoryController>();
        
        Invoke(nameof(RefreshPuzzleDisplay), 0.1f);
    }

    void OnDisable()
    {
        ClearAllItems();
    }

    private void CreateEmptySlots()
    {
        for (int i = PuzzlePieces.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(PuzzlePieces.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, PuzzlePieces.transform);
        }
    }

    private void ClearAllItems()
    {
        Transform puzzleSlotContainer = PuzzlePieces.transform;
        
        for (int i = 0; i < puzzleSlotContainer.childCount; i++)
        {
            Transform slot = puzzleSlotContainer.GetChild(i);
            
            while (slot.childCount > 0)
            {
                DestroyImmediate(slot.GetChild(0).gameObject);
            }
        }
    }

    public void RefreshPuzzleDisplay()
    {
        if (inventoryController == null || itemDictionary == null) return;

        Transform puzzleSlotContainer = PuzzlePieces.transform;

        if (puzzleSlotContainer.childCount < requiredItemIDs.Length)
        {
            CreateEmptySlots();
        }

        ClearAllItems();

        List<InventorySaveData> currentInventory = inventoryController.GetInventoryItems();
        if (currentInventory == null) return;

        for (int i = 0; i < requiredItemIDs.Length && i < puzzleSlotContainer.childCount; i++)
        {
            int requiredID = requiredItemIDs[i];
            Transform targetSlot = puzzleSlotContainer.GetChild(i);

            bool itemFoundInInventory = currentInventory.Exists(data => data.itemID == requiredID);

            if (itemFoundInInventory)
            {
                GameObject itemPrefab = itemDictionary.GetItemPrefab(requiredID);

                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, targetSlot);
                    RectTransform rectTransform = item.GetComponent<RectTransform>();

                    if (rectTransform != null)
                    {
                        rectTransform.anchoredPosition = Vector2.zero;
                        rectTransform.localScale = Vector3.one;
                    }
                }
            }
        }
    }
}