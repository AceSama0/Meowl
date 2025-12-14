using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering; // WaitForEndOfFrame için eklendi

public class InventoryController : MonoBehaviour
{
    private ItemDictionary ItemDictionary;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] int slotCount = 15;

    private bool isInitialized = false;
    private bool isRefreshing = false; // Yenileme kilit bayrağı

    void Start()
    {
        ItemDictionary = FindAnyObjectByType<ItemDictionary>(); 
        InitializeInventory();
    }


    void InitializeInventory()
    {
        if (inventoryPanel.transform.childCount > 0)
            return;

        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        isInitialized = true;
    }
    
    // AddItem metodu sadece item'ı ekler, UI'ı yenilemez.
    public bool AddItem(GameObject itemPrefab)
    {
        if (!isInitialized)
            InitializeInventory();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            SlotScripts slot = slotTransform.GetComponent<SlotScripts>();

            if (slot != null && slot.currentImage == null)
            {
                SoundEffectManager.Play("deneme");
                GameObject item = Instantiate(itemPrefab, slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentImage = item;
                
                // RefreshInventoryDisplay() Build hatalarını önlemek için burada çağrılmamalıdır.
                return true;
            }
        }

        Debug.Log("Inventory is full.");
        return false;
    }

    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            SlotScripts slot = slotTransform.GetComponent<SlotScripts>();
            if (slot != null && slot.currentImage != null)
            {
                Item item = slot.currentImage.GetComponent<Item>();
                if (item != null)
                {
                    invData.Add(new InventorySaveData
                    {
                        itemID = item.ID,
                        slotIndex = slotTransform.GetSiblingIndex()
                    });
                }
            }
        }

        return invData;
    }
    
    public void RefreshInventoryDisplay()
    {
        if (isRefreshing) return; // Zaten yenileniyorsa engelle

        List<InventorySaveData> currentItems = GetInventoryItems();
        SetIventortyItems(currentItems);
    }
    
    public void RemoveItemByID(int itemID)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            SlotScripts slot = slotTransform.GetComponent<SlotScripts>();

            if (slot != null && slot.currentImage != null)
            {
                Item item = slot.currentImage.GetComponent<Item>();
                if (item != null && item.ID == itemID)
                {
                    Destroy(slot.currentImage);
                    slot.currentImage = null;

                    return;
                }
            }
        }
    }


    public void SetIventortyItems(List<InventorySaveData> inventorySaveData)
    {
        if (inventorySaveData == null)
        {
            return;
        }

        StopAllCoroutines();
        StartCoroutine(SafeLoadInventory(inventorySaveData));
    }

    //Load Inventory
    private const int SLOTS_PER_ROW = 5;
    private const int ROW_ID_RANGE = 5;
    
    IEnumerator SafeLoadInventory(List<InventorySaveData> inventorySaveData)
    {
        isRefreshing = true; // KİLİTLE: Yenileme başladı

        // 1. Tüm eski slotları ve içindeki itemları sil
        for (int i = inventoryPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventoryPanel.transform.GetChild(i).gameObject);
        }
        
        // 🛑 KRİTİK DÜZELTME: Tüm Destory işlemlerinin bitmesini aktif olarak bekle
        int safetyCounter = 0;
        // Çocuk sayısı 0'dan büyük olduğu sürece (ve sonsuz döngüden kaçınmak için sayaca bak)
        while (inventoryPanel.transform.childCount > 0 && safetyCounter < 100) 
        {
            yield return null; // Bir sonraki frame'i bekle
            safetyCounter++;
        }
        
        // Ekstra güvenlik için bir frame daha bekle
        yield return new WaitForEndOfFrame(); 

        // 2. Veriyi sırala ve yeni slotları oluştur
        List<InventorySaveData> sortedData = inventorySaveData.OrderBy(data => data.itemID).ToList();

        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        yield return null; // Yeni slotların yerleşmesi için bekle

        // 3. İtemları yerleştir
        for (int i = 0; i < sortedData.Count; i++)
        {
            InventorySaveData data = sortedData[i];

            int rowNumber = (Mathf.Max(1, data.itemID) - 1) / ROW_ID_RANGE;
            int columnPositionInRow = (Mathf.Max(1, data.itemID) - 1) % ROW_ID_RANGE;
            int newSlotIndex = (rowNumber * SLOTS_PER_ROW) + columnPositionInRow;


            if (newSlotIndex < slotCount)
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
        
        isRefreshing = false; // KİLİDİ AÇ: Yenileme bitti
    }
}