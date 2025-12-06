using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary ItemDictionary;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] int slotCount = 15; 
    
    private bool isInitialized = false;

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

    public bool AddItem(GameObject itemPrefab)
    {
        if (!isInitialized)
            InitializeInventory();
            
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            SlotScripts slot = slotTransform.GetComponent<SlotScripts>();
            
            if (slot != null && slot.currentImage == null)
            {
                // İtemi slota ekle
                GameObject item = Instantiate(itemPrefab, slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentImage = item;
                
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
    

    public void SetIventortyItems(List<InventorySaveData> inventorySaveData)
    {
        StopAllCoroutines(); 
        StartCoroutine(SafeLoadInventory(inventorySaveData)); 
    }

    IEnumerator SafeLoadInventory(List<InventorySaveData> inventorySaveData)
    {
        for (int i = inventoryPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventoryPanel.transform.GetChild(i).gameObject);
        }

        yield return null; 
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }
        
        isInitialized = true;

        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex < slotCount)
            {
                Transform slotTransform = inventoryPanel.transform.GetChild(data.slotIndex);
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
        
        Debug.Log($"Inventory loaded with {inventorySaveData.Count} items.");
    }
}