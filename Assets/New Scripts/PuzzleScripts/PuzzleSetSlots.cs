using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PuzzleSetSlots : MonoBehaviour
{
    private ItemDictionary itemDictionary;
    private InventoryController inventoryController;
    [SerializeField] GameObject PuzzlePieces;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] int slotCount;
    [SerializeField] int[] requiredItemIDs;

    private bool isRefreshing = false;
    private static ItemDictionary cachedItemDictionary;
    private static InventoryController cachedInventoryController;

    void Awake()
    {
        if (cachedItemDictionary == null) cachedItemDictionary = FindAnyObjectByType<ItemDictionary>();
        itemDictionary = cachedItemDictionary;

        if (cachedInventoryController == null) cachedInventoryController = FindAnyObjectByType<InventoryController>();
        inventoryController = cachedInventoryController;

        StartCoroutine(CreateAllSlotsSafely());
    }

    void OnEnable()
    {
        if (itemDictionary == null) itemDictionary = cachedItemDictionary ?? FindAnyObjectByType<ItemDictionary>();
        if (inventoryController == null) inventoryController = cachedInventoryController ?? FindAnyObjectByType<InventoryController>();

        StartCoroutine(SafeRefreshPuzzle());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        ReturnItemsToInventory();
        ClearAllItems();
    }

    public void RefreshPuzzleDisplay()
    {
        if (isRefreshing) return;
        StartCoroutine(SafeRefreshPuzzle());
    }

    private IEnumerator SafeRefreshPuzzle()
    {
        isRefreshing = true;
        ClearAllItems();
        yield return null;

        // 🛑 AGRESİF BEKLEME: Envanter tamamen hazır olana kadar bekle
        while (inventoryController == null || !inventoryController.IsFullyReady)
        {
            yield return null; 
        }

        // 🛑 Envanterde aktif bir yenileme varsa bitmesini bekle
        while (inventoryController.isRefreshing)
        {
            yield return null;
        }

        // Artık veriyi güvenle çekebiliriz
        List<InventorySaveData> currentInventory = inventoryController.GetInventoryItems();
        Transform puzzleSlotContainer = PuzzlePieces.transform;

        if (currentInventory == null)
        {
             isRefreshing = false;
             yield break;
        }

        for (int i = 0; i < requiredItemIDs.Length && i < puzzleSlotContainer.childCount; i++)
        {
            int requiredID = requiredItemIDs[i];
            Transform targetSlotTransform = puzzleSlotContainer.GetChild(i);
            SlotScripts targetSlot = targetSlotTransform.GetComponent<SlotScripts>();

            InventorySaveData foundItem = currentInventory.Find(data => data.itemID == requiredID);

            if (foundItem != null)
            {
                GameObject itemPrefab = itemDictionary.GetItemPrefab(requiredID);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, targetSlotTransform);
                    if (targetSlot != null) targetSlot.currentImage = item;

                    RectTransform rectTransform = item.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        rectTransform.anchoredPosition = Vector2.zero;
                        rectTransform.localScale = Vector3.one;
                    }

                    // Item'ı envanterden sil
                    inventoryController.RemoveItemByID(requiredID);
                }
            }
        }
        
        // Envanter UI'ını güncelle
        inventoryController.RequestRefreshDisplay();
        isRefreshing = false;
    }

    IEnumerator CreateAllSlotsSafely()
    {
        for (int i = PuzzlePieces.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(PuzzlePieces.transform.GetChild(i).gameObject);
        }
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(slotPrefab, PuzzlePieces.transform);
            slot.name = $"PuzzleSlot_{i}";
        }
        yield return null;
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
            SlotScripts slotScript = slot.GetComponent<SlotScripts>();
            if (slotScript != null) slotScript.currentImage = null;
        }
    }

    private void ReturnItemsToInventory()
    {
        if (inventoryController == null || itemDictionary == null) return;

        Transform puzzleSlotContainer = PuzzlePieces.transform;
        for (int i = 0; i < puzzleSlotContainer.childCount; i++)
        {
            Transform slot = puzzleSlotContainer.GetChild(i);
            if (slot.childCount > 0)
            {
                GameObject itemObject = slot.GetChild(0).gameObject;
                Item itemComponent = itemObject.GetComponent<Item>();
                if (itemComponent != null)
                {
                    GameObject itemPrefab = itemDictionary.GetItemPrefab(itemComponent.ID);
                    if (itemPrefab != null) inventoryController.AddItem(itemPrefab);
                }
            }
        }
        inventoryController.RequestRefreshDisplay();
    }
    
    public bool CheckForCorrectItem(SlotScripts targetSlot, int requiredItemID)
    {
        if (targetSlot == null || targetSlot.currentImage == null) return false;
        GameObject itemObject = targetSlot.currentImage;
        Item itemComponent = itemObject.GetComponent<Item>();
        if (itemComponent != null) return itemComponent.ID == requiredItemID;
        return false;
    }
}