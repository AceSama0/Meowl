using UnityEngine;

public class PuzzleSlotVerifier : MonoBehaviour
{
    [SerializeField] public int requiredItemID; 
    
    private SlotScripts slotComponent; 

    void Awake()
    {
        slotComponent = GetComponent<SlotScripts>(); 
        
        if (slotComponent == null)
        {
            Debug.LogError("PuzzleSlotRules için SlotScripts bileşeni gerekiyor!");
        }
    }
    public bool VerifyItemRule()
    {
        if (slotComponent == null || slotComponent.currentImage == null)
        {
            return false; 
        }

        Item itemComponent = slotComponent.currentImage.GetComponent<Item>();

        if (itemComponent != null)
        {
            if (itemComponent.ID == requiredItemID)
            {
                Debug.Log("Doğru item");
                return true; 
            }
            else
            {
                Debug.Log("Yanlış item");
            }
        }
        return false; 
    }

    
}
