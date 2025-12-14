using UnityEngine;

public class PlayerItemCollecter : MonoBehaviour
{
    private InventoryController inventoryController;

    void Awake()
    {
        inventoryController = FindAnyObjectByType<InventoryController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if(item != null)
            {
                bool itemAdded = inventoryController.AddItem(collision.gameObject);
                if (itemAdded)
                {
                    item.Pickup();
                    Destroy(collision.gameObject);
                    inventoryController.RefreshInventoryDisplay();
                }                
            }
        }
    }
}
