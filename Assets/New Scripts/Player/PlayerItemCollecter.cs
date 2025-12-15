using UnityEngine;

public class PlayerItemCollecter : MonoBehaviour
{
    private InventoryController inventoryController;
    private bool isProcessing = false; // Çifte toplamayı önleyen kilit

    void Awake()
    {
        inventoryController = FindAnyObjectByType<InventoryController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isProcessing) return; // Zaten işlem yapılıyorsa çık

        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                isProcessing = true; // Kilitle

                // AddItem çağrısı içeride Refresh'i tetikler
                bool itemAdded = inventoryController.AddItem(collision.gameObject);

                if (itemAdded)
                {
                    item.Pickup(); 
                    Destroy(collision.gameObject);
                }
                
                // Güvenlik için kısa bir süre sonra kilidi aç
                Invoke(nameof(ResetProcessing), 0.1f);
            }
        }
    }

    void ResetProcessing()
    {
        isProcessing = false;
    }
}