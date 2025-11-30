using System.Collections;
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

        StartCoroutine(CreateAllSlotsSafely());
    }

    void OnEnable()
    {
        if (itemDictionary == null)
            itemDictionary = FindAnyObjectByType<ItemDictionary>();
        if (inventoryController == null)
            inventoryController = FindAnyObjectByType<InventoryController>();
        
        RefreshPuzzleDisplay();
    }

    void OnDisable()
    {
        CancelInvoke(nameof(RefreshPuzzleDisplay));
        ClearAllItems(); 
    }


    public void RefreshPuzzleDisplay()
    {
        if (inventoryController == null || itemDictionary == null) 
        {
            Debug.LogError("Manager referansları eksik!");
            return;
        }

        
        ClearAllItems(); 

        Transform puzzleSlotContainer = PuzzlePieces.transform;
        List<InventorySaveData> currentInventory = inventoryController.GetInventoryItems();
        if (currentInventory == null) return; 

        for (int i = 0; i < requiredItemIDs.Length && i < puzzleSlotContainer.childCount; i++)
        {
            int requiredID = requiredItemIDs[i];
            Transform targetSlot = puzzleSlotContainer.GetChild(i);

            bool itemFoundInInventory = currentInventory.Exists(data => data.itemID == requiredID);

            if (itemFoundInInventory)
            {
                if (inventoryController.HideItem(requiredID))
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
    IEnumerator CreateAllSlotsSafely()
    {
        for (int i = PuzzlePieces.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(PuzzlePieces.transform.GetChild(i).gameObject);
        }
        yield return null; 

        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, PuzzlePieces.transform);
        }

        yield return null; 
        RefreshPuzzleDisplay();
    }

    

    private void ClearAllItems()
    {
        
        Transform puzzleSlotContainer = PuzzlePieces.transform;
        for (int i = 0; i < puzzleSlotContainer.childCount; i++)
        {
            Transform slot = puzzleSlotContainer.GetChild(i);
            for (int j = slot.childCount - 1; j >= 0; j--)
            {
                Destroy(slot.GetChild(j).gameObject);
            }
        }
    }
    
    public bool CheckForCorrectItem(SlotScripts targetSlot, int requiredItemID)
    {
        if (targetSlot.currentImage == null)
        {
            return false;
        }

        GameObject itemObject = targetSlot.currentImage;
        Item itemComponent = itemObject.GetComponent<Item>();

        if (itemComponent != null)
        {
            if (itemComponent.ID == requiredItemID)
            {
                return true;
            }
        }
        return false;
    }
}