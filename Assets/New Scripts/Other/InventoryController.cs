using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
                SoundEffectManager.Play("deneme");
                GameObject item = Instantiate(itemPrefab, slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentImage = item;
                RefreshInventoryDisplay();

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
        for (int i = inventoryPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(inventoryPanel.transform.GetChild(i).gameObject);
        }
        yield return null;

        List<InventorySaveData> sortedData = inventorySaveData.OrderBy(data => data.itemID).ToList();

        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

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
    }
}