using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary ItemDictionary;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] int slotCount;
    [SerializeField] GameObject[] itemPrefabs;

    void Start()
    {
        ItemDictionary = FindAnyObjectByType<ItemDictionary>();

    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach(Transform slotTransform in inventoryPanel.transform)
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
        Debug.Log("Inventory is full.");
        return false;
    }

    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            SlotScripts slot = slotTransform.GetComponent<SlotScripts>();
            if (slot.currentImage != null)
            {
                Item item = slot.currentImage.GetComponent<Item>();
                invData.Add(new InventorySaveData { itemID = item.ID, slotIndex = slotTransform.GetSiblingIndex() });
            }
        }
        return invData;
    }
    public bool HideItem(int itemID)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            SlotScripts slot = slotTransform.GetComponent<SlotScripts>();

            if (slot != null && slot.currentImage != null)
            {
                Item item = slot.currentImage.GetComponent<Item>();

                if (item != null && item.ID == itemID)
                {
                    slot.currentImage.SetActive(false);
                
                    Debug.Log($"Item ID {itemID} ana envanterden puzzle için gizlendi.");
                    return true; 
                }
            }
        }
        return false;
    }

    public void SetIventortyItems(List<InventorySaveData> inventorySaveData)
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }
        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex < slotCount)
            {
                SlotScripts slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<SlotScripts>();
                GameObject itemPrefab = ItemDictionary.GetItemPrefab(data.itemID);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentImage = item;
                }
            }
        }

    }
}
