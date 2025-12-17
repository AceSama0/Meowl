using System.Collections.Generic;
using UnityEngine;  


public class InventoryController : MonoBehaviour
{

    private ItemDictionary ItemDictionary;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] int slotCount = 15;
    public GameObject[] itemPrefabs;



    void Awake()
    {
        ItemDictionary = FindAnyObjectByType<ItemDictionary>();
    }
    public bool AddItem(GameObject itemPrefab)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            SlotScripts slot = slotTransform.GetComponent<SlotScripts>();
            if(slot != null && slot.currentImage == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentImage = newItem;
                return true;
            }
        }
        Debug.Log("Inventory is full");
        return false;
    }

    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();
        
        // Slotların Parent'ı üzerinde döngü
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            SlotScripts slot = slotTransform.GetComponent<SlotScripts>();
            
            // 🛑 DÜZELTME 1: slot null ise atla (Line 46 hatası çözüldü)
            if (slot == null) continue;
            
            // Slot içinde item varsa
            if (slot.currentImage != null)
            {
                // 🛑 DÜZELTME 2: Item bileşenini item objesinin üzerinden al (Yanlış yer düzeltildi)
                Item item = slot.currentImage.GetComponent<Item>(); 
                
                // Item scripti yoksa NRE fırlatmaması için son bir kontrol
                if(item != null)
                {
                    invData.Add(new InventorySaveData { itemID = item.ID, slotIndex = slot.transform.GetSiblingIndex() });
                }
            }
        }
        return invData;
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
                    // Eğer AddItem listenizi kullanmıyorsanız bu yeterlidir.
                    // Eğer AddItem liste kullanıyorsa, buraya listeden çıkarma kodu da eklenmeli.
                    return; 
                }
            }
        }
    }

    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        foreach(InventorySaveData data in inventorySaveData)
        {
            if(data.slotIndex < slotCount)
            {
                SlotScripts slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<SlotScripts>();
                GameObject itemPrefab = ItemDictionary.GetItemPrefab(data.itemID);
                if(itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentImage = item;
                }
            }
        }
    }


}