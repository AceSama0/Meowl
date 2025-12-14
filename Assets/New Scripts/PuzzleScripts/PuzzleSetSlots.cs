using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering; // WaitForEndOfFrame için eklendi

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
        if (cachedItemDictionary == null)
        {
            cachedItemDictionary = FindAnyObjectByType<ItemDictionary>();
        }
        itemDictionary = cachedItemDictionary;

        if (cachedInventoryController == null)
        {
            cachedInventoryController = FindAnyObjectByType<InventoryController>();
        }
        inventoryController = cachedInventoryController;

        StartCoroutine(CreateAllSlotsSafely());
    }

    void OnEnable()
    {
        if (itemDictionary == null)
        {
            itemDictionary = cachedItemDictionary ?? FindAnyObjectByType<ItemDictionary>();
            cachedItemDictionary = itemDictionary;
        }

        if (inventoryController == null)
        {
            inventoryController = cachedInventoryController ?? FindAnyObjectByType<InventoryController>();
            cachedInventoryController = inventoryController;
        }

        StartCoroutine(SafeRefreshPuzzle()); // 🛑 Hızlı ve güvenli yenileme Coroutine'ini çağır
    }

    void OnDisable()
    {
        StopAllCoroutines();
        // İtemları envantere geri döndür (eğer kalıcı olarak silinmemişlerse)
        ReturnItemsToInventory(); 
        ClearAllItems();
    }

    // RefreshPuzzleDisplay sadece Coroutine'i başlatır.
    public void RefreshPuzzleDisplay()
    {
        if (isRefreshing)
        {
            Debug.LogWarning("Puzzle zaten yenileniyor!");
            return;
        }

        if (inventoryController == null || itemDictionary == null)
        {
            Debug.LogError("Manager referansları eksik!");
            return;
        }

        StartCoroutine(SafeRefreshPuzzle());
    }

    private IEnumerator SafeRefreshPuzzle()
    {
        isRefreshing = true;

        ClearAllItems();
        yield return null; // Silme işleminin bitmesini bekle

        Transform puzzleSlotContainer = PuzzlePieces.transform;
        
        // 🛑 EK GÜVENLİK: Envanterin yüklenmesini bekleme döngüsü
        List<InventorySaveData> currentInventory = null;
        int attempts = 0;

        while ((currentInventory == null || currentInventory.Count == 0) && attempts < 5)
        {
            currentInventory = inventoryController.GetInventoryItems();
            if (currentInventory == null || currentInventory.Count == 0)
            {
                yield return new WaitForSeconds(0.1f); 
            }
            attempts++;
        }

        if (currentInventory == null || currentInventory.Count == 0)
        {
            Debug.LogWarning("❌ Inventory boş, puzzle yenilenemedi.");
            isRefreshing = false;
            yield break;
        }
        
        // Puzzle itemlarını yerleştir
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
                    
                    // 🛑 KRİTİK DÜZELTME: Item'ı SlotScripts'e ata (Drag/Drop için zorunlu)
                    if(targetSlot != null)
                    {
                        targetSlot.currentImage = item; 
                    }

                    RectTransform rectTransform = item.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        rectTransform.anchoredPosition = Vector2.zero;
                        rectTransform.localScale = Vector3.one;
                    }

                    // ✅ Item'ı inventory'den SİL (Puzzle'a çekildiği için)
                    inventoryController.RemoveItemByID(requiredID);
                }
            }
        }

        // Inventory UI'ının son durumu göstermesi için yenile
        if (inventoryController != null)
        {
            inventoryController.RefreshInventoryDisplay();
        }
        
        isRefreshing = false;
    }

    IEnumerator CreateAllSlotsSafely()
    {
        for (int i = PuzzlePieces.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(PuzzlePieces.transform.GetChild(i).gameObject);
        }
        yield return new WaitForEndOfFrame(); // Build için agresif bekleme

        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(slotPrefab, PuzzlePieces.transform);
            slot.name = $"PuzzleSlot_{i}";
        }

        yield return null;
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
            // SlotScripts'teki currentImage referansını temizle
            SlotScripts slotScript = slot.GetComponent<SlotScripts>();
            if(slotScript != null)
            {
                slotScript.currentImage = null;
            }
        }
    }

    public bool CheckForCorrectItem(SlotScripts targetSlot, int requiredItemID)
    {
        if (targetSlot == null || targetSlot.currentImage == null)
        {
            return false;
        }

        GameObject itemObject = targetSlot.currentImage;
        Item itemComponent = itemObject.GetComponent<Item>();

        if (itemComponent != null)
        {
            return itemComponent.ID == requiredItemID;
        }

        return false;
    }

    // 🛑 DÜZELTME: Items'ları geri döndürme metodu
    private void ReturnItemsToInventory()
    {
        if (inventoryController == null || itemDictionary == null) return;

        Transform puzzleSlotContainer = PuzzlePieces.transform;

        for (int i = 0; i < puzzleSlotContainer.childCount; i++)
        {
            Transform slot = puzzleSlotContainer.GetChild(i);

            // Item, slotun child'ı olarak atanmış olmalı
            if (slot.childCount > 0)
            {
                GameObject itemObject = slot.GetChild(0).gameObject;
                Item itemComponent = itemObject.GetComponent<Item>();

                if (itemComponent != null)
                {
                    // Item'ı envantere geri ekle
                    GameObject itemPrefab = itemDictionary.GetItemPrefab(itemComponent.ID);
                    if (itemPrefab != null)
                    {
                        // AddItem'ın true döndüğünden emin olmalıyız (envanter doluysa kaybolabilir)
                        inventoryController.AddItem(itemPrefab); 
                    }
                }
            }
        }
        inventoryController.RefreshInventoryDisplay(); 
    }
}