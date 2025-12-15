using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering; // WaitForEndOfFrame için

public class InventoryController : MonoBehaviour
{
    private ItemDictionary ItemDictionary;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] int slotCount = 15;

    private bool isInitialized = false;
    
    // UI Yenileme Durumları
    public bool isRefreshing = false; // Puzzle bunu okuyacak
    private bool refreshRequested = false; // Yenileme isteği bayrağı

    // 🔥 ANA VERİ KAYNAĞI: UI değil, bu liste gerçektir.
    private List<InventorySaveData> currentInventoryData = new List<InventorySaveData>();
    
    // Başlatma Güvenliği
    private bool isFullyReady = false;
    public bool IsFullyReady => isFullyReady; // Dışarıdan okunabilir
    private Queue<GameObject> pendingItems = new Queue<GameObject>(); // Bekleyen itemlar

    void Awake()
    {
        ItemDictionary = FindAnyObjectByType<ItemDictionary>();
        InitializeInventory();
    }

    void Start()
    {
        StartCoroutine(StartupRoutine());
    }
    
    // Oyun başlarken sistemin oturmasını bekler
    IEnumerator StartupRoutine()
    {
        yield return new WaitForEndOfFrame();
        
        isFullyReady = true;
        
        // Başlangıçta bekleyen item varsa şimdi ekle
        while (pendingItems.Count > 0)
        {
            GameObject item = pendingItems.Dequeue();
            if (item != null) AddItemInternal(item);
        }
    }

    void InitializeInventory()
    {
        if (inventoryPanel.transform.childCount > 0)
        {
            isInitialized = true;
            return;
        }

        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }
        isInitialized = true;
    }

    // 🛑 Her frame'in sonunda sadece 1 kez yenileme yapar.
    // Bu, "5 tane item üretme" hatasını engeller.
    void LateUpdate()
    {
        if (refreshRequested && !isRefreshing)
        {
            refreshRequested = false;
            StartCoroutine(SafeLoadInventory());
        }
    }
    
    // ========================================================================
    // PUBLIC METOTLAR
    // ========================================================================

    // Dışarıdan Item Ekleme Çağrısı
    public bool AddItem(GameObject itemPrefab)
    {
        // Sistem hazır değilse kuyruğa at
        if (!isFullyReady)
        {
            pendingItems.Enqueue(itemPrefab);
            return true;
        }
        return AddItemInternal(itemPrefab);
    }
    
    // Dışarıdan Yenileme İsteği
    public void RequestRefreshDisplay()
    {
        refreshRequested = true;
    }
    
    // Sadece Veri Listesini Döndürür (UI okumaz)
    public List<InventorySaveData> GetInventoryItems()
    {
        return new List<InventorySaveData>(currentInventoryData);
    }

    public void RemoveItemByID(int itemID)
    {
        InventorySaveData itemToRemove = currentInventoryData.Find(data => data.itemID == itemID);
        if (itemToRemove != null)
        {
            currentInventoryData.Remove(itemToRemove);
            RequestRefreshDisplay();
        }
    }

    public void SetIventortyItems(List<InventorySaveData> inventorySaveData)
    {
        if (inventorySaveData == null) return;
        currentInventoryData = new List<InventorySaveData>(inventorySaveData);
        RequestRefreshDisplay();
    }

    // ========================================================================
    // PRIVATE MANTIK
    // ========================================================================

    private bool AddItemInternal(GameObject itemPrefab)
    {
        if (!isInitialized) InitializeInventory();

        if (currentInventoryData.Count >= slotCount)
        {
            Debug.Log("Inventory is full.");
            return false;
        }

        Item itemComponent = itemPrefab.GetComponent<Item>();
        if (itemComponent == null) return false;

        // Veriyi listeye ekle
        currentInventoryData.Add(new InventorySaveData
        {
            itemID = itemComponent.ID,
            slotIndex = -1
        });

        RequestRefreshDisplay();
        return true;
    }

    // ========================================================================
    // UI YENİLEME (Build Güvenli)
    // ========================================================================
    
    private const int SLOTS_PER_ROW = 5;
    private const int ROW_ID_RANGE = 5;

    IEnumerator SafeLoadInventory()
    {
        isRefreshing = true;

        // 1. Tüm eski slotları sil
        for (int i = inventoryPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventoryPanel.transform.GetChild(i).gameObject);
        }

        // Build'de silme işleminin bitmesini bekle
        int safety = 0;
        while (inventoryPanel.transform.childCount > 0 && safety < 100)
        {
            yield return null;
            safety++;
        }
        yield return new WaitForEndOfFrame();

        // 2. Yeni boş slotları oluştur
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }
        yield return null; // Slotların UI'da yerleşmesi için bekle

        // 3. Veri listesine göre itemları yerleştir
        // Listeyi ID'ye göre sıralıyoruz
        List<InventorySaveData> sortedData = currentInventoryData.OrderBy(data => data.itemID).ToList();

        foreach (var data in sortedData)
        {
            int rowNumber = (Mathf.Max(1, data.itemID) - 1) / ROW_ID_RANGE;
            int columnPositionInRow = (Mathf.Max(1, data.itemID) - 1) % ROW_ID_RANGE;
            int newSlotIndex = (rowNumber * SLOTS_PER_ROW) + columnPositionInRow;

            if (newSlotIndex < slotCount && newSlotIndex < inventoryPanel.transform.childCount)
            {
                Transform slotTransform = inventoryPanel.transform.GetChild(newSlotIndex);
                SlotScripts slot = slotTransform.GetComponent<SlotScripts>();
                GameObject itemPrefab = ItemDictionary.GetItemPrefab(data.itemID);

                if (itemPrefab != null && slot != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentImage = item;
                }
            }
        }

        isRefreshing = false;
    }
}